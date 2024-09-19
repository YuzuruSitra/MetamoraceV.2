using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class CharacterUIHandler : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Text _nickName;
        private RectTransform _nickNamePos;
        [SerializeField] private Transform target;
        [SerializeField] private Vector2 _playerNameOffset;
        [SerializeField] private CharacterPhotonStatus _characterPhotonStatus;
        private string _name;
        private Camera _mainCam;
        private Vector2 _baseScale = new Vector2(1920, 1080);

        void Start()
        {
            _nickNamePos = _nickName.GetComponent<RectTransform>();
            _mainCam = Camera.main;

            if (!photonView.IsMine) return;
            _name = _characterPhotonStatus.LocalPlayerName;
            UpdateName();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            photonView.RPC(nameof(UpdateName), RpcTarget.Others);
        }

        void Update()
        {
            // メインカメラが存在するかチェック
            if (_mainCam == null)
            {
                Debug.LogError("Main camera not found!");
                return;
            }

            var factor = Vector2.zero;
            factor.x = Screen.width / _baseScale.x;
            factor.y = Screen.height / _baseScale.y;

            var screenPos = RectTransformUtility.WorldToScreenPoint(_mainCam, target.position);
            _nickNamePos.anchoredPosition = screenPos + _playerNameOffset * factor;
        }

        [PunRPC]
        private void UpdateName()
        {
            if (_nickName.text == _name) return;
            _nickName.text = _name;
        }
    }
}
