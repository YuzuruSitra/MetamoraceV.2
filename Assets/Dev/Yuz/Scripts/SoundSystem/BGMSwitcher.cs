using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dev.Yuz.Scripts.SoundSystem
{
    public class BGMSwitcher : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _bgm;
        private SoundHandler _soundHandler;
        private void Start()
        {
            var objectsWithSameTag = GameObject.FindGameObjectsWithTag(gameObject.tag);
            
            if (objectsWithSameTag.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            _soundHandler = SoundHandler.InstanceSoundHandler;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
            _soundHandler.PlayBgm(_bgm[0]);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Master_Title":
                    _soundHandler.PlayBgm(_bgm[0]);
                    break;
                case "Master_Wait":
                    _soundHandler.PlayBgm(_bgm[1]);
                    break;
                case "Master_Battle":
                    _soundHandler.PlayBgm(_bgm[2]);
                    break;
            }
        }
    }
}
