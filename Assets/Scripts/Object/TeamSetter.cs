using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using System.Network;
using UnityEngine;

namespace Object
{
    public class TeamSetter : MonoBehaviour
    {
        [SerializeField] private int _setTeamNum;
        private readonly Dictionary<Collider, Player> _playerCache = new();
        private readonly HashSet<Player> _assignedPlayers = new(); // プレイヤーが既にチームに割り当てられているかの追跡

        private CustomInfoHandler _customInfoHandler;

        private void Start()
        {
            _customInfoHandler = CustomInfoHandler.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient || !other.gameObject.CompareTag("Player")) return;

            if (!TryGetPlayer(other, out var player)) return;

            if (_assignedPlayers.Contains(player)) return; // すでに処理済みなら何もしない

            var teamCount = CountPlayersInTeam(_setTeamNum);
            var setValue = teamCount >= 2 ? CustomInfoHandler.InitialValue : _setTeamNum;
            _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, setValue, player);
            _assignedPlayers.Add(player); // プレイヤーをセット済みに追加
        }

        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient || !other.gameObject.CompareTag("Player")) return;

            if (!TryGetPlayer(other, out var player)) return;

            if (!_assignedPlayers.Contains(player)) return; // すでにチームから外れているなら何もしない

            _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, CustomInfoHandler.InitialValue, player);
            _assignedPlayers.Remove(player); // プレイヤーをチームから外したのでリストから削除
        }

        private bool TryGetPlayer(Collider other, out Player player)
        {
            if (!_playerCache.TryGetValue(other, out player))
            {
                var photonView = other.GetComponent<PhotonView>();
                if (photonView == null || photonView.Owner == null) return false;
                player = photonView.Owner;
                _playerCache[other] = player;
            }
            return true;
        }

        private int CountPlayersInTeam(int teamNum)
        {
            var count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamValue) && (int)teamValue == teamNum)
                    count++;
            }
            return count;
        }
    }
}
