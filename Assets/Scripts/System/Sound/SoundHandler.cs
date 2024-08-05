using UnityEngine;
using UnityEngine.UI;

namespace System.Sound
{
    public class SoundHandler : MonoBehaviour
    {
        public static SoundHandler InstanceSoundHandler;
        [SerializeField]
        private AudioSource _bgmAudioSource;

        public float BgmVolume => _bgmAudioSource.volume;
        [SerializeField]
        private AudioSource _seAudioSource;
        public float SeVolume => _seAudioSource.volume;

        private void Awake()
        {
            if (InstanceSoundHandler != null)
            {
                Destroy(gameObject);
                return;
            }

            InstanceSoundHandler = this;
            DontDestroyOnLoad(gameObject);
        }
        
        // BGM音量変更
        public void SetNewValueBgm(float newValueBGM)
        {
            _bgmAudioSource.volume = Mathf.Clamp01(newValueBGM);
        }

        // BGM音量変更
        public void SetNewValueSe(float newValueSe)
        {
            _seAudioSource.volume = Mathf.Clamp01(newValueSe);
        }

        // スライダーの値を変更
        public void ChangeSliderValue(Slider bgm, Slider se)
        {
            bgm.value = _bgmAudioSource.volume;
            se.value = _seAudioSource.volume;
        }

        public void PlayBgm(AudioClip clip)
        {
            _bgmAudioSource.clip = clip;
            if(clip == null)
            {
                return;
            }
            _bgmAudioSource.Play();
        }

        public void PlaySe(AudioClip clip)
        {   
            if(clip == null)
            {
                return;
            }

            _seAudioSource.PlayOneShot(clip);
        }
    }
}