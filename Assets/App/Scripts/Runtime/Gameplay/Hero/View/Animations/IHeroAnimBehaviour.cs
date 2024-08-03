using BT.Runtime.Gameplay.Hero.Services.Attack;

namespace BT.Runtime.Gameplay.Hero.View.Animations
{
    public interface IHeroAnimBehaviour
    {
        void Init(IAttackService attackService);
    }
}
