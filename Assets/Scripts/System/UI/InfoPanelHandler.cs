using System.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class InfoPanelHandler : MonoBehaviour
    {
        [SerializeField] private Button _infoRuleBt;
        [SerializeField] private Button _infoHowToBt;
        [SerializeField] private Button _infoStoryBt;
        
        [SerializeField] private GameObject _rulePanel;
        [SerializeField] private GameObject _howToPanel;
        [SerializeField] private GameObject _storyPanel;
        private UiSeHandler _uiSeHandler;
        
        private void Start()
        {
            _uiSeHandler = UiSeHandler.InstanceUiSeHandler;
            _infoRuleBt.onClick.AddListener(InfoRule);
            _infoHowToBt.onClick.AddListener(InfoHowTo);
            _infoStoryBt.onClick.AddListener(InfoStory);
        }

        private void InfoRule()
        {
            AllCloseInfoPanel();
            _rulePanel.SetActive(true);
            _uiSeHandler.PushSound();
        }

        private void InfoHowTo()
        {
            AllCloseInfoPanel();
            _howToPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }

        private void InfoStory()
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
    }
}
