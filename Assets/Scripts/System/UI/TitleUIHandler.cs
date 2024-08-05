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
        
        [SerializeField] private ConnectionHandler _connectionHandler;
        private UiSeHandler _uiSeHandler;
        
        private void Start()
        {
            _uiSeHandler = UiSeHandler.InstanceUiSeHandler;
            _buttonImage[0].sprite = _randomSprite[1];
            _buttonImage[1].sprite = _privateSprite[0];
        }

        public void SetRandomRoom()
        {
            _connectionHandler.ModeState = 1;
            _buttonImage[0].sprite = _randomSprite[1];
            _buttonImage[1].sprite = _privateSprite[0];
            _uiSeHandler.PushSound();
        }
        public void SetPrivateRoom()
        {
            _connectionHandler.ModeState = 2;
            _buttonImage[0].sprite = _randomSprite[0];
            _buttonImage[1].sprite = _privateSprite[1];
            _defaultPanel.SetActive(false);
            _pasPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }

        public void DonePassInput(string pass)
        {
            _connectionHandler.RoomPas = pass;
            _uiSeHandler.InputSound();
        }
        
        public void DoneNameInput(string playerName)
        {
            _connectionHandler.PlayerName = playerName;
            PlayerPrefs.SetString(TitleBtAddListener.Key, playerName);
            _uiSeHandler.InputSound();
        }

        public void ClosePasPanel()
        {
            _defaultPanel.SetActive(true);
            _pasPanel.SetActive(false);
            _uiSeHandler.PushSound();
        }

        public void OpenSetting()
        {
            _defaultPanel.SetActive(false);
            _settingPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }
        public void CloseSetting()
        {
            _defaultPanel.SetActive(true);
            _settingPanel.SetActive(false);
            _uiSeHandler.PushSound();
        }
        public void OpenInfo()
        {
            _defaultPanel.SetActive(false);
            _infoPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }
        public void CloseInfo()
        {
            _defaultPanel.SetActive(true);
            _infoPanel.SetActive(false);
            _uiSeHandler.PushSound();
        }
        
    }
}