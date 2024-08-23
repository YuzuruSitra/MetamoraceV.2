using Block;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterObjBreaker : MonoBehaviourPunCallbacks
    {
        private GameObject _currentObj;
        private BlockBase _currentBlockBase;
        private readonly Vector3 _upPadding = new(0f,0.5f,0f);
        [SerializeField] private float _destroyPower;
        private float _powerFactor = 1.0f;
        [SerializeField] private float _playerReach;
        public bool IsBreaking { get; private set; }
        [SerializeField] private CharacterObjStacker _characterObjStacker;

        private void Update()
        {
            if (!photonView.IsMine) return; 
            if (Input.GetMouseButton(0)) BreakBlock();
        }
        
        private void BreakBlock()
        {
            if (_characterObjStacker.HasBlock == CharacterObjStacker.NullKey)
            {
                IsBreaking = true;
                if (!CheckHitBlock()) return;
                var power = _destroyPower * _powerFactor;
                var breakObjName = _currentBlockBase.DestroyBlock(power, gameObject);
                if (breakObjName == BlockBase.ErrorTag) return;
                _characterObjStacker.BreakBlock(_currentObj);
                return;
            }
            IsBreaking = false;
        }
        
        private bool CheckHitBlock()
        {
            var rayOrigin = transform.position + _upPadding;
            if (!Physics.Raycast(rayOrigin, transform.forward, out var hit, _playerReach)) return false;
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Block")) return false;
            if (Equals(_currentObj, hit.collider.gameObject)) return true;
            _currentObj = hit.collider.gameObject;
            _currentBlockBase = hit.collider.gameObject.GetComponent<BlockBase>();
            return true;
        }

        public void ChangePowerFactor(float value)
        {
            _powerFactor = value;
        }
    
    }
}
