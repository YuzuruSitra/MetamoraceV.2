using UnityEngine;
using UnityEngine.UI;

public class UITargetFollower : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private Transform target;

	[SerializeField] private float playerNameOffsetY;
    private Vector2 playerNameOffset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
		rectTransform.position = screenPos + (Vector3)playerNameOffset;  // Vector3にキャストして加算
    }
}