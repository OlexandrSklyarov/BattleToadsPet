using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Units/WarrirorConfig", fileName = "WarrirorConfig")]
    public sealed class WarrirorConfig : ScriptableObject
    {
        [field: Space, SerializeField, Min(0.01f)] public float AttackDistance {get; private set;} = 2f;
        [field: Space, SerializeField, Min(0.01f)] public float TriggerDistance {get; private set;} = 10f;
        [field: Space, SerializeField, Min(0.01f)] public float MeeleAttackDelay {get; private set;} = 2f;
    }
}
