using System.Network;
using System.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace System.UI
{
    public class ExitPanelHandler : MonoBehaviour
    {
        [SerializeField] private Button _yesBt;
        [SerializeField] private Button _noBt;
        private UiSeHandler _uiSeHandler;
        [SerializeField] private MatchLauncher _matchLauncher;
        [SerializeField] private WaitUIHandler _waitUIHandler;
        
        private void Start()
        {
            _uiSeHandler = UiSeHandler.InstanceUiSeHandler;
            
            _yesBt.onClick.AddListener(_uiSeHandler.PushSound);
            _yesBt.onClick.AddListener(_matchLauncher.LeaveRoom);
            _noBt.onClick.AddListener(_uiSeHandler.PushSound);
            _noBt.onClick.AddListener(_waitUIHandler.CloseExitPanel);
        }
        
    }
}
