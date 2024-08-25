using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterAttackSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;
        private EcsPool<CharacterAttackComponent> _attackDataPool;
        private EcsPool<CharacterVelocityComponent> _velovityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<CharacterVelocityComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
            _attackDataPool = world.GetPool<CharacterAttackComponent>();
            _velovityPool = world.GetPool<CharacterVelocityComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref _movementDataPool.Get(ent);
                
                ref var input = ref _inputDataPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var attack = ref _attackDataPool.Get(ent);
                ref var velocity = ref _velovityPool.Get(ent);                

                if (movement.IsGround)
                {
                    if (input.IsAttackWasPressed && !attack.IsExecuted && !attack.IsExecutedPower)
                    {                        
                        attack.AttackTimeout = config.ConfigRef.Attack.Delay;

                        if (attack.IsCanStartPowerAttack)
                        {
                            attack.IsExecutedPower = true;
                            attack.IsCanStartPowerAttack = false;
                        }
                        else
                        {
                            attack.IsExecuted = true;
                        }
                    }
                }
                else
                {
                    //in air attack
                }

                ResetAttackTime(ref attack);
            }
        }

        private void ResetAttackTime(ref CharacterAttackComponent attack)
        {
            if (attack.AttackTimeout > 0f)
            {
                attack.AttackTimeout -= Time.deltaTime;
            }
            else
            {
                attack.AttackTimeout = 0f;
            }
        }        
    }
}
