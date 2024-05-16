using BT.Runtime.Gameplay.Hero.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Hero.Systems
{
    public sealed class ChangeSpeedSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<CharacterConfigComponent> _configPool;
        private EcsPool<MovementDataComponent> _movementDataPool;
        private EcsPool<InputDataComponent> _inputDataPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();

            _filter = world.Filter<CharacterConfigComponent>()
                .Inc<MovementDataComponent>()
                .Inc<InputDataComponent>()
                .End();

            _configPool = world.GetPool<CharacterConfigComponent>();
            _movementDataPool = world.GetPool<MovementDataComponent>();
            _inputDataPool = world.GetPool<InputDataComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var movement = ref  _movementDataPool.Get(ent);
                ref var input = ref  _inputDataPool.Get(ent);
                ref var config = ref  _configPool.Get(ent);               
                
                var nextSpeed = (movement.Speed <= 0f) ? 
                    0f :  (movement.IsGround && input.IsRun) ?
                    movement.MaxSpeed : 
                    movement.Speed;

                if (!movement.IsGround) nextSpeed = movement.DesiredSpeed;

                movement.DesiredSpeed = Mathf.SmoothDamp
                (
                    movement.DesiredSpeed, 
                    nextSpeed, 
                    ref movement.MovementSmoothVelocity,
                    config.ConfigRef.Engine.SpeedSmoothTime
                );
            }
        }
    }
}

