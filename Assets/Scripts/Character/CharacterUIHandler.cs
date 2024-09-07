using System.Battle;
using System.Collections;
using System.Collections.Generic;
using System.Network;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class CharacterUIHandler : MonoBehaviour
    {
        [SerializeField] private Text _nickName;
        private string playerName;
        void Start()
        {
             playerName = PhotonNetwork.LocalPlayer.NickName;
             ApplyNickName(playerName);
        }

        void ApplyNickName(string PlayerName)
        {
            if(PlayerName == null)_nickName.text = "NoName";
            _nickName.text = PlayerName;
        }
        
    }
}
