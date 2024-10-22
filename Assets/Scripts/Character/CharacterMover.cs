using System.Network;
using System.Sound;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMover : MonoBehaviourPunCallbacks
    {
        [Header("歩行速度")]
        [SerializeField] private float _walkSpeed;
        [Header("走行速度")]
        [SerializeField] private float _runSpeed;
        [Header("減速速度")]
        [SerializeField] private float _decelerationSpeed;
        [Header("ジャンプ速度")]
        [SerializeField] private float _jumpSpeed;
        [Header("重力")]
        [SerializeField] private float _gravity;
        [SerializeField] private CharacterObjBreaker _characterObjBreaker;
        [SerializeField] private bool _isWaitScene;

        private CharacterController _controller;
        private Vector3 _moveDirection = Vector3.zero;
        private float _verticalSpeed;
        private bool _isMoving = true;
        private float _currentInputFactor;
        private float _speedFactor = 1.0f;
        private bool _isReversal;
        [SerializeField]
        private float _checkGroundReach = 0.1f;
        private float _checkBlockReach = 0.2f;

        public float CurrentMoveSpeed { get; private set; }
         bool _isGrounded;
        public bool IsGrounded => _isGrounded;

        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        private SoundHandler _soundHandler;
        [SerializeField] private AudioClip _jumpClip;
        private Collider _collider;
        Bounds bounds;
        private Vector3[] corners;
        private void Start()
        {
            if (!photonView.IsMine) return;
            _controller = GetComponent<CharacterController>();
            _soundHandler = SoundHandler.InstanceSoundHandler;
            _collider = GetComponent<Collider>();
            bounds = _collider.bounds;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            HandleMovement();
            //Debug.Log(CheckGround());
        }

        private void HandleMovement()
        {
            SpeedReduction();
            var speed = GetCurrentSpeed();
            _moveDirection.x = _currentInputFactor * speed * _speedFactor;
            CurrentMoveSpeed = speed * Mathf.Abs(_currentInputFactor);
            if (_currentInputFactor != 0) RotateCharacter();
            ApplyGravityAndJump();
            _controller.Move(_moveDirection * Time.deltaTime);
        }

        private float GetCurrentSpeed()
        {
            return Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
        }

        private void RotateCharacter()
        {
            var rot = Vector3.zero;
            rot.x = _currentInputFactor;
            var direction = rot;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void ApplyGravityAndJump()
        {
            // if (_isGrounded = CheckUnderBlock())
            // {
            //     if (_isMoving && !_characterObjBreaker.IsBreaking && Input.GetButtonDown("Jump"))
            //     {
            //         _verticalSpeed = _jumpSpeed;
            //         _soundHandler.PlaySe(_jumpClip);
            //     }
            // }
            if (_isGrounded = CheckGround())
            {
                if (_isMoving && !_characterObjBreaker.IsBreaking && Input.GetButtonDown("Jump"))
                {
                    _verticalSpeed = _jumpSpeed;
                    _soundHandler.PlaySe(_jumpClip);
                }
            }
            else  _verticalSpeed -= _gravity * Time.deltaTime;
            _moveDirection.y = _verticalSpeed;
            // if (_isGrounded)
            // {
            //     _verticalSpeed = -_gravity * Time.deltaTime;
            //     if (_isMoving && !_characterObjBreaker.IsBreaking && Input.GetButtonDown("Jump"))
            //         _verticalSpeed = _jumpSpeed;
            // }
            // else
            // {
            //     _verticalSpeed -= _gravity * Time.deltaTime;
            // }
            // _moveDirection.y = _verticalSpeed;
        }

        private void SpeedReduction()
        {
            var input = Input.GetAxis("Horizontal");
            input = _isReversal ? -input : input;
            if (input == 0 || _characterObjBreaker.IsBreaking)
                _currentInputFactor = Mathf.MoveTowards(_currentInputFactor, 0, _decelerationSpeed * Time.deltaTime);
            else
                _currentInputFactor = input;
            if (!_isMoving) _currentInputFactor = 0;
        }

        public void SetMoveBool(bool isMoving)
        {
            _isMoving = isMoving;
        }

        public void SetReversalBool(bool isReversal)
        {
            if (_isWaitScene) return;
            _isReversal = isReversal;
        }

        public void ChangeSpeedFactor(float value)
        {
            _speedFactor = value;
        }

        bool CheckGround()
        {
            bounds = _collider.bounds;
            // コライダーの四隅を計算
            corners = new Vector3[4];
            corners[0] = new Vector3(bounds.min.x, transform.position.y, bounds.min.z); // 左下
            corners[1] = new Vector3(bounds.max.x, transform.position.y, bounds.min.z); // 右下
            corners[2] = new Vector3(bounds.min.x, transform.position.y, bounds.max.z); // 左上
            corners[3] = new Vector3(bounds.max.x, transform.position.y, bounds.max.z); // 右上

            _isGrounded = false;  
            foreach (var corner in corners)
            {
                Debug.DrawRay(corner, Vector3.down * _checkGroundReach, Color.red);
                if (Physics.Raycast(corner, Vector3.down,  _checkGroundReach))
                {
                    _isGrounded = true;
                    break;  
                }
            }
            return _isGrounded;
        }
    }
}
