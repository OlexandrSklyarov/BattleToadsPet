using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/Combo/ComboItem", fileName = "ComboItem")]
    public sealed class ComboItem : ScriptableObject
    {        
        [field: SerializeField] public AnimatorOverrideController AnimatorController {get; private set;}
        [field: SerializeField, Min(1f)] public float AnimationSpeed {get; private set;} = 1f;
        [field: SerializeField, Min(1f)] public float Damage {get; private set;} = 5f;
    }
}