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
            
            // 繝�?��?��繝�縺?��繝励Ξ繧?��繝､繝ｼ謨?��繧偵き繧?��繝ｳ繝�
            var teamCount = CountPlayersInTeam(_setTeamNum);
            // 繝�?��?��繝�縺?��2�??��莉･荳翫?��繧句?��?��蜷医?��?�� TeamOutValue 繧定ｨ?��螳?��
            var setValue = teamCount >= 2 ? TeamOutValue : _setTeamNum;
            _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, setValue, player);
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
            _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, TeamOutValue, player);
            Debug.Log($"Player {player.NickName} is now out of the team.");
        }

        // 謖�螳壹?�?縺溘メ繝ｼ繝�縺?��縺?��繧九�励Ξ繧?��繝､繝ｼ縺?��謨?��繧偵き繧?��繝ｳ繝医�?繧九Γ繧?��繝�繝�
        private int CountPlayersInTeam(int teamNum)
        {
            var count = 0;
            foreach (var player in PhotonNetwork.PlayerList)
                if (player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamValue) && (int)teamValue == teamNum) 
                    count++;
            return count;
        }
    }
}
