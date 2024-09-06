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
        private readonly HashSet<Player> _assignedPlayers = new(); // �v���C���[�����Ƀ`�[���Ɋ��蓖�Ă��Ă��邩�̒ǐ�

        private CustomInfoHandler _customInfoHandler;

        private void Start()
        {
            _customInfoHandler = CustomInfoHandler.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient || !other.gameObject.CompareTag("Player")) return;

            if (!TryGetPlayer(other, out var player)) return;

            if (_assignedPlayers.Contains(player)) return; // ���łɏ����ς݂Ȃ牽�����Ȃ�

            var teamCount = CountPlayersInTeam(_setTeamNum);
            var setValue = teamCount >= 2 ? CustomInfoHandler.InitialValue : _setTeamNum;
            _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, setValue, player);
            _assignedPlayers.Add(player); // �v���C���[���Z�b�g�ς݂ɒǉ�
        }

        private void OnTriggerExit(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient || !other.gameObject.CompareTag("Player")) return;

            if (!TryGetPlayer(other, out var player)) return;

            if (!_assignedPlayers.Contains(player)) return; // ���łɃ`�[������O��Ă���Ȃ牽�����Ȃ�

            _customInfoHandler.ChangeValue(CustomInfoHandler.TeamIdKey, CustomInfoHandler.InitialValue, player);
            _assignedPlayers.Remove(player); // �v���C���[���`�[������O�����̂Ń��X�g����폜
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
