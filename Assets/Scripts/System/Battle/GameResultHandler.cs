using System.Collections;
using System.Network;
using Character;
using Photon.Pun;
using UnityEngine;

namespace System.Battle
{
    public class GameResultHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TimeHandler _timeHandler;
        [SerializeField] private BlockGenerator _blockGenerator;

        [SerializeField] private float _delayTime;
        private WaitForSeconds _waitFor;
        public event Action<int> CalcGameResult;
        private CharacterStatus _characterStatus;
        
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _timeHandler.FinishEvent += CalcResult;
            _waitFor = new WaitForSeconds(_delayTime);
            
            var playerGenerator = GameObject.FindWithTag("PlayerGenerator").GetComponent<PlayerGenerator>();
            var player = playerGenerator.CurrentPlayer;
            _characterStatus = player.GetComponent<CharacterStatus>();
            _characterStatus.ChangeConditionEvent += ReceiveCondition;
        }

        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _timeHandler.FinishEvent -= CalcResult;
            _characterStatus.ChangeConditionEvent -= ReceiveCondition;
        }

        private void ReceiveCondition(CharacterStatus.Condition condition)
        {
            if (condition is CharacterStatus.Condition.HDeath or CharacterStatus.Condition.VDeath)
            {
                StartCoroutine(ResultDelay());
            }
        }

        private IEnumerator ResultDelay()
        {
            yield return _waitFor;
            CalcResult();
        }

        private void CalcResult()
        {
            _timeHandler.ReceiveStopTime();
            var shareTeam1 = _blockGenerator.BlocksShareTeam1;
            var shareTeam2 = _blockGenerator.BlocksShareTeam2;
            var winTeamNum = (shareTeam1 == shareTeam2) ? 0 : (shareTeam1 < shareTeam2 ? 1 : 2);
            photonView.RPC(nameof(ShareCalc), RpcTarget.All, winTeamNum);
        }

        [PunRPC]
        private void ShareCalc(int winTeamNum)
        {
            CalcGameResult?.Invoke(winTeamNum);
        }
    }
}
