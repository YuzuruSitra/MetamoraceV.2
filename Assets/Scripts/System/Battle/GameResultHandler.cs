using Photon.Pun;
using UnityEngine;

namespace System.Battle
{
    public class GameResultHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TimeHandler _timeHandler;
        [SerializeField] private BlockGenerator _blockGenerator;
        public event Action<int> CalcGameResult;
        
        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _timeHandler.FinishEvent += CalcResult;
        }

        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            _timeHandler.FinishEvent -= CalcResult;
        }

        private void CalcResult()
        {
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
