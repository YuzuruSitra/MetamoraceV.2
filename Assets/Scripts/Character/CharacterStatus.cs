using System;
using System.Battle;
using System.Collections.Generic;
using Photon.Pun;
using System.Network;
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
        
        public enum SpecialEffects
        {
            None,
            SelfEnhancement
        }
        private SpecialEffects _currentSpecialEffects = SpecialEffects.None;
        public event Action<SpecialEffects> ChangeSpecialEffectsEvent;
        
        [SerializeField] private CharacterMover _characterMover;
        [SerializeField] private CharacterObjBreaker _characterObjBreaker;
        [SerializeField] private CharacterObjGenerator _characterObjGenerator;
        private TimeHandler _timeHandler;
        [SerializeField] private bool _isWaitScene;
        // Team 1 or 2.
        private int _localPlayerTeam;
        public int LocalPlayerTeam => _localPlayerTeam;
        private readonly HashSet<Condition> _nonMovingConditions = new()
        {
            Condition.Pause,
            Condition.Generate,
            Condition.Stan,
            Condition.VDeath,
            Condition.HDeath
        };
        
        private void Awake()
        {
            if (!photonView.IsMine) return;
            if (_isWaitScene) return;
            ChangeCondition(Condition.Pause);
            _timeHandler = GameObject.FindWithTag("TimeHandler").GetComponent<TimeHandler>();
            _timeHandler.CountDownedEvent += LaunchGame;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId))
            _localPlayerTeam = (int)teamId;
            var isReversal = (_localPlayerTeam == 1);
            _characterMover.SetReversalBool(isReversal);
        }

        private void OnDestroy()
        {
            if (_isWaitScene) return;
            _timeHandler.CountDownedEvent -= LaunchGame;
        }


        private void LaunchGame()
        {
            ChangeCondition(Condition.Idole);
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            JudgmentCondition();
        }
        
        // Within factors.
        private void JudgmentCondition()
        {
            if (_currentCondition is Condition.Stan or Condition.Pause or Condition.VDeath or Condition.HDeath) return;
            if (_characterObjGenerator.IsGenerate)
            {
                ChangeCondition(Condition.Generate);
            }
            else if (_characterObjBreaker.IsBreaking)
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
        
        public void ReceiveSpecialEffects(SpecialEffects effects)
        {
            if (_currentSpecialEffects == effects) return;
            _currentSpecialEffects = effects;
            ChangeSpecialEffectsEvent?.Invoke(effects);
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
            _characterObjGenerator.SetGenerateBool(isMove);
            _characterObjBreaker.SetBreakBool(isMove);
        }
        
    }
}
