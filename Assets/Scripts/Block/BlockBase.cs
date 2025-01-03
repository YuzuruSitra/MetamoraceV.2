using System.Battle;
using System.Sound;
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
         private Animator _cloudAnimator;
        [SerializeField] private GameObject _cloudEffect;

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
        private Vector3[] _rayOrigins = new Vector3[9];
        private Vector3 _extents;
        private const float RayPadding = 0.2f;
        [SerializeField] private AudioClip _breakBlockClip;
        private SoundHandler _soundHandler;
        [SerializeField]private GameObject _magicEffect;

        
        private void Start()
        {
            _cloudAnimator = _cloudEffect.GetComponent<Animator>();
            _soundHandler = SoundHandler.InstanceSoundHandler;
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
                _magicEffect.SetActive(true);
                _blockAnimator.SetBool(IsTouch, true);
            }
            else
            {
                _magicEffect.SetActive(false);
                _healthGage.enabled = false;
                _blockAnimator.SetBool(IsTouch, false);
            }

            if (!PhotonNetwork.IsMasterClient) return;
            if (_mesh.enabled) return;
            _currentDestroyTime += Time.deltaTime;
            if (DestroyTime <= _currentDestroyTime) PhotonNetwork.Destroy(gameObject);
        }

        public string DestroyBlock(float power, GameObject caller)
        {
            photonView.RPC(nameof(ChangeHealth), RpcTarget.All, power);
            if (!(_currentHealth <= 0)) return ErrorTag;
            if (caller.CompareTag("Player")) 
            {
                SendEffect(caller);
                photonView.RPC(nameof(LaunchBreak), RpcTarget.All);
            }
            else photonView.RPC(nameof(LaunchBreakForBomb), RpcTarget.All);
            
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
            _soundHandler.PlaySe(_breakBlockClip);
            _cloudAnimator.SetBool(IsBreak, true);
            _mesh.enabled = false;
            _col.enabled = false;
        }

          [PunRPC]
        public void LaunchBreakForBomb()
        {
            _soundHandler.PlaySe(_breakBlockClip);
            //アニメーターからのclouエフェクトを赤色に変更したい
            _cloudAnimator.SetBool(IsBreak, true);
            //Bombの色変化
            _cloudEffect.GetComponent<SpriteRenderer>().color = Color.red;
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
        var rayColor = Color.red;

        // 端と中央の間にある位置も含め、9本のレイを設定
        _rayOrigins[0] = transform.position + transform.right * (_extents.x - RayPadding) + transform.up * (_extents.y - RayPadding);       // 右上
        _rayOrigins[1] = transform.position - transform.right * (_extents.x - RayPadding) + transform.up * (_extents.y - RayPadding);       // 左上
        _rayOrigins[2] = transform.position + transform.right * (_extents.x - RayPadding) - transform.up * (_extents.y - RayPadding);       // 右下
        _rayOrigins[3] = transform.position - transform.right * (_extents.x - RayPadding) - transform.up * (_extents.y - RayPadding);       // 左下
        _rayOrigins[4] = transform.position;                                                                                               // 中央
        _rayOrigins[5] = transform.position + transform.right * (_extents.x - RayPadding);                                                 // 右端中央
        _rayOrigins[6] = transform.position - transform.right * (_extents.x - RayPadding);                                                 // 左端中央
        _rayOrigins[7] = transform.position + transform.up * (_extents.y - RayPadding);                                                    // 上端中央
        _rayOrigins[8] = transform.position - transform.up * (_extents.y - RayPadding);                                                    // 下端中央

        foreach (var origin in _rayOrigins)
        {
            Debug.DrawRay(origin, -transform.forward * rayLength, rayColor);

            if (Physics.Raycast(origin, -transform.forward, out var hitInfo, rayLength))
            {
                var obj = hitInfo.collider.gameObject;
                if (obj.layer == LayerMask.NameToLayer("Block"))
                {
                    Debug.Log("Hir");
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
