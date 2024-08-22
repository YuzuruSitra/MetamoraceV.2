using System.Network;
using UnityEngine;
using Photon.Pun;
public class PlayerMover : MonoBehaviourPunCallbacks
{
    private PlayerStatus playerStatus;
    private PlayerItemHandler playerItemHandler;
     [SerializeField]
    private Rigidbody _rb;
    [Header("歩行速度")]
    [SerializeField] private float _walkSpeed;
    public float WalkSpeed => _walkSpeed;
    [Header("走行速度")]
    [SerializeField] private float _runSpeed;
    
    [Header("ジャンプ速度")]
    [SerializeField] private float _jumpSpeed;
    private float _currentMoveSpeed;
    
    [Header("重力")]
    [SerializeField] private float _gravity;
    public float CurrentMoveSpeed { get; private set; }
    
    private CharacterController _controller;
    public bool IsGrounded => _controller.isGrounded;
    
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
    private float _verticalSpeed;
    private bool _isReversal;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _direction = Vector3.zero;
    // Start is called before the first frame update
    private void Start()
    {
        if (!photonView.IsMine) return;
         _JumpPower = _initialjumpPower;
         _currentMoveSpeed = _walkSpeed;
         playerItemHandler = GetComponent<PlayerItemHandler>();
         playerCheakAround = GetComponent<PlayerCheakAround>();
         _controller = GetComponent<CharacterController>();
         if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
         _isReversal = ((int)teamId != 0);
    }

    // Update is called once per frame
    private void Update()
    {
        //PlayerCtrl();
        //Jump();
        if (!photonView.IsMine) return;
        int ItemAEffectRate = playerItemHandler.ItemAEffectRate;
        // Caching the horizontal input and speed calculation
        var horizontal = Input.GetAxis("Horizontal");
        if (_isReversal) horizontal = -horizontal;
        var speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed * ItemAEffectRate;
        if (!_isMoving) horizontal = 0;
            
        _moveDirection.x = horizontal * speed;
        _moveDirection.z = 0;
        CurrentMoveSpeed = speed * Mathf.Abs(horizontal);
            
        if (horizontal != 0)
        {
            _direction.x = horizontal;
            _direction.z = 0;
            transform.rotation = Quaternion.LookRotation(_direction);
        }

        if (_controller.isGrounded)
        {
            _verticalSpeed = -_gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump")) _verticalSpeed = _jumpSpeed;
        }
        else
        {
            // Apply gravity when the character is in the air
            _verticalSpeed -= _gravity * Time.deltaTime;
        }
        // Combine horizontal movement with vertical speed
        _moveDirection.y = _verticalSpeed;
        _controller.Move(_moveDirection * Time.deltaTime);
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
        if (_controller.isGrounded)
        {
            _verticalSpeed = -_gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump")) _verticalSpeed = _jumpSpeed;
        }
        else
        {
            // Apply gravity when the character is in the air
            _verticalSpeed -= _gravity * Time.deltaTime;
        }
        // _onGround = playerCheakAround.CheakGroundRay();
        // if (Input.GetKeyDown(KeyCode.Space) && _onGround)
        // {
        //     //ジャンプSE鳴らす
        //     //_playerSoundHandler.PlayJumpSE();
        //     _rb.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
        // }
    }
    public void SetMoveBool(bool isMoving)
    {
        _isMoving = isMoving;
    }
}
