using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BT.Runtime.UI.Scenes
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _screen;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _operationInfo;

        public void Show() => _screen.gameObject.SetActive(true);

        public void Hide() 
        {
            _screen.gameObject.SetActive(false);
            Destroy(this.gameObject, 1f);
        }

        public void UpdateProgress(float progress, string operation)
        {
            _progressBar.value = progress;
            _operationInfo.text = $"Loading {operation}... [{Mathf.RoundToInt(100 * progress)}]";
        }
    }
}