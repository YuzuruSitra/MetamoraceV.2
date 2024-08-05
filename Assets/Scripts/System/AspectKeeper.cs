using UnityEngine;
using UnityEngine.Serialization;

namespace System
{
    [ExecuteAlways]
    public class AspectKeeper : MonoBehaviour
    {
        private Camera _targetCamera; //対象とするカメラ
 
        [SerializeField] private Vector2 _aspectVec; //目的解像度
        private Rect _viewportRect = new Rect(0, 0, 1, 1);
        private void Start()
        {
            _targetCamera = GetComponent<Camera>();
        }

        private void Update()
        {
            var screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
            var targetAspect = _aspectVec.x / _aspectVec.y; //目的のアスペクト比
 
            var magRate = targetAspect / screenAspect; //目的アスペクト比にするための倍率
            
            if (magRate < 1)
            {
                _viewportRect.width = magRate; //使用する横幅を変更
                _viewportRect.x = 0.5f - _viewportRect.width * 0.5f;//中央寄せ
            }
            else
            {
                _viewportRect.height = 1 / magRate; //使用する縦幅を変更
                _viewportRect.y = 0.5f - _viewportRect.height * 0.5f;//中央余生
            }
 
            _targetCamera.rect = _viewportRect; //カメラのViewportに適用
        }
    }
}