using UnityEngine;

namespace BT.Runtime.Data
{
    public static class GameConstants
    {
        public static class AnimatorPrm
        {
            public static readonly int NORM_SPEED_PRM = Animator.StringToHash("NormSpeed");
            public static readonly int MOVEMENT = Animator.StringToHash("Movement");
            public static readonly int STAND_UP = Animator.StringToHash("StandUp");
            public static readonly int FALL = Animator.StringToHash("Fall");
            public static readonly int LANDING = Animator.StringToHash("Landing");
            public static readonly int ATTACK = Animator.StringToHash("Attack");
            public static readonly int DEATH = Animator.StringToHash("Death");
            public static readonly int HIT = Animator.StringToHash("Hit");
            public static readonly int HIT_TYPE_PRM = Animator.StringToHash("HitTypePrm");
            public static readonly int ATTACK_SPEED_PRM = Animator.StringToHash("AttackSpeed");
            public static readonly int ATTACK_1 = Animator.StringToHash("ATTACK_1");
            public static readonly int ATTACK_2 = Animator.StringToHash("ATTACK_2");
            public static readonly int ATTACK_3 = Animator.StringToHash("ATTACK_3");
            public static readonly int POWER_ATTACK_1 = Animator.StringToHash("POWER_ATTACK_1");
            public static readonly int POWER_ATTACK_2 = Animator.StringToHash("POWER_ATTACK_2");
        }

        public static class Scene
        {
            public static readonly string BOOT = "Boot";            
            public static readonly string MEDIATOR_LEVEL_BOOT = "MediatorLevelBoot";            
        }
    }
}