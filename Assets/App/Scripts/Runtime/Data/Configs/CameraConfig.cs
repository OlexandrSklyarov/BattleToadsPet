using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/CameraConfig", fileName = "CameraConfig")]
    public sealed class CameraConfig : ScriptableObject
    {        
        [field: SerializeField] public Vector3 FollowOffset {get; private set;}      
        [field: SerializeField] public Vector3 AimOffset {get; private set;}      
        [field: SerializeField] public Vector3 BodyDamping {get; private set;}      
        [field: SerializeField, Min(1)] public int FOV {get; private set;} = 60;   
        [field: Space, SerializeField] public bool IsChangeInRuntime {get; private set;}
    }
}