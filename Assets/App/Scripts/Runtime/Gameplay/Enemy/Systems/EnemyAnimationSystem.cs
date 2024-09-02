using BT.Runtime.Data;
using BT.Runtime.Gameplay.Enemy.Components;
using BT.Runtime.Gameplay.General.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace BT.Runtime.Gameplay.Enemy.Systems
{
    public sealed class EnemyAnimationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<EnemyComponent> _enemyPool;
        private EcsPool<AnimatorComponent> _animatorPool;
        private EcsPool<NavMeshCharacterEngine> _navMeshEnginePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world.Filter<EnemyComponent>()
                .Inc<AnimatorComponent>()
                .Inc<NavMeshCharacterEngine>()
                .End();

            _enemyPool = _world.GetPool<EnemyComponent>();    
            _animatorPool = _world.GetPool<AnimatorComponent>();    
            _navMeshEnginePool = _world.GetPool<NavMeshCharacterEngine>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                ref var enemy = ref _enemyPool.Get(ent); 
                ref var anim = ref _animatorPool.Get(ent); 
                ref var engine = ref _navMeshEnginePool.Get(ent); 
 
                var speed = Mathf.Clamp01(engine.AgentRef.velocity.magnitude);
                anim.AnimatorRef.SetFloat(GameConstants.AnimatorPrm.NORM_SPEED_PRM, speed);

                if (enemy.IsMeeleAttackTrigger)
                {
                    anim.AnimatorRef.CrossFade(GameConstants.AnimatorPrm.ATTACK, 0.1f);
                }
            }
        }
    }
}
