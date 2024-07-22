using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/AttackConfig", fileName = "AttackConfig")]
    public sealed class AttackConfig : ScriptableObject
    {        
        [field: SerializeField, Min(0.001f)] public float Delay {get; private set;} = 0.5f;
    }
}