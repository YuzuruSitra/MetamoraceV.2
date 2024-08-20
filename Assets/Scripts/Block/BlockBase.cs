using Character;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Block
{
    public abstract class BlockBase : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private MeshRenderer _mesh;
        private BoxCollider _col;
        private const string ErrorTag = "Null";
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
        private const float RayLength = 0.02f;

        private void Start()
        {
            _mesh = GetComponent<MeshRenderer>();
            _col = GetComponent<BoxCollider>();
            _blockAnimator = GetComponent<Animator>();
            _currentHealth = _maxHealth;
            _currentActiveTime = _activeTime;
        }

        private void Update()
        {
            if (!IsGrounded()) transform.position += Vector3.down * (_fallSpeed * Time.deltaTime);

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

        public string DestroyBlock(float power, GameObject player)
        {
            photonView.RPC(nameof(ChangeHealth), RpcTarget.All, power);
            if (!(_currentHealth <= 0)) return ErrorTag;
            SendEffect(player);
            photonView.RPC(nameof(LaunchBreak), RpcTarget.All);
            return gameObject.tag;
        }

        [PunRPC]
        private void ChangeHealth(float power)
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
        private void LaunchBreak()
        {
            _mesh.enabled = false;
            _col.enabled = false;
            _cloudAnimator.SetBool(IsBreak, true);
        }

        protected virtual void SendEffect(GameObject player)
        {
            
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, _mesh.bounds.extents.y + RayLength);
        }
    }
}
