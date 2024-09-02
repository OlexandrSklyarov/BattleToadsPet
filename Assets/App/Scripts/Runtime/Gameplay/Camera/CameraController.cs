using BT.Runtime.Data.Configs;
using Unity.Cinemachine;
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

        [SerializeField] private CinemachineCamera _followCamera;
        [SerializeField] private CinemachineCamera _overviewCamera;

        private CinemachineBasicMultiChannelPerlin _perlin;
        private CameraConfig _config;

        [Inject]
        private void Construct(CameraConfig config)
        {
            _config = config;
        }

        private void Awake() 
        {
            _perlin = _followCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();     
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

        private void SetPresset(CinemachineCamera cam)
        {
            cam.Lens.FieldOfView = _config.FOV;
            
            var body = cam.GetComponent<CinemachineFollow>();
            body.FollowOffset = _config.FollowOffset;
            body.TrackerSettings.PositionDamping.x = _config.BodyDamping.x;
            body.TrackerSettings.PositionDamping.y = _config.BodyDamping.y;
            body.TrackerSettings.PositionDamping.z = _config.BodyDamping.z;

            cam.GetComponent<CinemachineRotationComposer>().TargetOffset = _config.AimOffset;            
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
            _perlin.AmplitudeGain = 0.25f;
            _perlin.FrequencyGain = 3f;

            await Awaitable.WaitForSecondsAsync(0.4f);

            _perlin.AmplitudeGain = 0f;
            _perlin.FrequencyGain = 0f;
        }
    }
}
