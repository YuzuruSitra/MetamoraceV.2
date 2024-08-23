using System;
using System.Collections.Generic;
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
            Generate,
            Break,
            VDeath,
            HDeath,
            Stan,
            Pause
        }

        private Condition _currentCondition;
        public event Action<Condition> ChangeConditionEvent;
        
        [SerializeField] private CharacterMover _characterMover;
        [SerializeField] private CharacterObjBreaker _characterObjBreaker;
        [SerializeField] private CharacterObjGenerator _characterObjGenerator;
        private readonly HashSet<Condition> _nonMovingConditions = new()
        {
            Condition.Pause,
            Condition.Break,
            Condition.Generate,
            Condition.Stan,
            Condition.VDeath,
            Condition.HDeath
        };
        
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
        
        // Within factors.
        private void JudgmentCondition()
        {
            if (_currentCondition is Condition.Stan or Condition.Pause) return;

            if (_characterObjGenerator.IsGenerate)
            {
                ChangeCondition(Condition.Generate);
            }
            if (_characterObjBreaker.IsBreaking)
            {
                ChangeCondition(Condition.Break);
            }
            else if (!_characterMover.IsGrounded)
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
        
        // External factors.
        public void ReceiveChangeState(Condition condition)
        {
            ChangeCondition(condition);
        }

        private void ChangeCondition(Condition newCondition)
        {
            if (_currentCondition == newCondition) return;
            ChangeConditionEvent?.Invoke(newCondition);
            _currentCondition = newCondition;
            ChangeMoveBool(newCondition);
        }

        private void ChangeMoveBool(Condition newCondition)
        {
            var isMove = !_nonMovingConditions.Contains(newCondition);
            _characterMover.SetMoveBool(isMove);
        }
        
    }
}
