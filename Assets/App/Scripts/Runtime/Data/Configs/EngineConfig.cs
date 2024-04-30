using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/EngineConfig", fileName = "EngineConfig")]
    public sealed class EngineConfig : ScriptableObject
    {        
        [field: SerializeField, Min(1f)] public float MoveSpeed {get; private set;} = 3f;
        [field: SerializeField, Min(1f)] public float MaxSpeed {get; private set;} = 6f;
        [field: SerializeField, Min(0.01f)] public float SpeedSmoothTime {get; private set;} = 0.2f;
        [field: SerializeField, Min(1f)] public float RotateSpeed {get; private set;} = 800f;
        [field: SerializeField, Min(1f)] public float JumpForce {get; private set;} = 50f;
    }
}