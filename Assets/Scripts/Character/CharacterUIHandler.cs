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
        private float _canvasScaleFactor;

        void Start()
        {
            if (!photonView.IsMine) return;
            _nickNamePos = _nickName.GetComponent<RectTransform>();
            _mainCam = Camera.main;
            var canvas = _nickName.GetComponentInParent<Canvas>();
            _canvasScaleFactor = canvas.scaleFactor;
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

            // ターゲットのワールド座標をスクリーン座標に変換
            var screenPos = RectTransformUtility.WorldToScreenPoint(_mainCam, target.position);
            // スクリーン座標とCanvasのスケーリングを考慮してanchoredPositionを計算
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_nickNamePos.parent as RectTransform, screenPos, _mainCam, out var localPos);
            // プレイヤー名のオフセットを適用しつつ、スケールファクターを反映
            _nickNamePos.anchoredPosition = localPos / _canvasScaleFactor + _playerNameOffset;
        }

        [PunRPC]
        private void UpdateName()
        {
            if (_nickName.text == _name) return;
            _nickName.text = _name;
        }
    }
}
