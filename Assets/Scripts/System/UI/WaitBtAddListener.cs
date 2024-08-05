using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class WaitBtAddListener : MonoBehaviour
    {
        [SerializeField] private Button _openSettingBt;
        [SerializeField] private Button _closeSettingBt;
        [SerializeField] private Button _openInfoBt;
        [SerializeField] private Button _closeInfoBt;
        [SerializeField] private Button _infoRuleBt;
        [SerializeField] private Button _infoHowToBt;
        [SerializeField] private Button _infoStoryBt;


        [SerializeField] private WaitUIHandler _waitUIHandler;
        
        private void Start()
        {
            _openSettingBt.onClick.AddListener(_waitUIHandler.OpenSetting);
            _closeSettingBt.onClick.AddListener(_waitUIHandler.CloseSetting);
            _openInfoBt.onClick.AddListener(_waitUIHandler.OpenInfo);
            _closeInfoBt.onClick.AddListener(_waitUIHandler.CloseInfo);
            _infoRuleBt.onClick.AddListener(_waitUIHandler.InfoRule);
            _infoHowToBt.onClick.AddListener(_waitUIHandler.InfoHowTo);
            _infoStoryBt.onClick.AddListener(_waitUIHandler.InfoStory);
        }
        
    }
}
