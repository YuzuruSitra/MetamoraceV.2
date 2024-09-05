using System.Network;
using Photon.Pun;
using UnityEngine;

namespace System.UI
{
    public class BattleNameHandler : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Text[] _headerNames;
        
        private void Start()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamIdKey)) continue;
                var id = (int)teamIdKey;
                if (_headerNames.Length - 1 < id) continue;
                _headerNames[id].text = player.NickName;
            }
        }

    }
}
