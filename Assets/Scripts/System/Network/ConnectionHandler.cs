using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Network
{
    public class ConnectionHandler : MonoBehaviourPunCallbacks
    {
        public int ModeState { get; set; }
        public string RoomPas { get; set; }
        public string PlayerName { get; set; }
        [SerializeField] private string _sceneName;
        public const int MaxPlayer = 4;
        private readonly RoomOptions _option = new();
    
        private void Start()
        {
            ModeState = 1;
            RoomPas = "";
            _option.MaxPlayers = MaxPlayer;
        }
        
        public void JoinRoom()
        {
            switch (ModeState)
            {
                case 0:
                    break;
                case 1:
                    JoinRandomRoom();
                    break;
                case 2:
                    JoinSelectRoom();
                    break;
            }
        }

        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
        
        // ランダムなルームへの参加が失敗した時に呼ばれるコールバック
        public override void OnJoinRandomFailed(short returnCode, string message) {
            PhotonNetwork.CreateRoom(null, _option);
        }

        private void JoinSelectRoom()
        {
            PhotonNetwork.JoinOrCreateRoom(RoomPas, _option, TypedLobby.Default);
        }
        
        
        // ルームへの参加が成功した時に呼ばれるコールバック
        public override void OnJoinedRoom()
        {
            PhotonNetwork.NickName = PlayerName;
            //PhotonNetwork.LoadLevel(_sceneName);
            SceneManager.LoadScene(_sceneName);
        }
        
    }
}
