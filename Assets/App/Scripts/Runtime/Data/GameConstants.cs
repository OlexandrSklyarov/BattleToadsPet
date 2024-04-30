using System;
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
            public static readonly int PUNCH_COMBO = Animator.StringToHash("PunchCombo");
            public static readonly int HOOK = Animator.StringToHash("Hook");
            public static readonly int JUMP_START = Animator.StringToHash("JumpStart");
            public static readonly int JUMP_FALL = Animator.StringToHash("JumpFall");
            public static readonly int JUMP_LANDING = Animator.StringToHash("JumpLanding");
        }
    }
}