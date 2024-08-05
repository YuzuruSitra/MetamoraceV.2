using UnityEngine;

namespace Character
{
    public class CharacterMover : MonoBehaviour
    {
        [Header("歩行速度")]
        [SerializeField] private float _walkSpeed;

        public float WalkSpeed => _walkSpeed;
        
        [Header("走行速度")]
        [SerializeField] private float _runSpeed;

        public float RunSpeed => _runSpeed;

        [Header("ジャンプ速度")]
        [SerializeField] private float _jumpSpeed;

        [Header("重力")]
        [SerializeField] private float _gravity;

        private CharacterController _controller;
        public float CurrentMoveSpeed { get; private set; }
        public bool IsGrounded => _controller.isGrounded;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _direction = Vector3.zero;
        private float _verticalSpeed;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
                
            _moveDirection = new Vector3(horizontal, 0, 0) * speed;
            CurrentMoveSpeed = speed * Mathf.Abs(horizontal);
            
            if (horizontal != 0)
            {
                _direction = new Vector3(horizontal, 0, 0);
                transform.rotation = Quaternion.LookRotation(_direction);
            }

            if (_controller.isGrounded)
            {
                // If the character is on the ground, reset the vertical speed
                _verticalSpeed = -_gravity * Time.deltaTime;

                // Check for jump input
                if (Input.GetButtonDown("Jump"))
                {
                    _verticalSpeed = _jumpSpeed;
                }
            }
            else
            {
                // Apply gravity when the character is in the air
                _verticalSpeed -= _gravity * Time.deltaTime;
            }

            // Combine horizontal movement with vertical speed
            _moveDirection.y = _verticalSpeed;

            // Apply movement
            _controller.Move(_moveDirection * Time.deltaTime);
        }
    }
}
