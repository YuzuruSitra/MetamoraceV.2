using Photon.Pun;
using UnityEngine;

namespace Object
{
    public class BlockDestroyer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (other.gameObject.layer == LayerMask.NameToLayer("Block")) 
                PhotonNetwork.Destroy(other.gameObject);
        }
    }
}