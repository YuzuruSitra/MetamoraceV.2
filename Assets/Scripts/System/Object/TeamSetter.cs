using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace System.Object
{
    public class TeamSetter : MonoBehaviour
    {
        [SerializeField] private int _setTeamNum;
        public const int TeamOutValue = -1;
        public const string TeamKey = "Team";
        private readonly Dictionary<Collider, Player> _playerCache = new();
        
        private Hashtable _teamInProperties;
        private Hashtable _teamOutProperties;

        private void Start()
        {
            _teamInProperties = new Hashtable
            {
                { TeamKey, _setTeamNum }
            };
            
            _teamOutProperties = new Hashtable
            {
                { TeamKey, TeamOutValue }
            };
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
            player.SetCustomProperties(teamCount >= 2 ? _teamOutProperties : _teamInProperties);
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
            player.SetCustomProperties(_teamOutProperties);
            Debug.Log($"Player {player.NickName} is now out of the team.");
        }

        // 指定したチームにいるプレイヤーの数をカウントするメソッド
        private int CountPlayersInTeam(int teamNum)
        {
            var count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
                if (player.CustomProperties.TryGetValue(TeamKey, out var teamValue) && (int)teamValue == teamNum) 
                    count++;
            return count;
        }
    }
}
