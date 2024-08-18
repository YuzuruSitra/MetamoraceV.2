using System;
using Photon.Pun;
using UnityEngine;

namespace Character
{
    public class CharacterStatus : MonoBehaviourPunCallbacks
    {
        public enum Condition
        {
            Idole,
            Walk,
            Run,
            Jump,
            Stan,
            Swing,
            Break,
            VDeath,
            HDeath
        }

        private Condition _currentCondition;
        public event Action<Condition> ChangeConditionEvent;
        
        [SerializeField] private CharacterMover _characterMover;
        
        private void Start()
        {
            if (!photonView.IsMine) return;
            _currentCondition = Condition.Idole;
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            JudgmentCondition();
        }

        private void JudgmentCondition()
        {
            if (!_characterMover.IsGrounded)
            {
                ChangeCondition(Condition.Jump);
            }
            else if (_characterMover.CurrentMoveSpeed == 0)
            {
                ChangeCondition(Condition.Idole);
            }
            else if (_characterMover.CurrentMoveSpeed <= _characterMover.WalkSpeed)
            {
                ChangeCondition(Condition.Walk);
            }
            else if (_characterMover.CurrentMoveSpeed <= _characterMover.RunSpeed)
            {
                ChangeCondition(Condition.Run);
            }
            
        }

        private void ChangeCondition(Condition newCondition)
        {
            if (_currentCondition == newCondition) return;
            ChangeConditionEvent?.Invoke(newCondition);
            _currentCondition = newCondition;
        }
        
    }
}
