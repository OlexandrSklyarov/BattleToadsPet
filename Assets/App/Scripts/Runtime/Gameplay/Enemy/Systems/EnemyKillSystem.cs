namespace BT.Runtime.Gameplay.Enemy.Systems
{
    using System.Collections.Generic;
    using BT.Runtime.Data;
    using BT.Runtime.Gameplay.Components;
    using BT.Runtime.Gameplay.Enemy.Components;
    using BT.Runtime.Gameplay.Enemy.View;
    using BT.Runtime.Gameplay.General.Components;
    using BT.Runtime.Gameplay.Services.GameWorldData;
    using Leopotam.EcsLite;
    using UnityEngine;

    public sealed class EnemyKillSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Dictionary<Collider, EcsPackedEntity> _entityColliders;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<AnimatorController> _animatorPool;
        private EcsPool<NavMeshCharacterEngine> _navMeshEnginePool;
        private EcsPool<Translate> _translatePool;

        public void Init(IEcsSystems systems)
        {
            _entityColliders = systems.GetShared<SharedData>().EntityColliders;

            _world = systems.GetWorld();
            
            _filter = _world.Filter<EnemyComponent>()
                .Inc<KillSelfRequest>()
                .End();

            _animatorPool = _world.GetPool<AnimatorController>();  
            _navMeshEnginePool = _world.GetPool<NavMeshCharacterEngine>();   
            _translatePool = _world.GetPool<Translate>();  
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var ent in _filter)
            {
                //stop engine
                if (_navMeshEnginePool.Has(ent))
                {
                    ref var engine = ref _navMeshEnginePool.Get(ent); 
                    engine.Ref.speed = 0;
                    engine.Ref.enabled = false;
                }

                //death animation
                if (_animatorPool.Has(ent))
                {
                    ref var anim = ref _animatorPool.Get(ent); 
                    anim.AnimatorRef.CrossFade(GameConstants.AnimatorPrm.DEATH, 0.1f);
                }

                //destroy with delay
                if (_translatePool.Has(ent))
                {
                    ref var tr = ref _translatePool.Get(ent);
                    if (tr.Ref.TryGetComponent(out EnemyView view))
                    {
                        RemoveEntityCollider(view);
                        view.ReclaimAsync();
                    }
                }

                _world.DelEntity(ent);
            }
        }

        private void RemoveEntityCollider(EnemyView enemyView)
        {
            if (_entityColliders.ContainsKey(enemyView.Collider))
            {
                _entityColliders.Remove(enemyView.Collider);
            }
        }
    }
}
