using Photon.Pun;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace NPC
{
    public class NPCStateManagement : MonoBehaviourPunCallbacks
    {
        //優先順位とそれに伴うアクション
        //
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
            // if ()
            // {
            //     
            // }
        }

       
       
    }
}
