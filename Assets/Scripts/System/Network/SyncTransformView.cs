using Photon.Pun;
using UnityEngine;

namespace System.Network
{
    public class SyncTransformView : MonoBehaviourPunCallbacks, IPunObservable
    {
        // ��Ԃɂ����鎞��
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
                // ���v���C���[�̃l�b�g���[�N�I�u�W�F�N�g�́A��ԏ������s��
                _elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(_elapsedTime / InterpolationPeriod);

                // �ʒu�Ɖ�]�̕��
                transform.position = Vector3.Lerp(_previousPosition, _targetPosition, t);
                transform.rotation = Quaternion.Lerp(_previousRotation, _targetRotation, t);

                if (t >= 1f)
                {
                    // ��Ԃ�����������A��Ԃ̊J�n�ƏI���̈ʒu���X�V����
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
                // ���M���̏���
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            } 
            else 
            {
                // ��M���̏���
                _previousPosition = transform.position;
                _previousRotation = transform.rotation;

                // ��M�����ʒu�Ɖ�]���Ԃ̃^�[�Q�b�g�ɐݒ�
                _targetPosition = (Vector3)stream.ReceiveNext();
                _targetRotation = (Quaternion)stream.ReceiveNext();

                // ��Ԃ̌o�ߎ��Ԃ����Z�b�g
                _elapsedTime = 0f;
            }
        }
    }
}