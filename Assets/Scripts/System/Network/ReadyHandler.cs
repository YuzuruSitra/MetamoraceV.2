using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Network
{
    public class ReadyHandler : MonoBehaviour
    {
        private CustomInfoHandler _customInfoHandler;
        private void Start()
        {
            var objectsWithSameTag = GameObject.FindGameObjectsWithTag(gameObject.tag);
            
            if (objectsWithSameTag.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            
            _customInfoHandler = CustomInfoHandler.Instance;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Master_Title":
                    break;
                case "Master_Wait":
                    _customInfoHandler.ChangeValue(CustomInfoHandler.ReadyKey, 0, PhotonNetwork.LocalPlayer);
                    break;
                case "Master_Battle":
                    _customInfoHandler.ChangeValue(CustomInfoHandler.ReadyKey, 1, PhotonNetwork.LocalPlayer);
                    break;
            }
        }
    }
}
