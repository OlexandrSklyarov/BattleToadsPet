using Cinemachine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;

namespace BT.Runtime.Gameplay.Views.Camera
{
    public class CameraController : MonoBehaviour, ICameraController
    {
        #region Data
        private enum CameraType {Follow, Overview}
        #endregion

        [SerializeField] private CinemachineVirtualCamera _followCamera;
        [SerializeField] private CinemachineVirtualCamera _overviewCamera;

        private CinemachineBasicMultiChannelPerlin _perlin;

        private void Awake() 
        {
            _perlin = _followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();     
        }

        public void FollowTarget(ICameraTarget target)
        {
            _followCamera.Follow = target.TR;
            _followCamera.LookAt = target.TR;
            ActiveCamera(CameraType.Follow);
        }

        public void OverviewTarget(Transform target)
        {
            _overviewCamera.Follow = target;
            _overviewCamera.LookAt = target;
            ActiveCamera(CameraType.Overview);
        }

        private void ActiveCamera(CameraType type)
        {
            _followCamera.Priority = (type == CameraType.Follow) ? 10 : 0;
            _overviewCamera.Priority = (type == CameraType.Overview) ? 10 : 0;
        }

        [Button]
        private void TestShake() => ShakeAsync().Forget();

        public async UniTaskVoid ShakeAsync()
        {
            _perlin.m_AmplitudeGain = 0.25f;
            _perlin.m_FrequencyGain = 3f;

            await Awaitable.WaitForSecondsAsync(0.4f);

            _perlin.m_AmplitudeGain = 0f;
            _perlin.m_FrequencyGain = 0f;
        }
    }
}
