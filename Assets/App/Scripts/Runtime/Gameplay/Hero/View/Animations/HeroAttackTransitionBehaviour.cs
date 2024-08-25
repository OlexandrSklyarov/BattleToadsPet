using BT.Runtime.Gameplay.Hero.Services.Attack;
using BT.Runtime.Gameplay.Views.Hero;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Hero.View.Animations
{
    public class HeroAttackTransitionBehaviour : StateMachineBehaviour, IHeroAnimBehaviour
    {
        [SerializeField] private AttackType _type;
        [SerializeField] private AttackPointType _pointType;
        
        private IAttackService _attackService;
        private bool _isInit;
        private bool _isActiveAttackState;

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
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_isActiveAttackState && _attackService.IsAttackExecuted()) 
            {
                _isActiveAttackState = true;
                animator.CrossFade(_attackService.GetAttackAnimID(_type), _attackService.GetCrossFadeTime());
                _attackService.ApllyAttack(_type, _pointType);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {            
            _isActiveAttackState = false;         
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
