using System.Collections.Generic;
using System.Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Object
{
    public class TeamSetter : MonoBehaviour
    {
        [SerializeField] private int _setTeamNum;
        public const int TeamOutValue = -1;
        private readonly Dictionary<Collider, Player> _playerCache = new();

        private CustomInfoHandler _customInfoHandler;
        
        private void Start()
        {
            _customInfoHandler = CustomInfoHandler.Instance;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (!other.gameObject.CompareTag("Player")) return;

            if (!_playerCache.TryGetValue(other, out var player))
            {
                var photonView = other.GetComponent<PhotonView>();
                if (photonView == null || photonView.Owner == null)
                {
                    Debug.Log("PhotonView or Owner is null");
                    return;
                }
                player = photonView.Owner;
                _playerCache[other] = player;
            }
            
            // チームのプレイヤー数をカウント
            var teamCount = CountPlayersInTeam(_setTeamNum);
            // チームに2人以上いる場合は TeamOutValue を設定
            var setValue = teamCount >= 2 ? TeamOutValue : _setTeamNum;
            _customInfoHandler.ChangeValue(CustomInfoHandler.BattleIdKey, setValue, player);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (!other.gameObject.CompareTag("Player")) return;

            if (!_playerCache.TryGetValue(other, out var player))
            {
                var photonView = other.GetComponent<PhotonView>();
                if (photonView == null || photonView.Owner == null)
                {
                    Debug.Log("PhotonView or Owner is null");
                    return;
                }
                player = photonView.Owner;
                _playerCache[other] = player;
            }
            _customInfoHandler.ChangeValue(CustomInfoHandler.BattleIdKey, TeamOutValue, player);
            Debug.Log($"Player {player.NickName} is now out of the team.");
        }

        // 指定したチームにいるプレイヤーの数をカウントするメソッド
        private int CountPlayersInTeam(int teamNum)
        {
            var count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.BattleIdKey, out var teamValue) && (int)teamValue == teamNum) 
                    count++;
            return count;
        }
    }
}
