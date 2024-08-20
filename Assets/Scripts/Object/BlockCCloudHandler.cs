using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Object
{
    public class BlockCCloudHandler : MonoBehaviour
    {
        private Image _image;
        public Transform _targetObject;
        private static readonly Vector3 InitialScale = new(0.01f, 0.01f, 0.01f);
        private static readonly Vector3 MaxScale = new(25.0f, 25.0f, 25.0f);
        private Coroutine _coroutine;
        
        public void LaunchCloud(Transform targetObject, float transitionTime, float waitTime)
        {
            var mainCamera = Camera.main;
            var screenPosition = mainCamera.WorldToScreenPoint(targetObject.position);
            transform.position = screenPosition;
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(CloudAnim(transitionTime, waitTime));
        }

        private IEnumerator CloudAnim(float transitionTime, float waitTime)
        {
            _image.enabled = true;
            
            var elapsedTime = 0f;
            while (elapsedTime < transitionTime)
            {
                var scale = Vector3.Slerp(InitialScale, MaxScale, elapsedTime / transitionTime);
                transform.localScale = scale;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = MaxScale;
            elapsedTime = 0f;
            while (elapsedTime < waitTime)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            elapsedTime = 0f;
            while (elapsedTime < transitionTime)
            {
                var scale = Vector3.Slerp(MaxScale, InitialScale, elapsedTime / transitionTime);
                transform.localScale = scale;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.localScale = InitialScale;
            _image.enabled = false;
            _coroutine = null;
        }
        
    }
}
