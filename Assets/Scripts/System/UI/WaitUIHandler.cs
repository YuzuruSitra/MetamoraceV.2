using System.Sound;
using UnityEngine;

namespace System.UI
{
    public class WaitUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _settingPanel;
        [SerializeField] private GameObject _infoPanel;
        [SerializeField] private GameObject _rulePanel;
        [SerializeField] private GameObject _howToPanel;
        [SerializeField] private GameObject _storyPanel;
        [SerializeField] private GameObject _exitPanel;
        private UiSeHandler _uiSeHandler;
        
        private void Start()
        {
            _uiSeHandler = UiSeHandler.InstanceUiSeHandler;
        }

        public void OpenSetting()
        {
            _settingPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }
        public void CloseSetting()
        {
            _settingPanel.SetActive(false);
            _uiSeHandler.PushSound();
        }
        public void OpenInfo()
        {
            _infoPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }
        public void CloseInfo()
        {
            _infoPanel.SetActive(false);
            _uiSeHandler.PushSound();
        }
        
        public void InfoRule()
        {
            AllCloseInfoPanel();
            _rulePanel.SetActive(true);
            _uiSeHandler.PushSound();
        }

        public void InfoHowTo()
        {
            AllCloseInfoPanel();
            _howToPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }

        public void InfoStory()
        {
            AllCloseInfoPanel();
            _storyPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }

        private void AllCloseInfoPanel()
        {
            _rulePanel.SetActive(false);
            _howToPanel.SetActive(false);
            _storyPanel.SetActive(false);
        }

        public void OpenExitPanel()
        {
            _exitPanel.SetActive(true);
        }
        public void CloseExitPanel()
        {
            _exitPanel.SetActive(false);
        }
        
    }
}
