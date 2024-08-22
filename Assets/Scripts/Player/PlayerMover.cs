using System.Collections;
using System.Collections.Generic;
using System.Network;
using UnityEngine;
using Photon.Pun;
public class PlayerMover : MonoBehaviourPunCallbacks
{
    private PlayerStatus playerStatus;
    private PlayerItemHandler playerItemHandler;
    private PlayerObjectManipulator playerObjectManipulator;
    //private ItemCBehavior itemCBehavior;
     [SerializeField]
    private Rigidbody _rb;
     [SerializeField]
    private float _walkSpeed
 = 5.0f;
    public float WalkSpeed => _walkSpeed;
    private float _currentMoveSpeed;
    public float CurrentMoveSpeed => _currentMoveSpeed;

    [SerializeField]
    private float _frontRayRength = 0.51f;
    [SerializeField]
    private float _initialjumpPower = 30.0f;
    private float _JumpPower;
    private bool _onGround = true;
    public bool OnGround => _onGround;

    private PlayerCheakAround playerCheakAround;
    private PlayerStatus _playerStatus;
    private bool _isMoving = true;

    // Start is called before the first frame update
    private void Start()
    {
         _JumpPower = _initialjumpPower;
         _currentMoveSpeed = _walkSpeed;
         playerItemHandler = GetComponent<PlayerItemHandler>();
         playerCheakAround = GetComponent<PlayerCheakAround>();
        //itemCBehavior = GetComponent<ItemCBehavior>();
         playerObjectManipulator = GetComponent<PlayerObjectManipulator>();
         _playerStatus = GetComponent<PlayerStatus>();
         if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;

    }

    // Update is called once per frame
    private void Update()
    {
        PlayerCtrl();
        Jump();
        playerItemHandler.UseItemA();
    }

    private void PlayerCtrl()
    {
        _currentMoveSpeed = _walkSpeed;
        float inputX = 0.0f;
        //チーム1とチーム2で操作反転
        // if (_playerDataReceiver.MineTeamID == 0)
        // {
            if (Input.GetKey("d")) inputX = 1.0f;
            if (Input.GetKey("a")) inputX = -1.0f;
        //}
        // else
        // {
        //     if (Input.GetKey("d")) inputX = -1.0f;
        //     if (Input.GetKey("a")) inputX = 1.0f;
        // }
        if (Input.GetKey("a") && Input.GetKey("d")) inputX = 0.0f;
        if (!_isMoving) inputX = 0;

        if (inputX == 0)
        {
            _currentMoveSpeed = 0;
            return;
        }
        
        Vector3 movement = new Vector3(inputX, 0, 0);
        // プレイヤーの向きを移動ベクトルに向ける
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.left);

       transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1280.0f * Time.deltaTime);
       // if (CheckFront(new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _frontRayRength))) return;
        int ItemAEffectRate = playerItemHandler.ItemAEffectRate;
        _rb.MovePosition(transform.position + movement * _currentMoveSpeed
                                           * Time.deltaTime * ItemAEffectRate );
    }

    private void Jump()
    {
        _onGround = playerCheakAround.CheakGroundRay();
        if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            //ジャンプSE鳴らす
            //_playerSoundHandler.PlayJumpSE();
            _rb.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
        }
    }
    public void SetMoveBool(bool isMoving)
    {
        _isMoving = isMoving;
    }
}
