using System.Network;
using System.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class TitleUIHandler : MonoBehaviour
    {
        [SerializeField] private Image[] _buttonImage = new Image[2];
        [SerializeField] private Sprite[] _randomSprite = new Sprite[2];
        [SerializeField] private Sprite[] _privateSprite = new Sprite[2];
        [SerializeField] private GameObject _defaultPanel;
        [SerializeField] private GameObject _pasPanel;
        [SerializeField] private GameObject _settingPanel;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _rulePanel;
        [SerializeField] private GameObject _howToPanel;
        [SerializeField] private GameObject _storyPanel;
        
        [SerializeField] private ConnectionHandler _connectionHandler;
        
        private SoundHandler _soundHandler;
        [SerializeField] private AudioClip _buttonSe;
        [SerializeField] private AudioClip _inputSe;
        
        private void Start()
        {
            _soundHandler = SoundHandler.InstanceSoundHandler;
            _buttonImage[0].sprite = _randomSprite[1];
            _buttonImage[1].sprite = _privateSprite[0];
        }

        public void SetRandomRoom()
        {
            _connectionHandler.ModeState = 1;
            _buttonImage[0].sprite = _randomSprite[1];
            _buttonImage[1].sprite = _privateSprite[0];
            PushSe();
        }
        public void SetPrivateRoom()
        {
            _connectionHandler.ModeState = 2;
            _buttonImage[0].sprite = _randomSprite[0];
            _buttonImage[1].sprite = _privateSprite[1];
            _defaultPanel.SetActive(false);
            _pasPanel.SetActive(true);
            PushSe();
        }

        public void DonePassInput(string pass)
        {
            _connectionHandler.RoomPas = pass;
            InputSe();
        }
        
        public void DoneNameInput(string playerName)
        {
            _connectionHandler.PlayerName = playerName;
            PlayerPrefs.SetString(TitleBtAddListener.Key, playerName);
            InputSe();
        }

        public void ClosePasPanel()
        {
            _defaultPanel.SetActive(true);
            _pasPanel.SetActive(false);
            PushSe();
        }

        public void OpenSetting()
        {
            _defaultPanel.SetActive(false);
            _settingPanel.SetActive(true);
            PushSe();
        }
        public void CloseSetting()
        {
            _defaultPanel.SetActive(true);
            _settingPanel.SetActive(false);
            PushSe();
        }
        public void OpenInfo()
        {
            _defaultPanel.SetActive(false);
            _infoPanel.SetActive(true);
            PushSe();
        }
        public void CloseInfo()
        {
            _defaultPanel.SetActive(true);
            _infoPanel.SetActive(false);
            PushSe();
        }
        
        public void InfoRule()
        {
            AllCloseInfoPanel();
            _rulePanel.SetActive(true);
            PushSe();
        }

        public void InfoHowTo()
        {
            AllCloseInfoPanel();
            _howToPanel.SetActive(true);
            PushSe();
        }

        public void InfoStory()
        {
            AllCloseInfoPanel();
            _storyPanel.SetActive(true);
            PushSe();
        }

        private void AllCloseInfoPanel()
        {
            _rulePanel.SetActive(false);
            _howToPanel.SetActive(false);
            _storyPanel.SetActive(false);
        }

        private void PushSe()
        {
            _soundHandler.PlaySe(_buttonSe);
        }

        private void InputSe()
        {
            _soundHandler.PlaySe(_inputSe);
        }

        public void ChangeBgmValue(float value)
        {
            _soundHandler.SetNewValueBgm(value);
        }
        public void ChangeSeValue(float value)
        {
            _soundHandler.SetNewValueSe(value);
        }

    }
}