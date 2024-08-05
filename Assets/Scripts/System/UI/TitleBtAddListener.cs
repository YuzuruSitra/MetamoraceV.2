using System.Network;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class TitleBtAddListener : MonoBehaviour
    {
        [SerializeField] private Button _randomMachBt;
        [SerializeField] private Button _privateMachBt;
        [SerializeField] private Button _joinBt;
        [SerializeField] private Button _returnPassBt;
        [SerializeField] private Button _openSettingBt;
        [SerializeField] private Button _closeSettingBt;
        [SerializeField] private Button _openInfoBt;
        [SerializeField] private Button _closeInfoBt;
        [SerializeField] private Button _infoRuleBt;
        [SerializeField] private Button _infoHowToBt;
        [SerializeField] private Button _infoStoryBt;
        
        [SerializeField] private InputField _passInput;
        [SerializeField] private InputField _nameInput;
        [SerializeField] private Slider _sliderBgm;
        [SerializeField] private Slider _sliderSe;
        
        [SerializeField] private TitleUIHandler _titleUIHandler;
        [SerializeField] private ConnectionHandler _connectionHandler;
        public const string Key = "Player";
        
        private void Start()
        {
            CheckName();
            _randomMachBt.onClick.AddListener(_titleUIHandler.SetRandomRoom);
            _privateMachBt.onClick.AddListener(_titleUIHandler.SetPrivateRoom);
            _joinBt.onClick.AddListener(_connectionHandler.JoinRoom);
            _returnPassBt.onClick.AddListener(_titleUIHandler.ClosePasPanel);
            _openSettingBt.onClick.AddListener(_titleUIHandler.OpenSetting);
            _closeSettingBt.onClick.AddListener(_titleUIHandler.CloseSetting);
            _openInfoBt.onClick.AddListener(_titleUIHandler.OpenInfo);
            _closeInfoBt.onClick.AddListener(_titleUIHandler.CloseInfo);
            _infoRuleBt.onClick.AddListener(_titleUIHandler.InfoRule);
            _infoHowToBt.onClick.AddListener(_titleUIHandler.InfoHowTo);
            _infoStoryBt.onClick.AddListener(_titleUIHandler.InfoStory);
            _passInput.onValueChanged.AddListener(_titleUIHandler.DonePassInput);
            _nameInput.onValueChanged.AddListener(_titleUIHandler.DoneNameInput);
            _sliderBgm.onValueChanged.AddListener(_titleUIHandler.ChangeBgmValue);
            _sliderSe.onValueChanged.AddListener(_titleUIHandler.ChangeSeValue);
        }

        private void CheckName()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                var playerName = PlayerPrefs.GetString(Key);
                _nameInput.text = playerName;
                _titleUIHandler.DoneNameInput(playerName);
            }
            else
            {
                var playerName = "NoName";
                _nameInput.text = playerName;
                _titleUIHandler.DoneNameInput(playerName);
            }
        }
    }
}
