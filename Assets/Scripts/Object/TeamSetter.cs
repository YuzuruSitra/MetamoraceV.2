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
            
            // Áπù‚?öÔøΩ?ΩºÁπùÔøΩÁ∏∫?ΩÆÁπùÂä±ŒûÁπß?Ω§ÁπùÔΩ§ÁπùÔΩºË¨®?Ω∞ÁπßÂÅµ„ÅçÁπß?Ω¶ÁπùÔΩ≥ÁπùÔøΩ
            var teamCount = CountPlayersInTeam(_setTeamNum);
            // Áπù‚?öÔøΩ?ΩºÁπùÔøΩÁ∏∫?Ω´2Ëé??Ω∫ËéâÔΩ•Ëç≥Áø´?ºûÁπßÂè•?øΩ?Ω¥Ëú∑Âåª?øΩ?ΩØ TeamOutValue ÁπßÂÆöÔΩ®?Ω≠Ëû≥?øΩ
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

        // Ë¨ñÔøΩËû≥Â£π?º?Á∏∫Ê∫ò„É°ÁπùÔΩºÁπùÔøΩÁ∏∫?Ω´Á∏∫?øΩÁπß‰πùÔøΩÂä±ŒûÁπß?Ω§ÁπùÔΩ§ÁπùÔΩºÁ∏∫?ΩÆË¨®?Ω∞ÁπßÂÅµ„ÅçÁπß?Ω¶ÁπùÔΩ≥ÁπùÂåª‚ò?Áπß‰πùŒìÁπß?ΩΩÁπùÔøΩÁπùÔøΩ
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
