
namespace BT.Runtime.Gameplay.Hero.Components
{
    public struct CharacterAttackComponent
    {
        public float LastAttackTime;
        public float LastAttackEnd;
        public int ComboIndex;
        public bool IsCanStartPowerAttack;
        public bool IsExecuted;
        public bool IsExecutedPower;
    }
}
