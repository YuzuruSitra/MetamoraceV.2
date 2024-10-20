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
        [SerializeField] private Button _infoItemBt;
        [SerializeField] private Button _infoDuelBt;
        
        [SerializeField] private GameObject _rulePanel;
        [SerializeField] private GameObject _howToPanel;
        [SerializeField] private GameObject _storyPanel;
        [SerializeField] private GameObject _itemPanel;
        [SerializeField] private GameObject _duelPanel;
        private UiSeHandler _uiSeHandler;
        
        private void Start()
        {
            _uiSeHandler = UiSeHandler.InstanceUiSeHandler;
            _infoRuleBt.onClick.AddListener(InfoRule);
            _infoHowToBt.onClick.AddListener(InfoHowTo);
            _infoStoryBt.onClick.AddListener(InfoStory);
            _infoItemBt.onClick.AddListener(InfoItem);
            _infoDuelBt.onClick.AddListener(InfoDuel);
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

        private void InfoItem()
        {
            AllCloseInfoPanel();
            _itemPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }
        private void InfoDuel()
        {
            AllCloseInfoPanel();
            _duelPanel.SetActive(true);
            _uiSeHandler.PushSound();
        }
        private void AllCloseInfoPanel()
        {
            _rulePanel.SetActive(false);
            _howToPanel.SetActive(false);
            _storyPanel.SetActive(false);
            _itemPanel.SetActive(false);
            _duelPanel.SetActive(false);
        }
    }
}
