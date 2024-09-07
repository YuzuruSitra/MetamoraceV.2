using Photon.Pun;
using UnityEngine;
using System.Collections.Generic;

namespace System.Network
{
    public class SyncAnimatorView : MonoBehaviourPunCallbacks, IPunObservable
    {
        [SerializeField] private Animator _animator;

        private List<string> _boolParameters = new List<string>();
        private List<string> _floatParameters = new List<string>();

        private void Start()
        {
            if (_animator == null)
            {
                Debug.LogError("Animatorが設定されていません。");
                return;
            }

            // Animatorの全てのパラメーターを取得
            var parameters = _animator.parameters;
            foreach (var param in parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                {
                    _boolParameters.Add(param.name);
                }
                else if (param.type == AnimatorControllerParameterType.Float)
                {
                    _floatParameters.Add(param.name);
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 送信側の処理
                SendParameters(stream);
            }
            else
            {
                // 受信側の処理
                ReceiveParameters(stream);
            }
        }

        private void SendParameters(PhotonStream stream)
        {
            foreach (var paramName in _boolParameters)
            {
                stream.SendNext(_animator.GetBool(paramName));
            }

            foreach (var paramName in _floatParameters)
            {
                stream.SendNext(_animator.GetFloat(paramName));
            }
        }

        private void ReceiveParameters(PhotonStream stream)
        {
            foreach (var paramName in _boolParameters)
            {
                _animator.SetBool(paramName, (bool)stream.ReceiveNext());
            }

            foreach (var paramName in _floatParameters)
            {
                _animator.SetFloat(paramName, (float)stream.ReceiveNext());
            }
        }
    }
}
