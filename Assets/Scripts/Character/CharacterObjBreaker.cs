using Block;
using Photon.Pun;
using UnityEngine;
using System.Battle;

namespace Character
{
    public class CharacterObjBreaker : MonoBehaviourPunCallbacks
    {
        private bool _isBreak;
        private GameObject _currentObj;
        private BlockBase _currentBlockBase;
        private readonly Vector3 _upPadding = new(0f,0.4f,0f);
        [SerializeField] private float _destroyPower;
        private float _powerFactor = 1.0f;
        [SerializeField] private float _playerReach;
        public bool IsBreaking { get; private set; }
        [SerializeField] private CharacterObjStacker _characterObjStacker;
        private BlockGenerator _blockGenerator;
        [SerializeField] private CharacterPhotonStatus _characterPhotonStatus;
        private int _teamID;

        private void Start()
        {
            if (!photonView.IsMine) return;
            _blockGenerator = GameObject.FindWithTag("BlockGenerator").GetComponent<BlockGenerator>();
            _teamID = _characterPhotonStatus.LocalPlayerTeamID - 1;
        }

        private void Update()
        {
            if (!photonView.IsMine) return; 
            if (Input.GetMouseButton(0)) BreakBlock();
            else IsBreaking = false;
        }
        
        private void BreakBlock()
        {
            if (!_isBreak || _characterObjStacker.HasBlock != CharacterObjStacker.NullKey) return;
            IsBreaking = true;
            if (!CheckHitBlock()) return;
            var power = _destroyPower * _powerFactor;
            var breakObjName = _currentBlockBase.DestroyBlock(power, gameObject);
            if (breakObjName == BlockBase.ErrorTag) return;
            _blockGenerator.ChangeBlocksShare(breakObjName, _teamID, false);
            _characterObjStacker.BreakBlock(_currentObj);
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
        
        public void SetBreakBool(bool isBreak)
        {
            _isBreak = isBreak;
        }
    
    }
}
