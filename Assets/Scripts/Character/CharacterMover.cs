using System.Network;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterMover : MonoBehaviourPunCallbacks
    {
        [Header("歩行速度")]
        [SerializeField] private float _walkSpeed;

        public float WalkSpeed => _walkSpeed;
        
        [Header("走行速度")]
        [SerializeField] private float _runSpeed;

        private float _speedFactor = 1.0f;
        public float RunSpeed => _runSpeed;

        [Header("ジャンプ速度")]
        [SerializeField] private float _jumpSpeed;

        [Header("重力")]
        [SerializeField] private float _gravity;
        [SerializeField] private bool _isWaitScene;
        private bool _isReversal;
        
        private CharacterController _controller;
        public float CurrentMoveSpeed { get; private set; }
        public bool IsGrounded => _controller.isGrounded;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _direction = Vector3.zero;
        private float _verticalSpeed;
        private bool _isMoving = true;
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            _controller = GetComponent<CharacterController>();
            if (_isWaitScene) return;
            if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
            _isReversal = ((int)teamId != 0);
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            
            // Caching the horizontal input and speed calculation
            var horizontal = Input.GetAxis("Horizontal");
            if (_isReversal) horizontal = -horizontal;
            var speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;

            if (!_isMoving) horizontal = 0;
            
            _moveDirection.x = horizontal * speed * _speedFactor;
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
                if (_isMoving && Input.GetButtonDown("Jump")) _verticalSpeed = _jumpSpeed;
            }
            else
            {
                // Apply gravity when the character is in the air
                _verticalSpeed -= _gravity * Time.deltaTime;
            }
            // Combine horizontal movement with vertical speed
            _moveDirection.y = _verticalSpeed;
            _controller.Move(_moveDirection * Time.deltaTime);
        }

        public void SetMoveBool(bool isMoving)
        {
            _isMoving = isMoving;
        }

        public void ChangeSpeedFactor(float value)
        {
            _speedFactor = value;
        }
        
    }
}
