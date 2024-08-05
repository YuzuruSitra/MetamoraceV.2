using UnityEngine;

namespace System.Sound
{
    public class UiSeHandler : MonoBehaviour
    {
        public static UiSeHandler InstanceUiSeHandler;
        [SerializeField] private SoundHandler _soundHandler;
        [SerializeField] private AudioClip _pushBt;
        [SerializeField] private AudioClip _inputText;

        private void Awake()
        {
            if (InstanceUiSeHandler != null)
            {
                Destroy(gameObject);
                return;
            }

            InstanceUiSeHandler = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PushSound()
        {
            _soundHandler.PlaySe(_pushBt);
        }
        
        public void InputSound()
        {
            _soundHandler.PlaySe(_inputText);
        }
    }
}
