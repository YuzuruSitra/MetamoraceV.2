using System.Collections;
using System.Collections.Generic;
using Block;
using UnityEngine;

public class PlayerCheakAround : MonoBehaviour
{
    private BlockBase _currentBlock;
     private Vector3 _upPadding = new Vector3(0f,0.5f,0f);
     [SerializeField] 
    private float _playerReach = 1.0f;
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
    private bool _isPlayerDeath = false;
    public bool IsPlayerDeath => _isPlayerDeath;

    void Update()
    {
        JudgeVerticalDeath();
        JudgeHorizontalDeath();
    }
    public BlockBase CheckBlockRay(Vector3 direction)
    {
        Ray ray = new Ray(transform.position + _upPadding, direction);
        Debug.DrawRay(transform.position + _upPadding, direction, Color.red, 1.0f);
        if (!Physics.Raycast(ray, out RaycastHit hit, _playerReach)) return null;
        _currentBlock = hit.collider.GetComponent<BlockBase>();
        return _currentBlock;
    }

    public bool CheakGroundRay()
    {
        float raypos = 0.45f;
        float rayheight = 0.1f;
        _onGround = CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, raypos), Vector3.down)) ||
                    CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, -raypos), Vector3.down)) ||
                    CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, -raypos), Vector3.down)) ||
                    CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, raypos), Vector3.down));
        return _onGround;
    }
    // 地上にいるか判定
    private bool CheckAndJump(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _jumprayrength, Color.red, 0.1f);
        return Physics.Raycast(ray, out hit, _jumprayrength);
    }
    //縦方向の死亡判定
    private void JudgeVerticalDeath()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
        Ray ray = new Ray(rayOrigin, Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction * _deathDecisionRayRange, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, _deathDecisionRayRange) && _onGround) 
        {
            _verticalDeath = true;
            _isPlayerDeath = true;
            Debug.Log("DeathVertical");
            //StartCoroutine(ChangePhysics());
        }
    }
    //奥行きの死亡判定
    private void JudgeHorizontalDeath()
    {
        //Vector3 rayDirection = (_playerDataReceiver.MineTeamID == 0) ? Vector3.left : Vector3.right;
        Vector3 rayDirection = Vector3.left;
        Vector3 rayOrigin = transform.position + new Vector3(0f, 0.5f, 0f);
        Ray ray = new Ray(rayOrigin, rayDirection);
    
        Debug.DrawRay(rayOrigin, rayDirection * _deathDecisionRayRange, Color.blue, 1.0f);
    
        if (Physics.Raycast(ray, out RaycastHit hit, _deathDecisionRayRange))
        {
            _horizontalDeath = true;
            _isPlayerDeath = true;
            Debug.Log("DeathHorizontal");
            //StartCoroutine(ChangePhysics());
        }
    }

    // private IEnumerator ChangePhysics()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    //     _col.isTrigger = true;
    //     _myPV.RPC(nameof( _playerEffectHangler.ChangeDie), PhotonTargets.All, true);
    // }
}
