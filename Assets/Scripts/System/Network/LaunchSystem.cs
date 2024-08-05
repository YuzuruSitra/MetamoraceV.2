using Photon.Pun;
using UnityEngine;

namespace System.Network
{
    public class LaunchSystem : MonoBehaviour
    {
        private void Awake()
        {
            PhotonNetwork.ConnectUsingSettings();
            Application.targetFrameRate = 60;
        }

    }
}
