using Photon.Pun;
using UnityEngine;

namespace System.Battle
{
    public class TimeHandler : MonoBehaviourPunCallbacks
    {
        [Header("Seconds")]
        [SerializeField] private float _battleTime;
        public float BattleTime => _battleTime;
        [Header("Seconds")]
        [SerializeField] private float _countTime;
        public float CountTime => _countTime;
        
        [SerializeField] private BattleLauncher _battleLauncher;
        public bool IsCountDown { get; private set; }
        public event Action CountDownedEvent;
        public event Action FinishEvent;

        private bool _isCountDowned;
        private bool _isFinished;
        
        private void Start()
        {
            _battleLauncher.BattleLaunch += Launch;
        }

        private void OnDestroy()
        {
            _battleLauncher.BattleLaunch -= Launch;
        }

        private void Update()
        {
            if (!IsCountDown) return;
            if (_countTime > 0)
            {
                _countTime -= Time.deltaTime;
                return;
            }

            if (!_isCountDowned)
            {
                CountDownedEvent?.Invoke();
                _isCountDowned = true;
            }
            _battleTime -= Time.deltaTime;
            if (_battleTime > 0) return;
            if (!PhotonNetwork.IsMasterClient) return;
            if (_isFinished) return;
            photonView.RPC(nameof(SharedFinishGame), RpcTarget.All);
            _isFinished = true;
        }
        
        private void Launch()
        {
            photonView.RPC(nameof(SharedLaunchGame), RpcTarget.All);
        }
        
        [PunRPC]
        public void SharedLaunchGame()
        {
            IsCountDown = true;
        }

        public void ReceiveStopTime()
        {
            photonView.RPC(nameof(SharedStopTime), RpcTarget.All);
        }
        
        [PunRPC]
        public void SharedStopTime()
        {
            IsCountDown = false;
        }
        
        [PunRPC]
        private void SharedFinishGame()
        {
            _battleTime = 0;
            IsCountDown = false;
            FinishEvent?.Invoke();
        }
    }
}
