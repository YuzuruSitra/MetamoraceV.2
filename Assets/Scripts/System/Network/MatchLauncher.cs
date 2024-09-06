using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace System.Network
{
    public class MatchLauncher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string _titleSceneName;
        [SerializeField] private string _gameSceneName;
        private bool _isMatching;
        public event Action<bool> ChangeIsMatching;
        private CustomInfoHandler _customInfoHandler;
        private bool _isWaitMemberId = true;
        private Coroutine _gameStartCoroutine;
        private void Start()
        {
            _customInfoHandler = CustomInfoHandler.Instance;
            if (!PhotonNetwork.IsMasterClient) return;
            PhotonNetwork.CurrentRoom.IsOpen = true;
            foreach (var player in PhotonNetwork.PlayerList)
                _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, CustomInfoHandler.InitialValue, player);
        }

        public void GameStart()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (!_isMatching)
            {
                if (_gameStartCoroutine != null)
                {
                    StopCoroutine(_gameStartCoroutine);
                    _gameStartCoroutine = null;
                }
                return;
            }
            // カスタムプロパティを設定して、全員のプロパティ更新を確認するまで待機する
            _gameStartCoroutine = StartCoroutine(WaitForAllPlayersReady());
        }

        private IEnumerator WaitForAllPlayersReady()
        {
            SetInGameID();

            // すべてのプレイヤーが準備完了になるまで待機
            while (_isWaitMemberId) yield return null;

            // プレイヤー全員が準備完了になったらシーン遷移
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(_gameSceneName);
            _gameStartCoroutine = null;
        }
        
        // 退出処理
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(_titleSceneName);
        }
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (changedProps.ContainsKey(CustomInfoHandler.TeamIdKey))
                IsEnable();
            if (changedProps.ContainsKey(CustomInfoHandler.MemberIdKey))
                CheckMemberIdChanged();
        }

        private void CheckMemberIdChanged()
        {
            var isWait = false;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.MemberIdKey, out var memberIdValue))
                {
                    if ((int)memberIdValue == CustomInfoHandler.InitialValue)
                    {
                        isWait = true;
                        break;
                    }
                }
                else
                {
                    isWait = true;
                    break;
                }
            }
            _isWaitMemberId = isWait;
        }

        private void IsEnable()
        {
            var isMatching = true;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamValue))
                {
                    if ((int)teamValue == CustomInfoHandler.InitialValue)
                    {
                        isMatching = false;
                        break;
                    }
                }
                else
                {
                    isMatching = false;
                    break;
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
