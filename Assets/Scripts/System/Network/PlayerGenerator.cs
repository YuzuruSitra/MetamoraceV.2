using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace System.Network
{
    public class PlayerGenerator : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject[] _waitCharacter;
        [SerializeField] private GameObject[] _battleCharacter;
        [SerializeField] private Vector3 _waitInsPos;
        [SerializeField] private Vector3[] _battleInsPos;
        public GameObject CurrentPlayer { get; private set; }
        
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
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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
                    CurrentPlayer = PhotonNetwork.Instantiate(_waitCharacter[id].name, _waitInsPos, Quaternion.identity);
                    break;
                case "Master_Battle":
                    if (id >= _battleCharacter.Length)
                    {
                        Debug.LogWarning("ID Error");
                        return;
                    }
                    if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.MemberIdKey, out var teamId)) return;
                    var posID = (int)teamId;
                    CurrentPlayer = PhotonNetwork.Instantiate(_battleCharacter[id].name, _battleInsPos[posID], Quaternion.identity);
                    break;
            }
        }
    }
}
