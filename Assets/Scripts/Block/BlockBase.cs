using System.Battle;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Block
{
    public abstract class BlockBase : MonoBehaviourPunCallbacks
    {
        private MeshRenderer _mesh;
        private BoxCollider _col;
        public const string ErrorTag = "Null";
        [SerializeField] private float _maxHealth;
        private float _currentHealth;
        [SerializeField] private Image _healthGage;
        [SerializeField] private float _activeTime;
        private float _currentActiveTime;
        private Animator _blockAnimator;
        private static readonly int IsTouch = Animator.StringToHash("IsTouch");
        [SerializeField] private Animator _cloudAnimator;
        private static readonly int IsBreak = Animator.StringToHash("IsBreak");
        private const float DestroyTime = 1.0f;
        private float _currentDestroyTime;
        
        [SerializeField] private float _fallSpeed;
        private const float RayLength = 0.04f;
        private const float ExitRayLength = 0.1f;
        [SerializeField] private bool _isMoving;
        [SerializeField] private int _insPlayerTeam;
        private float _targetPosZ;
        [SerializeField] private float _moveSpeed;
        private Vector3 _currentPos;
        private const float Threshold = 0.01f;

        private bool _isExit;
        private Vector3[] _rayOrigins = new Vector3[4];
        private Vector3 _extents;
        private const float RayPadding = 0.2f;
        private void Start()
        {
            Transform child = transform.GetChild(0);
            _mesh = child.GetComponent<MeshRenderer>();
            _col = GetComponent<BoxCollider>();
            _blockAnimator = child.GetComponent<Animator>();
            _currentHealth = _maxHealth;
            _currentActiveTime = _activeTime;
            _extents = _mesh.bounds.extents;
            if (!_isMoving) return;
            _targetPosZ = (_insPlayerTeam == 1) ? BlockGenerator.Team2PosZ : BlockGenerator.Team1PosZ;
            _currentPos = transform.position;
        }
        
        
        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (!_isMoving)
                {
                    GravityFall();
                    DoExit();
                }
                else
                {
                    TowardsPos();
                }
            }
            if (_currentActiveTime <= _activeTime)
            {
                _currentActiveTime += Time.deltaTime;
                _healthGage.enabled = true;
                ChangeHealthGage();
                _blockAnimator.SetBool(IsTouch, true);
            }
            else
            {
                _healthGage.enabled = false;
                _blockAnimator.SetBool(IsTouch, false);
            }

            if (_mesh.enabled) return;
            _currentDestroyTime += Time.deltaTime;
            if (DestroyTime <= _currentDestroyTime) PhotonNetwork.Destroy(gameObject);
        }

        public string DestroyBlock(float power, GameObject caller)
        {
            photonView.RPC(nameof(ChangeHealth), RpcTarget.All, power);
            if (!(_currentHealth <= 0)) return ErrorTag;
            if (caller.CompareTag("Player")) SendEffect(caller);
            photonView.RPC(nameof(LaunchBreak), RpcTarget.All);
            return gameObject.tag;
        }

        [PunRPC]
        public void ChangeHealth(float power)
        {
            _currentActiveTime = 0;
            _currentHealth -= power * Time.deltaTime;
        }

        private void ChangeHealthGage()
        {
            var ratio = _currentHealth / _maxHealth;
            _healthGage.fillAmount = ratio;
        }

        [PunRPC]
        public void LaunchBreak()
        {
            _cloudAnimator.SetBool(IsBreak, true);
            _mesh.enabled = false;
            _col.enabled = false;
        }

        protected virtual void SendEffect(GameObject player) { }

        private void GravityFall()
        {
            if (!IsGrounded()) transform.position += Vector3.down * (_fallSpeed * Time.deltaTime);
        }
        
        private void TowardsPos()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var step = _moveSpeed * Time.deltaTime;
            _currentPos.z = Mathf.MoveTowards(_currentPos.z, _targetPosZ, step);
            transform.position = _currentPos;

            if (!(Mathf.Abs(_currentPos.z - _targetPosZ) < Threshold)) return;
            _currentPos.z = _targetPosZ;
            transform.position = _currentPos;
            _isMoving = false;
        }

        private void DoExit()
        {
            if (!_isExit)
            {
                if (!IsGrounded()) return;

                bool exitDetected = false;
                var rayLength = _extents.x + ExitRayLength;
                var rayColor = Color.red; // Ray�̐F

                // ���b�V����4�̊p�̍��W���v�Z
                _rayOrigins[0] = transform.position + transform.right * (_extents.x - RayPadding) + transform.up * (_extents.y - RayPadding);
                _rayOrigins[1] = transform.position - transform.right * (_extents.x - RayPadding) + transform.up * (_extents.y - RayPadding);
                _rayOrigins[2] = transform.position + transform.right * (_extents.x - RayPadding) - transform.up * (_extents.y - RayPadding);
                _rayOrigins[3] = transform.position - transform.right * (_extents.x - RayPadding) - transform.up * (_extents.y - RayPadding);

                // �e�p����Ray���΂��ĉ���
                foreach (var origin in _rayOrigins)
                {
                    Debug.DrawRay(origin, -transform.forward * rayLength, rayColor);
                    
                    if (Physics.Raycast(origin, -transform.forward, out var hitInfo, rayLength))
                    {
                        var obj = hitInfo.collider.gameObject;
                        if (obj.layer == LayerMask.NameToLayer("Block"))
                        {
                            exitDetected = true;
                            break;
                        }
                    }
                }

                if (!exitDetected) return;

                _isExit = true;
            }
            else
            {
                transform.position += transform.forward.normalized * (_moveSpeed * Time.deltaTime);
            }
        }

        
        private bool IsGrounded()
        {
            var extents = _mesh.bounds.extents;
            var layerMask = ~LayerMask.GetMask("IgnoreLayer");
            var centerGrounded = Physics.Raycast(transform.position, Vector3.down, extents.y + RayLength, layerMask);
            var leftGrounded = Physics.Raycast(transform.position - transform.right * extents.x, Vector3.down, extents.y + RayLength, layerMask);
            var rightGrounded = Physics.Raycast(transform.position + transform.right * extents.x, Vector3.down, extents.y + RayLength, layerMask);
            return centerGrounded || leftGrounded || rightGrounded;
        }
    }
}
