using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Runtime.Gameplay.UI.Elements;
using TMPro;
using UnityEngine;

namespace Game.Runtime.Services.LoadingOperations.View
{
    public class LoadingScreenView : MonoBehaviour
    {
        public static LoadingScreenView Instance { get; private set; }
        
        public bool IsCompleted => _currentLoadingProgress >= 1f;
    
        [SerializeField] private Canvas _loadingCanvas;
        [SerializeField] private CanvasGroup _root;
        [SerializeField] private UIProgressBar _loadingProgressSlider;
        [SerializeField] private TextMeshProUGUI _loadingInfo;

        private float _currentLoadingProgress;

        private const float BAR_SPEED = 0.95f;
        private const float WAIT_COMPLETED_TIME = 1f;
        private const float HIDE_DURATION = 0.5f;
    

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    

        public async UniTask LoadAsync(Queue<ILoadingOperation> operations)
        {
            _root.alpha = 1f;
            _root.blocksRaycasts = true;

            _loadingProgressSlider.SetProgressImmediatly(0f);
            _loadingCanvas.enabled = true;

            StartCoroutine(UpdateProgressBar());

            foreach (var operation in operations)
            {
                await operation.Load(OnProgressChange);
            }

            await WaitForProgressBarFill();

            while(_root.alpha > 0f)
            {
                _root.alpha -= Time.deltaTime / HIDE_DURATION;
                await UniTask.Yield();
            }

            _root.blocksRaycasts = false;
            
            if (_loadingCanvas == null) return;

            _loadingCanvas.enabled = false;
        }

    
        private async UniTask WaitForProgressBarFill()
        {
            if (_loadingProgressSlider.FillProgress < _currentLoadingProgress)
            {
                await UniTask.WaitForSeconds(1f);
            }

            _loadingProgressSlider.SetProgressImmediatly(1f);
            SetPercentText(1f);

            await UniTask.WaitForSeconds(WAIT_COMPLETED_TIME);
        }

        private void OnProgressChange(float progress) => _currentLoadingProgress = progress;


        private IEnumerator UpdateProgressBar()
        {
            while (_loadingCanvas.enabled)
            {
                if (_loadingProgressSlider.FillProgress < _currentLoadingProgress)
                {
                    var progress = _loadingProgressSlider.FillProgress + Time.deltaTime * BAR_SPEED;
                    _loadingProgressSlider.SetProgressImmediatly(progress);
                    SetPercentText(progress);
                }
                yield return null;
            }
        }

        private void SetPercentText(float progress)
        {
            progress = Mathf.Clamp01(progress);
            var percentage = Mathf.RoundToInt(progress * 100f);
            _loadingInfo.text = $"{percentage} %";
        }
    }
}
