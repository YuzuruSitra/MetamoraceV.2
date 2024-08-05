using Photon.Pun;
using UnityEngine;

namespace Dev.Yuz.Scripts.NetworkSystem
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
