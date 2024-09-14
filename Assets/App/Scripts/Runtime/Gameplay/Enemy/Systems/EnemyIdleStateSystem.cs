using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.Extensions;
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
        private EcsPool<Translate> _translatePool;
        private EcsPool<StunTimer> _stunTimerPool;

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>();

            _world = systems.GetWorld();

            _filter = _world.Filter<EnemyComponent>()
                .Inc<IdleState>()
                .Inc<Translate>()
                .End();         

            _enemyPool = _world.GetPool<EnemyComponent>();
            _translatePool = _world.GetPool<Translate>();
            _stunTimerPool = _world.GetPool<StunTimer>();
        }

        public void Run(IEcsSystems systems)
        {
            if (!_sharedData.HeroEntity.Unpack(_world, out int heroEntity)) return;
            ref var heroTr =  ref _translatePool.Get(heroEntity);

            foreach(var ent in _filter)
            {
                if (_stunTimerPool.Has(ent))
                {
                    //switch IDLE => STUN
                    _world.TryReplaceComponent<IdleState, StunState>(ent);
                    continue;
                }

                ref var enemy = ref _enemyPool.Get(ent);
                ref var myTr = ref _translatePool.Get(ent);

                if ((myTr.Ref.position - heroTr.Ref.position).sqrMagnitude <= enemy.TriggerDistance * enemy.TriggerDistance)
                {
                    if ((myTr.Ref.position - heroTr.Ref.position).sqrMagnitude <= enemy.AttackDistance * enemy.AttackDistance)
                    {
                        //try attack
                    }
                    else 
                    {
                        //chase
                        ref var chaseState = ref _world.TryReplaceComponent<IdleState, ChaseTargetState>(ent);
                        chaseState.Target = _sharedData.HeroEntity;                        
                    }                                        
                }
            }
        }
    }
}
