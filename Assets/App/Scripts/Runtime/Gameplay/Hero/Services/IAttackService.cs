using BT.Runtime.Gameplay.Hero.View.Animations;

namespace BT.Runtime.Gameplay.Hero.Services
{
    public interface IAttackService
    {
        int GetAttackAnimID(AttackType type);
        void ResetAttackExecuted();
        void ApllyAttack(AttackType type);
        bool IsAttackExecuted();
    }
}
