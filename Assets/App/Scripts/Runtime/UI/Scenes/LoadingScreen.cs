using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BT.Runtime.UI.Scenes
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _screen;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _operationInfo;

        private float _targetProgress;
        private float _curProgress;
        private float _smoothVelocity;

        private const float DURATION = 0.5f;
        private const float FADE_DURATION = 0.5f;

        public void Show() 
        {
            _screen.blocksRaycasts = true;
            _curProgress = _targetProgress = 0f;

            StartCoroutine(UpdateProgressBarAnimation());
        }

        public void Hide() 
        {
            _screen.blocksRaycasts = false;
            _curProgress = 1f;
        }

        public void SetProgress(float progress, string operation)
        {
            _targetProgress = progress;
            _operationInfo.text = $"Loading {operation}";
        }

        private IEnumerator UpdateProgressBarAnimation()
        {
            while(_curProgress < 1f)
            {
                if(_screen.alpha < 1f)
                {
                    _screen.alpha += Time.deltaTime / FADE_DURATION;
                }

                _curProgress = Mathf.SmoothDamp(_curProgress, _targetProgress, ref _smoothVelocity, DURATION);
                _progressBar.value = _curProgress;

                yield return null;
            }

            _progressBar.value = 1f;

            yield return new WaitForSeconds(DURATION);
            
            while(_screen.alpha > 0f)
            {
                _screen.alpha -= Time.deltaTime / FADE_DURATION;

                yield return null;
            }

            Debug.Log("Loading completed");
        }        
    }
}