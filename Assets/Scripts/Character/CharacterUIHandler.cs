using Photon.Pun;
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
        private Camera _mainCam;

        void Start()
        {
            _nickNamePos = _nickName.GetComponent<RectTransform>();
            var playerName = _characterPhotonStatus.LocalPlayerName;
            _nickName.text = playerName;
            _mainCam = Camera.main;
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
            _nickNamePos.anchoredPosition = screenPos + _playerNameOffset;
        }

    }
}
