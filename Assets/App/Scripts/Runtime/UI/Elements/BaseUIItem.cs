using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game.Runtime.Gameplay.UI.Elements
{
    public abstract class BaseUIItem : MonoBehaviour
    {
        public void Show() 
        {
            ShowAsync(0.25f).Forget();
        }

        public async UniTask ShowAsync(float duration) 
        {
            gameObject.SetActive(true);

            transform.localScale = Vector3.one * 0.01f;

            var sec = DOTween.Sequence();

            sec.Append(transform.DOScale(1f, duration)
                .SetLink(gameObject)
                .SetEase(Ease.InOutBounce)
                .OnComplete(() => transform.localScale = Vector3.one)
                .OnKill(() => transform.localScale = Vector3.one));

            await sec.AsyncWaitForCompletion();
        }
        

        public void Hide() => gameObject.SetActive(false);
    }
}

