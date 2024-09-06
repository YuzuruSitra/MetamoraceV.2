using System.Network;
using System.Sound;
using Object;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class TeamPanelHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private UnityEngine.UI.Text _roomText;
        [SerializeField] private UnityEngine.UI.Text[] _nameText;
        [SerializeField] private Button _startBt;
        private UiSeHandler _uiSeHandler;
        [SerializeField] private MatchLauncher _matchLauncher;
        
        private void Start()
        {
            _roomText.text = PhotonNetwork.MasterClient.NickName + "のルーム";
            if (!PhotonNetwork.IsMasterClient) _startBt.interactable = false;
            _uiSeHandler = UiSeHandler.InstanceUiSeHandler;
            _startBt.onClick.AddListener(_uiSeHandler.PushSound);
            _startBt.onClick.AddListener(_matchLauncher.GameStart);
            _matchLauncher.ChangeIsMatching += ChangeBtActive;
        }

        private void OnDestroy()
        {
            _matchLauncher.ChangeIsMatching -= ChangeBtActive;
        }

        private void ChangeBtActive(bool isActive)
        {
            _startBt.interactable = isActive;
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
                if (!customProperties.ContainsKey(CustomInfoHandler.TeamIdKey)) continue;
                var teamValue = (int)customProperties[CustomInfoHandler.TeamIdKey];
                switch (teamValue)
                {
                    case TeamSetter.TeamOutValue:
                        break;
                    case 1:
                        for (var i = 0; i < _nameText.Length / 2; i++)
                            if (_nameText[i].text == "")
                            {
                                _nameText[i].text = player.NickName;
                                break;
                            }
                        break;
                    case 2:
                        for (var i = _nameText.Length / 2; i < _nameText.Length; i++)
                            if (_nameText[i].text == "")
                            {
                                _nameText[i].text = player.NickName;
                                break;
                            }
                        break;
                }
            }
        }
    }
}
