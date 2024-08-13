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
        public event Action FinishEvent;
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
            if (!_isStart) return;
            if (_countTime > 0)
            {
                _countTime -= Time.deltaTime;
                return;
            }

            if (_battleTime <= 0) return;
            _battleTime -= Time.deltaTime;
            
            if (!(_battleTime <= 0)) return;
            _battleTime = 0;
            if (PhotonNetwork.IsMasterClient) photonView.RPC(nameof(SharedFinishGame), RpcTarget.All);
        }
        
        private void Launch()
        {
            photonView.RPC(nameof(SharedLaunchGame), RpcTarget.All);
        }
        
        [PunRPC]
        private void SharedLaunchGame()
        {
            _isStart = true;
        }
        
        [PunRPC]
        private void SharedFinishGame()
        {
            FinishEvent?.Invoke();
        }
    }
}
