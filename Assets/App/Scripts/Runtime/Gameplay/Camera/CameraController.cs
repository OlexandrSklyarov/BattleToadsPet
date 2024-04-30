using System;
using BT.Runtime.Data.Configs;
using Cinemachine;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using VContainer;

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
        private CameraConfig _config;

        [Inject]
        private void Construct(CameraConfig config)
        {
            _config = config;
        }

        private void Awake() 
        {
            _perlin = _followCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();     
        }

        private void Start() 
        {
            SetPresset(_followCamera);
            SetPresset(_overviewCamera);    
        }

        private void Update() 
        {
            if (!_config.IsChangeInRuntime) return;

            SetPresset(_followCamera);
            SetPresset(_overviewCamera);
        }

        private void SetPresset(CinemachineVirtualCamera cam)
        {
            cam.m_Lens.FieldOfView = _config.FOV;
            
            var body = cam.GetCinemachineComponent<CinemachineTransposer>();
            body.m_FollowOffset = _config.FollowOffset;
            body.m_XDamping = _config.BodyDamping.x;
            body.m_YDamping = _config.BodyDamping.y;
            body.m_ZDamping = _config.BodyDamping.z;

            cam.GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset = _config.AimOffset;            
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
