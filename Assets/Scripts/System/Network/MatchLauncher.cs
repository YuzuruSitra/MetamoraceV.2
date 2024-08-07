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

        private void Start()
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }

        public void GameStart()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            photonView.RPC(nameof(StopAutoSync), RpcTarget.All);
            // 開始に条件を追記していく予定
            if (!_isMatching) return;
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
