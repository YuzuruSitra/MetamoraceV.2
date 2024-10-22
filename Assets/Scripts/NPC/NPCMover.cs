using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NPC
{
    public class NPCMover : MonoBehaviour
    { 
    private NPCItemHandler _npcItemHandler;
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
    // private PlayerStatus _playerStatus;
    private bool _isMoving = true;
    private float _verticalSpeed;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _direction = Vector3.zero;

    [SerializeField] private NPCCheackAround _npcCheackAround;
    // Start is called before the first frame update
    private void Start()
    {
       //if (!photonView.IsMine) return;
         _npcItemHandler = GetComponent<NPCItemHandler>();
         _controller = GetComponent<CharacterController>();
         //if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
         //_isReversal = ((int)teamId != 0);
    }

    // Update is called once per frame
    private void Update()
    {
       // if (!photonView.IsMine) return;
        // int ItemAEffectRate = _npcItemHandler.ItemAEffectRate;
        // // Caching the horizontal input and speed calculation
        // var horizontal = Input.GetAxis("Horizontal");
        // var speed = _walkSpeed * ItemAEffectRate;
        // if (!_isMoving) horizontal = 0;
        //     
        // _moveDirection.x = horizontal * speed;
        // _moveDirection.z = 0;
        // CurrentMoveSpeed = speed * Mathf.Abs(horizontal);
        //     
        // if (horizontal != 0)
        // {
        //     _direction.x = horizontal;
        //     _direction.z = 0;
        //     transform.rotation = Quaternion.LookRotation(_direction);
        // }
        //
        // if (_controller.isGrounded)
        // {
        //     _verticalSpeed = -_gravity * Time.deltaTime;
        //     if (Input.GetButtonDown("Jump")) _verticalSpeed = _jumpSpeed;
        // }
        // else
        // {
        //     // Apply gravity when the character is in the air
        //     _verticalSpeed -= _gravity * Time.deltaTime;
        // }
        // // Combine horizontal movement with vertical speed
        // _moveDirection.y = _verticalSpeed;
        // _controller.Move(_moveDirection * Time.deltaTime);
        // _npcItemHandler.UseItemA();
    }

    public void Jump()
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
        _moveDirection.y = _verticalSpeed;
    }

    public void AvoidVerticalBlock()
    {
        int ItemAEffectRate = _npcItemHandler.ItemAEffectRate;
        var horizontal = 1;
        var speed = _walkSpeed * ItemAEffectRate;;
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
        _controller.Move(_moveDirection * Time.deltaTime);
    }

    public void ForwardBlock()
    {
        int ItemAEffectRate = _npcItemHandler.ItemAEffectRate;
        string _blockDirection = _npcCheackAround.CheckArroundBlock();
        if (_blockDirection == "Right")
        {
            var horizontal = 1;
            var speed = _walkSpeed * ItemAEffectRate;
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
            _controller.Move(_moveDirection * Time.deltaTime);
        }
        else
        {
            var horizontal = -1;
            var speed = _walkSpeed* ItemAEffectRate;
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
            _controller.Move(_moveDirection * Time.deltaTime);
        }
    }
    
    
    
    public void SetMoveBool(bool isMoving)
    {
        _isMoving = isMoving;
    }
    }

}
