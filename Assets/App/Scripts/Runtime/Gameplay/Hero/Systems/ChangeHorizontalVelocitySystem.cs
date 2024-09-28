using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class ChangeHorizontalVelocitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<CharacterConfigComponent> _configDataPool;
        private EcsPool<InputDataComponent> _inputDataPoool;
        private EcsPool<CharacterVelocity> _velocityPool;
        private EcsPool<ViewModelTransform> _viewPool;
        private EcsPool<CharacterAttackComponent> _attackDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterVelocity>()
                .Inc<ViewModelTransform>()
                .Inc<CharacterAttackComponent>()
                .End();

            _movementDataPool = world.GetPool<MovementDataComponent>();
            _configDataPool = world.GetPool<CharacterConfigComponent>();
            _inputDataPoool = world.GetPool<InputDataComponent>();
            _velocityPool = world.GetPool<CharacterVelocity>();
            _viewPool = world.GetPool<ViewModelTransform>();
            _attackDataPool = world.GetPool<CharacterAttackComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref _movementDataPool.Get(ent);
                ref var config = ref _configDataPool.Get(ent);
                ref var input = ref _inputDataPoool.Get(ent);                
                ref var velocity = ref _velocityPool.Get(ent);  
                ref var view = ref _viewPool.Get(ent);       
                ref var attack = ref _attackDataPool.Get(ent);

                var acceleration = (movement.IsGround) ? 
                    config.ConfigRef.Engine.GroundAcceleration :
                    config.ConfigRef.Engine.AirAcceleration;

                var deceleration = (movement.IsGround) ? 
                    config.ConfigRef.Engine.GroundDeceleration :
                    config.ConfigRef.Engine.AirDeceleration;                
                
                if (input.MoveDirection.sqrMagnitude > Mathf.Epsilon) // moving
                {
                    var targetVelocity = (input.IsRunHold) ?
                        new Vector3(input.MoveDirection.x, 0f, input.MoveDirection.y) * config.ConfigRef.Engine.MaxRunSpeed :
                        new Vector3(input.MoveDirection.x, 0f, input.MoveDirection.y) * config.ConfigRef.Engine.MaxWalkSpeed;

                    velocity.Horizontal = Vector3.Lerp(velocity.Horizontal, targetVelocity, acceleration * Time.deltaTime);
                }
                else //stopping
                {
                    velocity.Horizontal = Vector3.Lerp(velocity.Horizontal, Vector3.zero, deceleration * Time.deltaTime);
                    if (velocity.Horizontal.magnitude < 0.1f) velocity.Horizontal = Vector3.zero;
                }
                
                ClampHorVelocityFromAttack(ref attack, ref velocity, ref view);
            }
        }

        private void ClampHorVelocityFromAttack(ref CharacterAttackComponent attack, 
            ref CharacterVelocity velocity, 
            ref ViewModelTransform view)
        {
            if (attack.AttackTimeout > 0f)
            {
                var vel = view.ModelTransformRef.forward * Mathf.Epsilon;
                velocity.Horizontal = new Vector3(vel.x, 0f, vel.z);
            }
        }
    }
}
