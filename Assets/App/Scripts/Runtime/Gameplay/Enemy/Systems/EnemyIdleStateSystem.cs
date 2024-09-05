using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemyIdleStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _sharedData;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<IdleState> _idleStatePool;
        private EcsPool<ChaseTargetState> _chaseStatePool;
        private EcsPool<TranslateComponent> _translatePool;

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>();

            _world = systems.GetWorld();

            _filter = _world.Filter<EnemyComponent>()
                .Inc<IdleState>()
                .Inc<TranslateComponent>()
                .End();

            _enemyPool = _world.GetPool<EnemyComponent>();
            _translatePool = _world.GetPool<TranslateComponent>();
            _idleStatePool = _world.GetPool<IdleState>();
            _chaseStatePool = _world.GetPool<ChaseTargetState>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!_sharedData.HeroEntity.Unpack(_world, out int heroEntity)) return;

            ref var heroTr =  ref _translatePool.Get(heroEntity);

            foreach(var ent in _filter)
            {
                ref var enemy = ref _enemyPool.Get(ent);
                ref var myTr = ref _translatePool.Get(ent);

                if ((myTr.TrRef.position - heroTr.TrRef.position).sqrMagnitude <= enemy.TriggerDistance * enemy.TriggerDistance)
                {
                    if ((myTr.TrRef.position - heroTr.TrRef.position).sqrMagnitude <= enemy.AttackDistance * enemy.AttackDistance)
                    {
                        //try attack
                    }
                    else 
                    {
                        //chase
                        _idleStatePool.Del(ent);

                        ref var chaseState = ref _chaseStatePool.Add(ent);
                        chaseState.Target = _sharedData.HeroEntity;                        
                    }                                        
                }
            }
        }
    }
}
