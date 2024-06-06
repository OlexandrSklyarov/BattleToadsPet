using System;
using UnityEngine;

namespace BT.Runtime.Gameplay.Extensions
{
    public static class AnimatorExtensions
    {
        public static  bool IsStateTimeEnd(Animator animator)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f;
        }

        public static  bool IsState(Animator animator, int state)
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            return info.shortNameHash == state;
        }

        public static bool IsStateTimeProgressHasReached(Animator animator, float normProgress)
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= normProgress;
        }
    }
}
