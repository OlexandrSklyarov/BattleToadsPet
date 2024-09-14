using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.Extensions;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using Util;
using Debug = Util.Debug;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemyAttackStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private SharedData _sharedData;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<IdleState> _idleStatePool;
        private EcsPool<ChaseTargetState> _chaseStatePool;
        private EcsPool<AttackState> _attackStatePool;
        private EcsPool<NavMeshCharacterEngine> _navMeshEnginePool;
        private EcsPool<ViewModelTransform> _viewModelTrPool;
        private EcsPool<StunTimer> _stunTimerPool;
        private EcsPool<Translate> _translatePool;

        private const float MIN_ANGLE_TO_TARGET = 5;

        public void Init(IEcsSystems systems)
        {
            _sharedData = systems.GetShared<SharedData>();

            _world = systems.GetWorld();

            _filter = _world.Filter<EnemyComponent>()
                .Inc<AttackState>()
                .Inc<Translate>()
                .Inc<NavMeshCharacterEngine>()
                .Inc<ViewModelTransform>()
                .End();

            _enemyPool = _world.GetPool<EnemyComponent>();
            _translatePool = _world.GetPool<Translate>();
            _idleStatePool = _world.GetPool<IdleState>();
            _chaseStatePool = _world.GetPool<ChaseTargetState>();
            _attackStatePool = _world.GetPool<AttackState>();
            _navMeshEnginePool = _world.GetPool<NavMeshCharacterEngine>();
            _viewModelTrPool = _world.GetPool<ViewModelTransform>();
            _stunTimerPool = _world.GetPool<StunTimer>();

        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                if (_stunTimerPool.Has(ent))
                {
                    //switch IDLE => STUN
                    _world.TryReplaceComponent<IdleState, StunState>(ent);
                    continue;
                }

                ref var enemy = ref _enemyPool.Get(ent);
                ref var navMesh = ref _navMeshEnginePool.Get(ent);
                ref var myTr = ref _translatePool.Get(ent);
                ref var attackState = ref _attackStatePool.Get(ent);
                ref var viewModel = ref _viewModelTrPool.Get(ent);

                enemy.IsMeeleAttackTrigger = false;

                if (!attackState.Target.Unpack(_world, out int heroEntity)) 
                {
                    //switch state Attack ==> Idle
                    _world.TryReplaceComponent<AttackState, IdleState>(ent);
                    continue;
                }

                ref var heroTr = ref _translatePool.Get(heroEntity);                 

                if ((myTr.Ref.position - heroTr.Ref.position).sqrMagnitude <= enemy.AttackDistance * enemy.AttackDistance)
                {
                    viewModel.LookAt = Vector3Math.DirToQuaternion(heroTr.Ref.position - myTr.Ref.position);  
                    var angle = Quaternion.Angle(viewModel.LookAt, viewModel.ModelTransformRef.rotation);

                    if (angle < MIN_ANGLE_TO_TARGET && attackState.NextAttackDelay <= 0f)
                    {
                        Debug.Print("Try attack hero!!!");  
                        enemy.IsMeeleAttackTrigger = true;                                   
                        attackState.NextAttackDelay = enemy.MeeleAttackDelay;
                    }

                    attackState.NextAttackDelay -= Time.deltaTime;
                }
                else
                {
                    //switch state Attack ==> Idle
                    _world.TryReplaceComponent<AttackState, IdleState>(ent);
                }
            }
        }
    }
}
