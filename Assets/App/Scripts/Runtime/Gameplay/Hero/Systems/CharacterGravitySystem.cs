using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class CharacterGravitySystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();            
        }        

        public void Run(IEcsSystems systems)
        {            
            foreach(var e in _filter)
            {
                ref var movement = ref _movementDataPool.Get(e);
                ref var config = ref _configPool.Get(e);   

                if (movement.IsGround)
                {
                    movement.VerticalVelocity = config.ConfigRef.Gravity.GroundGravity;
                }
                else
                {
                    var previousVelocity = movement.VerticalVelocity;
                    var newVelocity = movement.VerticalVelocity + (movement.Gravity * Time.deltaTime);
                    var nextVelocity = (previousVelocity + newVelocity) * 0.5f;
                    movement.VerticalVelocity = nextVelocity;

                    DebugUtil.Print($"Jump movement.VerticalVelocity {movement.VerticalVelocity }");
                }              
            }
        }
    }
}

