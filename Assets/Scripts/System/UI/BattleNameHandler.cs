using System.Network;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

namespace System.UI
{
    public class BattleNameHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private UnityEngine.UI.Text[] _headerNames;
        
        private void Start()
        {
            UpdateNameList();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdateNameList();
        }
        
        private void UpdateNameList()
        {
            foreach (var playerText in _headerNames)
            {
                playerText.text = "";
            }
            foreach (var player in PhotonNetwork.PlayerList)
            {
                var num = player.ActorNumber - 1;
                if (num < _headerNames.Length) _headerNames[num].text = player.NickName;
            }
        }



    }
}
