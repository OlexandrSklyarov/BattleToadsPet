using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Enemy.Components
{
    public struct AttackState
    {
        public EcsPackedEntity Target;
        public float NextAttackDelay;
    }
}
