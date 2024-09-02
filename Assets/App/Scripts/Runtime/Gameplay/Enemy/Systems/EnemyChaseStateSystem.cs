using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemyChaseStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _sharedData;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<IdleState> _idleStatePool;
        private EcsPool<ChaseHeroState> _chaseStatePool;
        private EcsPool<AttackState> _attackStatePool;
        private EcsPool<NavMeshCharacterEngine> _navMeshEnginePool;
        private EcsPool<TranslateComponent> _translatePool;

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>();

            _world = systems.GetWorld();

            _filter = _world.Filter<EnemyComponent>()
                .Inc<ChaseHeroState>()
                .Inc<TranslateComponent>()
                .Inc<NavMeshCharacterEngine>()
                .End();

            _enemyPool = _world.GetPool<EnemyComponent>();
            _translatePool = _world.GetPool<TranslateComponent>();
            _idleStatePool = _world.GetPool<IdleState>();
            _chaseStatePool = _world.GetPool<ChaseHeroState>();
            _attackStatePool = _world.GetPool<AttackState>();
            _navMeshEnginePool = _world.GetPool<NavMeshCharacterEngine>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var enemy = ref _enemyPool.Get(ent);
                ref var navMesh = ref _navMeshEnginePool.Get(ent);
                ref var myTr = ref _translatePool.Get(ent);
                ref var chaseState = ref _chaseStatePool.Get(ent);

                if (!chaseState.Target.Unpack(_world, out int heroEntity)) 
                {
                    //switch state Chase ==> Idle
                    _chaseStatePool.Del(ent);
                    _idleStatePool.Add(ent);
                    continue;
                }

                ref var heroTr = ref _translatePool.Get(heroEntity); 
                

                if ((myTr.TrRef.position - heroTr.TrRef.position).sqrMagnitude > enemy.AttackDistance * enemy.AttackDistance)
                {
                    navMesh.AgentRef.speed = 2f;
                    navMesh.AgentRef.SetDestination(heroTr.TrRef.position);                                       
                }
                else
                {
                    //switch state Chase ==> Attack
                    var target = chaseState.Target; 

                    _chaseStatePool.Del(ent);

                    navMesh.AgentRef.speed = 0f;
                    ref var attackState = ref _attackStatePool.Add(ent);
                    attackState.Target = target;
                }
            }
        }
    }
}
