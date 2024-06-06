using UnityEngine;

namespace BT.Runtime.Data.Configs
{
    [CreateAssetMenu(menuName = "SO/Configs/Character/AttackConfig", fileName = "AttackConfig")]
    public sealed class AttackConfig : ScriptableObject
    {        
        [field: SerializeField, Min(0.001f)] public float SwitchComboAttackTimer {get; private set;} = 0.5f;
        [field: SerializeField, Min(0.001f)] public float MinAttackAnimationProgress {get; private set;} = 0.6f;
        [field: Space(20), SerializeField] public ComboItem[] Combos {get; private set;}
        [field: Space(20), SerializeField] public ComboItem PowerAttackUp {get; private set;}
        [field: Space(20), SerializeField] public ComboItem PowerAttackDown {get; private set;}
        [field: Space(20), SerializeField] public bool IsCanStartPowerAttackDebug {get; private set;}
    }
}