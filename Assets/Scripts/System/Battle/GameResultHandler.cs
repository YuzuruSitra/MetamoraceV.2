using System.Collections;
using System.Network;
using System.Sound;
using Character;
using Photon.Pun;
using UnityEngine;

namespace System.Battle
{
    public class GameResultHandler : MonoBehaviourPunCallbacks
    {
        public bool IsGameFin { get; private set; }
        [SerializeField] private TimeHandler _timeHandler;
        [SerializeField] private BlockGenerator _blockGenerator;

        [SerializeField] private float _delayTime;
        private WaitForSeconds _waitFor;
        public event Action<int,LoseReason> CalcGameResult;
        private CharacterStatus _characterStatus;
        private CharacterPhotonStatus _characterPhotonStatus;
        private SoundHandler _soundHandler;
        [SerializeField] private AudioClip _finWhistleClip;
        //private string _loseReason;

        public enum LoseReason
        {
            HDeath,
            VDeath,
            CalcRate
        }
         private LoseReason _loseReason;
        
        private void Start()
        {
            _waitFor = new WaitForSeconds(_delayTime);
            
            _soundHandler = SoundHandler.InstanceSoundHandler;
            var playerGenerator = GameObject.FindWithTag("PlayerGenerator").GetComponent<PlayerGenerator>();
            var player = playerGenerator.CurrentPlayer;
            _characterStatus = player.GetComponent<CharacterStatus>();
            _characterPhotonStatus = player.GetComponent<CharacterPhotonStatus>();
            _characterStatus.ChangeConditionEvent += ReceiveCondition;

            if (!PhotonNetwork.IsMasterClient) return;
            _timeHandler.FinishEvent += CalcTimeResult;
        }

        private void OnDestroy()
        {
            _characterStatus.ChangeConditionEvent -= ReceiveCondition;
            if (!PhotonNetwork.IsMasterClient) return;
            _timeHandler.FinishEvent -= CalcTimeResult;
        }

        private void ReceiveCondition(CharacterStatus.Condition condition)
        {
            if (condition is CharacterStatus.Condition.HDeath or CharacterStatus.Condition.VDeath)
            {
                _loseReason = condition == CharacterStatus.Condition.HDeath ? LoseReason.HDeath: LoseReason.VDeath; // 追加: 敗因情報を設定
                StartCoroutine(ResultDelay());
            }
        }

        private IEnumerator ResultDelay()
        {
            photonView.RPC(nameof(ShareFinGame), RpcTarget.All);
            yield return _waitFor;
            Debug.Log(_waitFor);
            CalcDeathResult();
        }
        
        [PunRPC]
        public void ShareFinGame()
        {
            IsGameFin = true;
            _soundHandler.PlaySe(_finWhistleClip);
        }

        private void CalcDeathResult()
        {
            _timeHandler.ReceiveStopTime();
            var winTeamNum = 3 - _characterPhotonStatus.LocalPlayerTeamID;
            photonView.RPC(nameof(ShareCalc), RpcTarget.All, winTeamNum,_loseReason);
        }

        private void CalcTimeResult()
        {
            _timeHandler.ReceiveStopTime();
            var shareTeam1 = _blockGenerator.BlocksShareTeam1;
            var shareTeam2 = _blockGenerator.BlocksShareTeam2;
            var winTeamNum = (shareTeam1 == shareTeam2) ? 0 : (shareTeam1 < shareTeam2 ? 1 : 2);
            _loseReason = LoseReason.CalcRate; 
            photonView.RPC(nameof(ShareCalc), RpcTarget.All, winTeamNum, _loseReason);
        }

        [PunRPC]
        private void ShareCalc(int winTeamNum,LoseReason loseReason)
        {
            CalcGameResult?.Invoke(winTeamNum,loseReason);
        }
    }
}
