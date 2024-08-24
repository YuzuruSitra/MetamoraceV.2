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
        private bool _isStart;
        private bool _isStop;
        public event Action CountDownedEvent;
        public event Action FinishEvent;

        private bool _isCountDowned;
        private bool _isFinished;
        
        private void Start()
        {
            _isStart = false;
            _battleLauncher.BattleLaunch += Launch;
        }

        private void OnDestroy()
        {
            _battleLauncher.BattleLaunch -= Launch;
        }

        private void Update()
        {
            if (_isStop) return;
            if (!_isStart) return;
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
            _isStart = true;
        }

        public void ReceiveStopTime()
        {
            photonView.RPC(nameof(SharedStopTime), RpcTarget.All);
        }
        
        [PunRPC]
        public void SharedStopTime()
        {
            _isStop = true;
        }
        
        [PunRPC]
        private void SharedFinishGame()
        {
            FinishEvent?.Invoke();
        }
    }
}
