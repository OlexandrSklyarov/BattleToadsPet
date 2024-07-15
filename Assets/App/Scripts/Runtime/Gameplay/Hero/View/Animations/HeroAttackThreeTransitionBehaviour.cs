using BT.Runtime.Gameplay.Extensions;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Views.Hero;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.View.Animation
{
    public class HeroAttackThreeTransitionBehaviour : StateMachineBehaviour, IHeroAnimBehaviour
    {
        private EcsPackedEntity _heroPackedEntity;
        private EcsWorld _world;
        private bool _isInit;

        public void Init(EcsPackedEntity packedEntity, EcsWorld world)
        {
            _heroPackedEntity = packedEntity;
            _world = world;
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
           if (_heroPackedEntity.Unpack(_world, out int entity))
            {
                ref var attack = ref _world.GetComponent<CharacterAttackComponent>(entity);
                attack.IsExecuted = false; 
                attack.IsExecutedPower = false;                
            }
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
