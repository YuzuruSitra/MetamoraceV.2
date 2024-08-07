using Dev.Yuz.Scripts.System.Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Dev.Yuz.Scripts.UI
{
    public class TeamPanelHandler : MonoBehaviourPunCallbacks
    {

        [SerializeField] private Text[] _nameText;
        [SerializeField] private Button _startBt;

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient) _startBt.interactable = false;
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            ChangeTeamName();
        }

        private void ChangeTeamName()
        {
            foreach (var t in _nameText) t.text = "";

            var players = PhotonNetwork.PlayerList;
            foreach (var player in players)
            {
                var customProperties = player.CustomProperties;
                if (!customProperties.ContainsKey(TeamSetter.TeamKey)) continue;
                var teamValue = (int)customProperties["Team"];
                switch (teamValue)
                {
                    case TeamSetter.TeamOutValue:
                        break;
                    case 1:
                        for (var i = 0; i < _nameText.Length / 2; i++)
                            if (_nameText[i].text == "") _nameText[i].text = player.NickName;
                        break;
                    case 2:
                        for (var i = _nameText.Length / 2; i < _nameText.Length; i++)
                            if (_nameText[i].text == "") _nameText[i].text = player.NickName;
                        break;
                }
            }
        }
    }
}
