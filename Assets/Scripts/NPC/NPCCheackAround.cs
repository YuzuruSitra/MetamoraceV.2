using System.Collections;
using System.Collections.Generic;
using System.Network;
using Block;
using Photon.Pun;
using UnityEngine;

namespace NPC
{
    public class NPCCheackAround : MonoBehaviour
    {
        private BlockBase _currentBlock;
        private BlockBehaviour _currentBlock1;
     private Vector3 _upPadding = new Vector3(0f,0.5f,0f);
     [SerializeField] 
    private float _npcReach = 1.0f;
    [SerializeField]
    private float _jumprayrength = 0.15f;
    bool _onGround;
    [SerializeField] 
    private float _verticalRayOffset = 2.0f;
    [SerializeField] 
    private float _horizontalRayOffset = 0.5f;
    [SerializeField] 
    private float _deathDecisionRayRange = 0.15f;
    private bool _verticalDeath = false;
    public bool VerticalDeath => _verticalDeath;
    private bool _horizontalDeath = false;
    public bool HorizontalDeath => _horizontalDeath;
    private bool _isNPCDeath = false;
    public bool IsPlayerDeath => _isNPCDeath;
    [SerializeField] private NPCEffectHandler _npcEffectHandler;
    private int _teamId;
    [SerializeField]
    float MaxBlockDetection = 8.0f;
    float RightdistanceToHitObject = 0;
    float LeftdistanceToHitObject = 0;
    float _vericalAvoidRayLengh = 6.0f;
    private bool _overBlock = false;
    [SerializeField] private NPCMover _npcMover;
    void Start()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
        _teamId = (int)teamId;
    }
    void Update()
    {
        JudgeVerticalDeath();
        JudgeHorizontalDeath();
       
    }
    //一番近いブロックnの方向を取得
    public string CheckArroundBlock()
    {
        Ray Rightray = new Ray(transform.position + _upPadding, Vector3.right);
        Ray Leftray = new Ray(transform.position + _upPadding, Vector3.left);
        Debug.DrawRay(transform.position + _upPadding, Vector3.right * MaxBlockDetection, Color.blue, 0.1f);
        Debug.DrawRay(transform.position + _upPadding, Vector3.left * MaxBlockDetection, Color.yellow, 0.1f);

        bool rightHit = Physics.Raycast(Rightray, out RaycastHit righthit, MaxBlockDetection) &&
                        (righthit.collider.CompareTag("Ambras") || righthit.collider.CompareTag("Heros"));
        bool leftHit = Physics.Raycast(Leftray, out RaycastHit lefthit, MaxBlockDetection) &&
                       (lefthit.collider.CompareTag("Ambras") || lefthit.collider.CompareTag("Heros"));

        RightdistanceToHitObject = rightHit ? righthit.distance : MaxBlockDetection;
        LeftdistanceToHitObject = leftHit ? lefthit.distance : MaxBlockDetection;

        if (!rightHit && !leftHit)
        {
            // Debug.Log(rightHit);
            // Debug.Log(leftHit);
            return null;
        }
        return LeftdistanceToHitObject <= RightdistanceToHitObject ? "Left" : "Right";
    }
    
    //縦の圧死を防ぐレイ
    public bool CheckVerticalDeathBlock()
    {
         _overBlock = false;
        //頭上検出Ray
        //頭上にブロックがあればよける、両方がブロックで挟まれている場合はジャンプ
        Vector3 rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
        Ray rayover = new Ray(rayOrigin, Vector3.up);
        Debug.DrawRay(rayover.origin, rayover.direction * _vericalAvoidRayLengh, Color.green);

        if (Physics.Raycast(rayover, out RaycastHit hitblock, _vericalAvoidRayLengh) )
        {
            if (hitblock.collider.CompareTag("Ambras")||hitblock.collider.CompareTag("Heros"))
            {
                _overBlock = true;
            }
            else _overBlock = false;
        }
        return _overBlock;
    }

    //奥行きの圧死を回避するレイ
    public bool CheckHorizontalDeathBlock()
    {
        bool _depthBlock = false;
       
        Vector3 rayOrigin = transform.position + Vector3.up * _horizontalRayOffset;
        Ray raydepth = new Ray(rayOrigin, Vector3.forward);
        Debug.DrawRay(raydepth.origin, raydepth.direction * _vericalAvoidRayLengh, Color.yellow);
    
        if (Physics.Raycast(raydepth, out RaycastHit hitblock, _vericalAvoidRayLengh) )
        {
            if (hitblock.collider.CompareTag("Ambras")||hitblock.collider.CompareTag("Heros"))
            {
                _depthBlock = true;
            }
            else _depthBlock = false;
        }
        return _depthBlock;
    }
    public BlockBase CheckBlockRay()
    {
        Vector3 npcDirection = transform.forward;
        npcDirection.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, npcDirection);
        Debug.DrawRay(transform.position + _upPadding, npcDirection, Color.red, 1.0f);
        if (!Physics.Raycast(ray, out RaycastHit hit, _npcReach)) return null;
        _currentBlock = hit.collider.GetComponent<BlockBase>();
        return _currentBlock;
    }
    public BlockBehaviour CheckBlockRay1()
    {
        Vector3 npcDirection = transform.forward;
        npcDirection.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, npcDirection);
        Debug.DrawRay(transform.position + _upPadding, npcDirection, Color.red, 1.0f);
        if (!Physics.Raycast(ray, out RaycastHit hit, _npcReach)) return null;
        _currentBlock1 = hit.collider.GetComponent<BlockBehaviour>();
        return _currentBlock1;
    }
    
    //縦方向の死亡判定
    private void JudgeVerticalDeath()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
        Ray ray = new Ray(rayOrigin, Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction * _deathDecisionRayRange, Color.green);

        if (!Physics.Raycast(ray, out RaycastHit hit, _deathDecisionRayRange)) return;
        if (!_npcMover.IsGrounded) return;
        if (_isNPCDeath) return;
        
            _verticalDeath = true;
            _isNPCDeath = true;
            _npcEffectHandler.ChangeDie(true);
            Debug.Log("DeathVertical");
            //StartCoroutine(ChangePhysics());
    }
    //奥行きの死亡判定
    private void JudgeHorizontalDeath()
    {
        Vector3 rayDirection = (_teamId == 0) ? Vector3.forward : Vector3.back;
        //Vector3 rayDirection = Vector3.left;
        Vector3 rayOrigin = transform.position + new Vector3(0f, 0.5f, 0f);
        Ray ray = new Ray(rayOrigin, rayDirection);
    
        Debug.DrawRay(rayOrigin, rayDirection * _deathDecisionRayRange, Color.red, 1.0f);

        if (!Physics.Raycast(ray, out RaycastHit hit, _deathDecisionRayRange)) return;
        if (_isNPCDeath) return;        
            _horizontalDeath = true;
            _isNPCDeath = true;
            Debug.Log("DeathHorizontal");
            _npcEffectHandler.ChangeDie(true);
            //StartCoroutine(ChangePhysics());
        
    }
    }
}
