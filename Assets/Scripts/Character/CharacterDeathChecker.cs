using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterDeathChecker : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float _verticalRayOffset = 2.0f;
        [SerializeField] private float _horizontalRayOffset = 0.5f;
        [SerializeField] private float _verticalrayDistance = 0.15f;
        [SerializeField] private float _horizontalrayDistance = 0.4f;
        private Vector3 _horizontalRayDirection;
        [SerializeField] private CharacterMover _characterMover;
        [SerializeField] private CharacterStatus _characterStatus;
        [SerializeField] private CharacterPhotonStatus _characterPhotonStatus;

        private void Start()
        {
            if (!photonView.IsMine) return;
            _horizontalRayDirection = (_characterPhotonStatus.LocalPlayerTeamID == 1) ? Vector3.forward : Vector3.back;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            JudgeVerticalDeath();
            JudgeHorizontalDeath();
        }

        private void JudgeVerticalDeath()
        {
            var rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
            if (!Physics.Raycast(rayOrigin, Vector3.up, out var hit, _verticalrayDistance)) return;
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Block")) return; 
            if (!_characterMover.IsGrounded) return;
            _characterStatus.ReceiveChangeState(CharacterStatus.Condition.VDeath);
        }
        
        private void JudgeHorizontalDeath()
        {
            var rayOrigin = transform.position + Vector3.up * _horizontalRayOffset;
            Debug.DrawRay(rayOrigin, _horizontalRayDirection * _horizontalrayDistance, Color.red); // レイを常に表示
            if (!Physics.Raycast(rayOrigin, _horizontalRayDirection, out var hit, _horizontalrayDistance)) return;
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Block")) return; 
            _characterStatus.ReceiveChangeState(CharacterStatus.Condition.HDeath);
        }
    }
}
