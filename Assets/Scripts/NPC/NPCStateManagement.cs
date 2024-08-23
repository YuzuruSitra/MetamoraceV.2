using Photon.Pun;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace NPC
{
    public class NPCStateManagement : MonoBehaviourPunCallbacks
    {
        [SerializeField] private NPCCheackAround _npcCheackAround;
        [SerializeField] private NPCMover _npcMover;
        [SerializeField] private NPCObjectmanipulater _npcObjectmanipulater;
        
        public enum NPCState
        {
            Idle,
            Avoid,
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

        public event Action<NPCState> ChangeStateEvent;

        private NPCState _currentNpcState;

        private void Start()
        {
            //if (!photonView.IsMine) return;
        }

        private void Update()
        {
            //if (!photonView.IsMine) return;
            JudgmentCondition();//
           
        }
        private void JudgmentCondition()
        {
           // if (_currentCondition is Condition.Stan or Condition.Pause) return;
            if (_npcCheackAround.VerticalDeath)
            {
                ChangeCondition(NPCState.VDeath);
            }
            else if (_npcCheackAround.HorizontalDeath)
            {
                ChangeCondition(NPCState.HDeath);
            }
            else if (_npcCheackAround.CheckVerticalDeathBlock())
            {
                ChangeCondition(NPCState.Avoid);
                _npcMover.AvoidVerticalBlock();
            }
            else if (_npcCheackAround.CheckBlockRay1() != null && !_npcObjectmanipulater.HasBlock)
            {
                ChangeCondition(NPCState.Break);
                _npcObjectmanipulater.BreakBlock();
            }
            else if (_npcObjectmanipulater.HasBlock)
            {
                ChangeCondition(NPCState.Generate);
                _npcObjectmanipulater.CreateBlock();
            }
        
        
           
            
        
            // else if (_playerMover.CurrentMoveSpeed <= _playerMover.RunSpeed)
            // {
            //     ChangeCondition(Condition.Run);
            // }
        }
        //ブロックを回避
        //ブロックを壊す
        private void ChangeCondition(NPCState newState)
        {
            if (_currentNpcState == newState) return;
            ChangeStateEvent?.Invoke(newState);
            _currentNpcState = newState;
            ChangeMoveBool(newState);
        }

        private void ChangeMoveBool(NPCState newState)
        {
            // var isMove = !_nonMovingConditions.Contains(newState);
            // _characterMover.SetMoveBool(isMove);
        }
    }
}

