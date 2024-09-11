using System.Battle;
using System.Collections;
using System.Collections.Generic;
using System.Network;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class CharacterUIHandler : MonoBehaviour
    {
        [SerializeField] private Text _nickName;
        private string playerName;
        private readonly Vector2 _nameOffSets = new(0f, 0.8f);
        private RectTransform _nickNamePos;
        [SerializeField] private Transform target;

        [SerializeField] private float playerNameOffsetY;
        private Vector2 playerNameOffset;


        void Start()
        {
            _nickNamePos = _nickName.GetComponent<RectTransform>();

            playerName = PhotonNetwork.LocalPlayer.NickName;
            ApplyNickName(playerName);
        }

        void Update()
        {
            // メインカメラが存在するかチェック
            if (Camera.main == null)
            {
                Debug.LogError("Main camera not found!");
                return;
            }

            // Yオフセットを再設定（ここで調整）
            playerNameOffset = new Vector2(0f, playerNameOffsetY);

            // ターゲットのワールド座標をスクリーン座標に変換し、オフセットを適用
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position);
            _nickNamePos.position = screenPos + (Vector3)playerNameOffset;  // Vector3にキャストして加算
        }

        void ApplyNickName(string PlayerName)
        {
            if (PlayerName == null) _nickName.text = "NoName";
            _nickName.text = PlayerName;
        }

    }
}
