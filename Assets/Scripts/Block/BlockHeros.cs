using System.Battle;
using System.Network;
using Photon.Pun;
using UnityEngine;

namespace Block
{
    public class BlockHeros : BlockBase
    {
        private bool _completeMove;
        private float _targetPosZ;
        [SerializeField] private float _moveSpeed;
        private Vector3 _currentPos;
        private const float Threshold = 0.01f;
        
        protected override void Start()
        {
            base.Start();
            // メモ：生成されたチームの位置から目標地点を設定したほうがよき
            if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomInfoHandler.TeamIdKey, out var teamId)) return;
            _targetPosZ = ((int)teamId == 0) ? BlockGenerator.Team2PosZ : BlockGenerator.Team1PosZ;
            _currentPos = transform.position;
        }
        
        protected override void Update()
        {
            if (_completeMove)
                base.Update();
            else
                TowardsPos();
        }

        private void TowardsPos()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            var step = _moveSpeed * Time.deltaTime;
            _currentPos.z = Mathf.MoveTowards(_currentPos.z, _targetPosZ, step);
            transform.position = _currentPos;

            if (!(Mathf.Abs(_currentPos.z - _targetPosZ) < Threshold)) return;
            _currentPos.z = _targetPosZ;
            transform.position = _currentPos;
            _completeMove = true;
        }
    }
}