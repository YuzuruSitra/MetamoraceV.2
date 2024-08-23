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
        private readonly float ReEnterStateInterval = 2.0f;
        private Dictionary<NPCState, float> stateReEnterTimes = new Dictionary<NPCState, float>();
        
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
            _currentNpcState = NPCState.Idle;

            // 各ステートの再入可能時間を初期化
            foreach (NPCState state in Enum.GetValues(typeof(NPCState)))
            {
                stateReEnterTimes[state] = -ReEnterStateInterval;
            }
        }

        private void Update()
        {
            JudgmentCondition();
        }

        private void JudgmentCondition()
        {
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
            else if (_npcCheackAround.CheckArroundBlock() != null)
            {
                ChangeCondition(NPCState.Walk);
                _npcMover.ForwardBlock();
            }
            else
            {
                ChangeCondition(NPCState.Idle);
            }
        }

        private void ChangeCondition(NPCState newState)
        {
            float currentTime = Time.time;
            
            // 新しいステートに入る前に再入可能か確認
            if (_currentNpcState == newState || currentTime - stateReEnterTimes[newState] < ReEnterStateInterval)
            {
                return;
            }

            // 現在のステートの再入可能時間を設定
            stateReEnterTimes[_currentNpcState] = currentTime;

            // ステートの変更処理
            ChangeStateEvent?.Invoke(newState);
            _currentNpcState = newState;
            ChangeMoveBool(newState);
        }

        private void ChangeMoveBool(NPCState newState)
        {
            // 移動状態の変更に関するロジックを実装
        }

        public void ReceiveChangeState(NPCState _state)
        {
            ChangeCondition(_state);
        }
    }
}
