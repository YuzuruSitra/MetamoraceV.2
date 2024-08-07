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
        private Hashtable _teamIdProperties;
        private const string TeamIdKey = "TeamID";
        private void Start()
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            _teamIdProperties = new Hashtable
            {
                { TeamIdKey, 0 }
            };
        }

        public void GameStart()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC(nameof(StopAutoSync), RpcTarget.All);
            // 開始に条件を追記していく予定
            if (!_isMatching) return;
            SetInGameID();
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(_gameSceneName);
        }
        
        // 退出処理
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

        private void SetInGameID()
        {
            var team1Count = 0;
            var team2Count = 2;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var customProperties = player.CustomProperties;
                if (!customProperties.ContainsKey(TeamSetter.TeamKey)) continue;
                var teamValue = (int)customProperties[TeamSetter.TeamKey];
                switch (teamValue)
                {
                    case 1:
                        _teamIdProperties[TeamIdKey] = team1Count;
                        team1Count++;
                        break;
                    case 2:
                        _teamIdProperties[TeamIdKey] = team2Count;
                        team2Count++;
                        break;
                }
                player.SetCustomProperties(_teamIdProperties);
            }
        }
        
    }
}
