using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Runtime.Gameplay.UI.Elements
{
    public class UIProgressBar : BaseUIItem
    {
        public float FillProgress => Mathf.InverseLerp(0f, 1f, _fill.localScale.x);
        
        [SerializeField] private RectTransform _fill;
        [SerializeField] private TextMeshProUGUI _valueText;        

        public void SetProgress(float normValue)
        {
            var value = new Vector3
            (
                Mathf.Clamp01(normValue),
                _fill.localScale.y,
                _fill.localScale.z
            );

            _fill.DOScale(value, 0.25f);
        }

        public async UniTask SetProgressAnimatedPercentAsync(float startNormValue, float normValue, float duration)
        {
            _fill.localScale = new Vector3
            (
                Mathf.Clamp01(startNormValue),
                _fill.localScale.y,
                _fill.localScale.z
            );

            var value = new Vector3
            (
                Mathf.Clamp01(normValue),
                _fill.localScale.y,
                _fill.localScale.z
            );

            var sec = DOTween.Sequence();
            sec.Append(_fill.DOScale(value, duration));
            sec.Join(DOVirtual.Float(0f, normValue, duration, (v) =>
            {
                SetText($"{Mathf.RoundToInt(v * 100)}%");
            }));

            await sec.AsyncWaitForCompletion();
        }

        public void SetText(string value)
        {
            _valueText.text = value;
        }

        public void SetProgressImmediatly(float normValue)
        {
            normValue = Mathf.Clamp(normValue, 0.001f, 1f);

            _fill.localScale = new Vector3
            (
                normValue,
                _fill.localScale.y,
                _fill.localScale.z
            );
        }        
    }
}
