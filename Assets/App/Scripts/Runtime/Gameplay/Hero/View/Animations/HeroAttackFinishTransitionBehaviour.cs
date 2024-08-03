using BT.Runtime.Gameplay.Hero.Services.Attack;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.View.Animations
{
    public class HeroAttackFinishTransitionBehaviour : StateMachineBehaviour, IHeroAnimBehaviour
    {
        [SerializeField] private AttackPointType _pointType;
        
        private IAttackService _attackService;
        private bool _isInit;

        public void Init(IAttackService attackService)
        {
            _attackService = attackService;
            _isInit = true;
        }

        private void TryRegisterInView(Animator animator)
        {
            if (_isInit) return;

            var view = animator.GetComponentInParent<HeroView>();

            if (view != null) view.RegisterAnimBehaviour(this);
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
           TryRegisterInView(animator);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        // {

        // }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _attackService.ResetAttackExecuted();
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}
