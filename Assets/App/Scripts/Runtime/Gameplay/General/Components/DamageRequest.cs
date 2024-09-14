using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.General.Components
{
    public struct DamageRequest
    {
        public EcsPackedEntity Source;
        public EcsPackedEntity Target;
        public float Damage;
    }
}
