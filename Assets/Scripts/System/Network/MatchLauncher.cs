using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace System.Network
{
    public class MatchLauncher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string _sceneName;
        private bool _isMatching;
        public event Action<bool> ChangeIsMatching;
        
        public void GameStart()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC(nameof(StopAutoSync), RpcTarget.All);
            // 開始に条件を追記していく予定
            if (!_isMatching) return;
            PhotonNetwork.LoadLevel(_sceneName);
        }
        
        [PunRPC]
        private void StopAutoSync()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            IsEnable();
        }

        private void IsEnable()
        {
            var isMatching = true;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(TeamSetter.TeamKey, out var teamValue))
                {
                    if ((int)teamValue == TeamSetter.TeamOutValue)
                    {
                        isMatching = false;
                    }
                }
                else
                {
                    isMatching = false;
                }
            }
            
            _isMatching = isMatching;
            ChangeIsMatching?.Invoke(_isMatching);
        }
        
    }
}
