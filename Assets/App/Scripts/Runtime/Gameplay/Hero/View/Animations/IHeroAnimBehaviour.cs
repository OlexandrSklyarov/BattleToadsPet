using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Hero.View.Animation
{
    public interface IHeroAnimBehaviour
    {
        void Init(EcsPackedEntity packedEntity, EcsWorld world);
    }
}
