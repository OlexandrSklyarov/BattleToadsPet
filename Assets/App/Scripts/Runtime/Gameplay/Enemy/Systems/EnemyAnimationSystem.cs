using BT.Runtime.Data;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.General.Components;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemyAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _damageEventFilter;
        private EcsFilter _filter;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<AnimatorController> _animatorPool;
        private EcsPool<NavMeshCharacterEngine> _navMeshEnginePool;
        private EcsPool<EntityDamageEvent> _damageEventPool;
        private readonly int[] _hitIndexes = new[] {0,1};

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _damageEventFilter = _world.Filter<EntityDamageEvent>().End();
            
            _filter = _world.Filter<EnemyComponent>()
                .Inc<AnimatorController>()
                .Inc<NavMeshCharacterEngine>()
                .End();

            _enemyPool = _world.GetPool<EnemyComponent>();    
            _animatorPool = _world.GetPool<AnimatorController>();    
            _navMeshEnginePool = _world.GetPool<NavMeshCharacterEngine>();
            _damageEventPool = _world.GetPool<EntityDamageEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var enemy = ref _enemyPool.Get(ent);
                ref var anim = ref _animatorPool.Get(ent);
                ref var engine = ref _navMeshEnginePool.Get(ent);

                //speed
                var speed = Mathf.Clamp01(engine.Ref.velocity.magnitude);
                anim.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, speed);
                
                //damage
                if (TryPlayDamage(ent))
                {
                    anim.AnimatorRef.SetInteger(GameConstants.AnimatorPrm.HIT_TYPE_PRM, _hitIndexes.RandomElement());
                    anim.AnimatorRef.CrossFade(GameConstants.AnimatorPrm.HIT, 0.1f);
                    continue;
                }

                //attack
                if (enemy.IsMeeleAttackTrigger)
                {
                    anim.AnimatorRef.CrossFade(GameConstants.AnimatorPrm.ATTACK, 0.1f);
                }
            }
        }

        private bool TryPlayDamage(int curEntity)
        {
            foreach (var evtEnt in _damageEventFilter)
            {
                ref var evt = ref _damageEventPool.Get(evtEnt);

                if (evt.Target.Unpack(_world, out int target) && target == curEntity)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
