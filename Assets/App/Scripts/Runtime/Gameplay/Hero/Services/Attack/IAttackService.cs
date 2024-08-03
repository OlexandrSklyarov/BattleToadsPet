using BT.Runtime.Gameplay.Hero.View.Animations;

namespace BT.Runtime.Gameplay.Hero.Services.Attack
{
    public interface IAttackService
    {
        int GetAttackAnimID(AttackType type);
        void ResetAttackExecuted();
        void ApllyAttack(AttackType type, AttackPointType _pointType);
        bool IsAttackExecuted();
    }
}
