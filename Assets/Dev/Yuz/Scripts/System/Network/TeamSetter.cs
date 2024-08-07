using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace Dev.Yuz.Scripts.System.Network
{
    public class TeamSetter : MonoBehaviour
    {
        [SerializeField] private int _setTeamNum;
        public const int TeamOutValue = -1;
        public const string TeamKey = "Team";

        private readonly Dictionary<Collider, Player> _playerCache = new();

        // キャッシュ用の Hashtable
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
            
            _teamInProperties[TeamKey] = _setTeamNum;
            player.SetCustomProperties(_teamInProperties);
            Debug.Log($"Player {player.NickName} is now on team {_setTeamNum}");
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
        }
    }
}
