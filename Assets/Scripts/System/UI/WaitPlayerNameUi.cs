using System.Linq;
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
        UpdateMemberList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateMemberList();
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

    private void UpdateMemberList()
    {
        foreach (var playerText in _playerTexts)
        {
            playerText.text = "";
        }
        var sortedPlayerList = PhotonNetwork.PlayerList
            .OrderBy(player => player.ActorNumber)
            .ToArray();
        for (var i = 0; i < sortedPlayerList.Length; i++)
        {
            if (i < _playerTexts.Length) _playerTexts[i].text = sortedPlayerList[i].NickName;
            Debug.Log(sortedPlayerList[i].NickName);
        }
    }
}
