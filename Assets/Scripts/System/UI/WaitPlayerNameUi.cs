using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class WaitPlayerNameUi : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text[] _playerTexts;

    private void Start()
    {
        UpdateAllMemberText();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerText(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ClearPlayerText(otherPlayer);
    }

    private void UpdateAllMemberText()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            var num = player.ActorNumber - 1;
            if (_playerTexts.Length <= num) return;
            _playerTexts[num].text = player.NickName;
        }
    }

    private void UpdatePlayerText(Player player)
    {
        int num = player.ActorNumber - 1;
        if (num < _playerTexts.Length)
        {
            _playerTexts[num].text = player.NickName;
        }
    }

    private void ClearPlayerText(Player player)
    {
        int num = player.ActorNumber - 1;
        if (num < _playerTexts.Length)
        {
            _playerTexts[num].text = "";
        }
    }

    private void UpdateMemberList()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            UpdatePlayerText(player);
        }
    }
}
