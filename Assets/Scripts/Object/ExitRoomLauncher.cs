using System.UI;
using Photon.Pun;
using UnityEngine;

namespace Object
{
    public class ExitRoomLauncher : MonoBehaviour
    {
        [SerializeField] private WaitUIHandler _waitUIHandler;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var photonView = other.GetComponent<PhotonView>();
            if (photonView == null) return;
            if (!photonView.IsMine) return;
            _waitUIHandler.OpenExitPanel();
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var photonView = other.GetComponent<PhotonView>();
            if (photonView == null) return;
            if (!photonView.IsMine) return;
            _waitUIHandler.CloseExitPanel();
        }
        
    }
}
