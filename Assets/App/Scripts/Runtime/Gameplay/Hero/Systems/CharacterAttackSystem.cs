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
        private EcsPool<AnimatorComponent> _animatorPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterAttackComponent>()
                .Inc<AnimatorComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
            _attackDataPool = world.GetPool<CharacterAttackComponent>();
            _animatorPool = world.GetPool<AnimatorComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref _movementDataPool.Get(ent);
                ref var input = ref _inputDataPool.Get(ent);
                ref var config = ref _configPool.Get(ent);
                ref var attack = ref _attackDataPool.Get(ent);
                ref var animator = ref _animatorPool.Get(ent);
                
                ResetExecuteAttack(ref attack);

                if (movement.IsGround)
                {
                    if (input.IsAttack)
                    {                        
                        if (attack.IsCanStartPowerAttack)
                        {
                            attack.IsExecutedPower = true;
                            attack.IsCanStartPowerAttack = false;
                            attack.ComboIndex = 0;
                        }
                        else
                        {
                            attack.ComboIndex++;
                            attack.ComboIndex %= config.ConfigRef.Attack.Combos.Length;
                            attack.IsExecuted = true;
                            attack.LastAttackTime = config.ConfigRef.Attack.SwitchComboAttackTimer;
                            Debug.Log($"attack.ComboIndex {attack.ComboIndex}");
                        }
                    }
                }
                else
                {
                    //in air attack
                }

                ResetCombo(ref attack);
            }
        }

        private void ResetExecuteAttack(ref CharacterAttackComponent attack)
        {
            attack.IsExecuted = false;
            attack.IsExecutedPower = false;
        }

        private void ResetCombo(ref CharacterAttackComponent attack)
        {
            if (attack.LastAttackTime > 0f)
            {
                attack.LastAttackTime -= Time.deltaTime;
            }
            else 
            {
                attack.ComboIndex = -1;
            }
        }
    }
}