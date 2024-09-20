using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Network;

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
        private Vector2 _baseScale = new Vector2(1920, 1080);

        void Start()
        {
            _nickNamePos = _nickName.GetComponent<RectTransform>();
            _mainCam = Camera.main;
            var playerName = _characterPhotonStatus.LocalPlayerName;
            _nickName.text = playerName;
        }

        void Update()
        {
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
    }
}
