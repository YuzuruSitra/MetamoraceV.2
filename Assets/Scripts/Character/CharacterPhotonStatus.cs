using Photon.Pun;
using System.Network;
using UnityEngine;

namespace Character
{
    public class CharacterPhotonStatus : MonoBehaviourPunCallbacks
    {
        public string LocalPlayerName { get; private set; }
        public int LocalPlayerMemberID { get; private set; }
        public int LocalPlayerTeamID { get; private set; }

        [SerializeField] private bool _isWaitScene;
        private void Awake()
        {
            LocalPlayerName = photonView.Owner.NickName;
            if (!photonView.IsMine) return;
            if (_isWaitScene) return;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.MemberIdKey, out var memberId))
                LocalPlayerMemberID = (int)memberId;
            LocalPlayerTeamID = LocalPlayerMemberID < 2 ? 1 : 2;
        }
    }
}