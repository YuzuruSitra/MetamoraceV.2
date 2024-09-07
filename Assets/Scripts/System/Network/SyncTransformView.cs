using Photon.Pun;
using UnityEngine;

namespace System.Network
{
    public class SyncTransformView : MonoBehaviourPunCallbacks, IPunObservable
    {
        // 補間にかける時間
        private const float InterpolationPeriod = 0.1f;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;
        private Vector3 _previousPosition;
        private Quaternion _previousRotation;
        private float _elapsedTime;

        private void Start() 
        {
            _previousPosition = transform.position;
            _previousRotation = transform.rotation;
            _targetPosition = _previousPosition;
            _targetRotation = _previousRotation;
        }

        private void Update() 
        {
            if (!photonView.IsMine) 
            {
                // 他プレイヤーのネットワークオブジェクトは、補間処理を行う
                _elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(_elapsedTime / InterpolationPeriod);

                // 位置と回転の補間
                transform.position = Vector3.Lerp(_previousPosition, _targetPosition, t);
                transform.rotation = Quaternion.Lerp(_previousRotation, _targetRotation, t);

                if (t >= 1f)
                {
                    // 補間が完了したら、補間の開始と終了の位置を更新する
                    _previousPosition = transform.position;
                    _previousRotation = transform.rotation;
                    _elapsedTime = 0f;
                }
            }
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
        {
            if (stream.IsWriting) 
            {
                // 送信側の処理
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            } 
            else 
            {
                // 受信側の処理
                _previousPosition = transform.position;
                _previousRotation = transform.rotation;

                // 受信した位置と回転を補間のターゲットに設定
                _targetPosition = (Vector3)stream.ReceiveNext();
                _targetRotation = (Quaternion)stream.ReceiveNext();

                // 補間の経過時間をリセット
                _elapsedTime = 0f;
            }
        }
    }
}