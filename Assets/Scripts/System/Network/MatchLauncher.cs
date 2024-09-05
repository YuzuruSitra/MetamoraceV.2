using ExitGames.Client.Photon;
using Object;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Network
{
    public class MatchLauncher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string _titleSceneName;
        [SerializeField] private string _gameSceneName;
        private bool _isMatching;
        public event Action<bool> ChangeIsMatching;
        private CustomInfoHandler _customInfoHandler;
        
        private void Start()
        {
            _customInfoHandler = CustomInfoHandler.Instance;
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

        public void GameStart()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC(nameof(StopAutoSync), RpcTarget.All);
            // 開始に条件を追記して�?く予�?
            if (!_isMatching) return;
            SetInGameID();
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(_gameSceneName);
        }
        
        // 退出処�?
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(_titleSceneName);
        }
        
        [PunRPC]
        private void StopAutoSync()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            IsEnable();
        }

        private void IsEnable()
        {
            var isMatching = true;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamValue))
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

        private void SetInGameID()
        {
            var team1Count = 0;
            var team2Count = 2;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var customProperties = player.CustomProperties;
                if (!customProperties.ContainsKey(CustomInfoHandler.TeamIdKey)) continue;
                var teamValue = (int)customProperties[CustomInfoHandler.TeamIdKey];
                switch (teamValue)
                {
                    case 1:
                        _customInfoHandler.ChangeValue(CustomInfoHandler.MemberIdKey, team1Count, player);
                        team1Count++;
                        break;
                    case 2:
                        _customInfoHandler.ChangeValue(CustomInfoHandler.MemberIdKey, team2Count, player);
                        team2Count++;
                        break;
                }
            }
        }
        
    }
}
