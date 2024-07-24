using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterGravitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;
        private EcsPool<CharacterVelocityComponent> _velocityPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .Inc<CharacterVelocityComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();            
            _inputDataPool = world.GetPool<InputDataComponent>(); 
            _velocityPool = world.GetPool<CharacterVelocityComponent>();

        }        

        public void Run(IEcsSystems systems)
        {            
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var config = ref _configPool.Get(e);   
                ref var input = ref _inputDataPool.Get(e);   
                ref var velocity = ref _velocityPool.Get(e);   

                movement.IsFalling = velocity.Vertical <= 0f || !input.IsJumpHold;

                if (movement.IsGround) // ground
                {
                    velocity.Vertical = config.ConfigRef.Gravity.GroundGravity;
                    movement.FallTime = 0f;
                }                
                else if (movement.IsFalling) //fall
                {
                    var previousVelocity = velocity.Vertical;
                    var newVelocity = velocity.Vertical + (movement.Gravity * config.ConfigRef.Engine.FallMultiplier * Time.deltaTime);
                    var nextVelocity = (previousVelocity + newVelocity) * 0.5f;
                    velocity.Vertical = nextVelocity;
                    movement.FallTime += Time.deltaTime;
                }
                else // jump
                {
                    var previousVelocity = velocity.Vertical;
                    var newVelocity = velocity.Vertical + (movement.Gravity * Time.deltaTime);
                    var nextVelocity = (previousVelocity + newVelocity) * 0.5f;
                    velocity.Vertical = nextVelocity;
                } 

                velocity.Vertical = Mathf.Clamp
                (
                    velocity.Vertical, 
                    -config.ConfigRef.Engine.MaxFallSpeed, 
                    config.ConfigRef.Engine.MaxUpSpeed
                );
            }
        }
    }
}

