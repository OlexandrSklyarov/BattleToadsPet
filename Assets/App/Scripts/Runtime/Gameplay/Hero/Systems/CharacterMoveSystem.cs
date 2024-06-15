using System;
using System.Runtime.InteropServices;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterEngineComponent> _characterEnginePool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<CharacterConfigComponent> _configDataPool;
        private EcsPool<InputDataComponent> _inputDataPoool;
        private EcsPool<CharacterCheckGroundComponent> _groundDataPoool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterEngineComponent>()
                .Inc<MovementDataComponent>()
                .Inc<CharacterConfigComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterCheckGroundComponent>()
                .End();

            _characterEnginePool = world.GetPool<CharacterEngineComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _configDataPool = world.GetPool<CharacterConfigComponent>();
            _inputDataPoool = world.GetPool<InputDataComponent>();
            _groundDataPoool = world.GetPool<CharacterCheckGroundComponent>();
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

                    movement.Velocity = Vector3.Lerp(movement.Velocity, targetVelocity, acceleration * Time.fixedDeltaTime);
                }
                else //stopping
                {
                    movement.Velocity = Vector3.Lerp(movement.Velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
                }
                
                engine.ControllerRef.RB.velocity = new Vector3(movement.Velocity.x, movement.VerticalVelocity, movement.Velocity.z);
                Debug.Log($"engine.ControllerRef.RB.velocity {engine.ControllerRef.RB.velocity} | acceleration {acceleration} | deceleration {deceleration} | movement.Velocity {movement.Velocity}");
            }
        }
    }
}
