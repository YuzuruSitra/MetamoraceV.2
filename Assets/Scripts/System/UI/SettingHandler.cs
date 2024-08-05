using System.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class SettingHandler : MonoBehaviour
    {
        private SoundHandler _soundHandler;
        [SerializeField] private Slider _sliderBgm;
        [SerializeField] private Slider _sliderSe;

        private void Start()
        {
            _soundHandler = SoundHandler.InstanceSoundHandler;
            CheckVolume();
            _sliderBgm.onValueChanged.AddListener(ChangeBgmValue);
            _sliderSe.onValueChanged.AddListener(ChangeSeValue);
        }

        private void CheckVolume()
        {
            _sliderBgm.value = _soundHandler.BgmVolume;
            _sliderSe.value = _soundHandler.SeVolume;
        }
        
        private void ChangeBgmValue(float value)
        {
            _soundHandler.SetNewValueBgm(value);
        }
        private void ChangeSeValue(float value)
        {
            _soundHandler.SetNewValueSe(value);
        }
    }
}
