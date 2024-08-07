using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Dev.Yuz.Scripts.System.Network
{
    public class MatchLauncher : MonoBehaviourPunCallbacks
    {
        // 他プレイヤーがルームへ参加した時に呼ばれるコールバック
        public override void OnPlayerEnteredRoom(Player newPlayer) {
            Debug.Log($"{newPlayer.NickName}が参加しました");
        }

        // 他プレイヤーがルームから退出した時に呼ばれるコールバック
        public override void OnPlayerLeftRoom(Player otherPlayer) {
            Debug.Log($"{otherPlayer.NickName}が退出しました");
        }
        
    }
}
