using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.Extensions;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemyStunStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _sharedData;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<NavMeshCharacterEngine> _navEnginePool;
        private EcsPool<StunTimer> _stunTimerPool;

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>();

            _world = systems.GetWorld();

            _filter = _world.Filter<EnemyComponent>()
                .Inc<StunState>()
                .Inc<NavMeshCharacterEngine>()
                .Inc<StunTimer>()
                .End();

            _enemyPool = _world.GetPool<EnemyComponent>();
            _navEnginePool = _world.GetPool<NavMeshCharacterEngine>();
            _stunTimerPool = _world.GetPool<StunTimer>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var enemy =  ref _enemyPool.Get(ent);
                ref var engine =  ref _navEnginePool.Get(ent);
                ref var timer =  ref _stunTimerPool.Get(ent);

                engine.Ref.enabled = false;

                if (timer.Value > 0f)
                {
                    timer.Value -= Time.deltaTime;
                }
                else
                {
                    //switch state STUN => IDLE
                    _world.TryReplaceComponent<StunState, IdleState>(ent);
                }
            }
        }
    }
}
