using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class ChangeHorizontalVelocitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterControllerEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<CharacterConfigComponent> _configDataPool;
        private EcsPool<InputDataComponent> _inputDataPoool;
        private EcsPool<CharacterCheckGroundComponent> _groundDataPoool;
        private EcsPool<CharacterVelocityComponent> _velocityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterControllerEngineComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterCheckGroundComponent>()
                .Inc<CharacterVelocityComponent>()
                .End();

            _characterEnginePool = world.GetPool<CharacterControllerEngineComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _configDataPool = world.GetPool<CharacterConfigComponent>();
            _inputDataPoool = world.GetPool<InputDataComponent>();
            _groundDataPoool = world.GetPool<CharacterCheckGroundComponent>();
            _velocityPool = world.GetPool<CharacterVelocityComponent>();

        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var engine = ref _characterEnginePool.Get(ent);
                ref var movement = ref _movementDataPool.Get(ent);
                ref var config = ref _configDataPool.Get(ent);
                ref var input = ref _inputDataPoool.Get(ent);
                ref var ground = ref _groundDataPoool.Get(ent);                
                ref var velocity = ref _velocityPool.Get(ent);                

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
                }
                
                engine.ControllerRef.CC.Move(
                    new Vector3(velocity.Horizontal.x, velocity.Vertical, velocity.Horizontal.z) * Time.deltaTime);
            }
        }
    }
}
