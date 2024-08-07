using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev.Yuz.Scripts.System.Network
{
    public class PlayerGenerator : MonoBehaviourPunCallbacks
    {
        [SerializeField] private string[] _waitCharacter;
        [SerializeField] private string[] _battleCharacter;
        [SerializeField] private Vector3 _waitInsPos;
        [SerializeField] private Vector3[] _battleInsPos;
        
        private void Start()
        {
            var objectsWithSameTag = GameObject.FindGameObjectsWithTag(gameObject.tag);
            
            if (objectsWithSameTag.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var id = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            switch (scene.name)
            {
                case "Master_Title":
                    break;
                case "Master_Wait":
                    if (id >= _waitCharacter.Length)
                    {
                        Debug.LogWarning("ID Error");
                        return;
                    }
                    PhotonNetwork.Instantiate(_waitCharacter[id], _waitInsPos, Quaternion.identity);
                    break;
                case "Master_Battle":
                    if (id >= _battleCharacter.Length)
                    {
                        Debug.LogWarning("ID Error");
                        return;
                    }
                    PhotonNetwork.Instantiate(_battleCharacter[id], _battleInsPos[id], Quaternion.identity);
                    break;
            }
        }
    }
}
