using UnityEngine;

namespace BT.Runtime.Data
{
    public static class GameConstants
    {
        public static class AnimatorPrm
        {
            public static readonly int NORM_SPEED_PRM = Animator.StringToHash("NormSpeed");
            public static readonly int ATTACK_SPEED_PRM = Animator.StringToHash("AttackSpeed");
            public static readonly int MOVEMENT = Animator.StringToHash("Movement");
            public static readonly int STAND_UP = Animator.StringToHash("StandUp");
            public static readonly int ATTACK = Animator.StringToHash("Attack");
            public static readonly int JUMP_START = Animator.StringToHash("JumpStart");
            public static readonly int JUMP = Animator.StringToHash("Jump");
            public static readonly int FALL = Animator.StringToHash("Fall");
            public static readonly int JUMP_LANDING = Animator.StringToHash("JumpLanding");
        }
    }
}