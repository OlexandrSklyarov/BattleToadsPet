using System.Collections.Generic;
using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Combat.Systems
{
    public sealed class AttackRequestHandleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Dictionary<Collider, EcsPackedEntity> _entityColliders;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<AttackRequestComponent> _attackRequestPool;
        private Collider[] _colliderResult;

        public void Init(IEcsSystems systems)
        {
            _entityColliders = systems.GetShared<SharedData>().EntityColliders;
            _world = systems.GetWorld();

            _filter = _world.Filter<AttackRequestComponent>().End();
            _attackRequestPool = _world.GetPool<AttackRequestComponent>();
            _colliderResult = new Collider[32];
        }

        public void Run(IEcsSystems systems)
        {
            foreach(var ent in _filter)
            {
                ref var request = ref _attackRequestPool.Get(ent);

                ApplyDamageTargets(ref request, ent);

                _attackRequestPool.Del(ent);
            }
        }

        private void ApplyDamageTargets(ref AttackRequestComponent request, int attackEntity)
        {
            var findCount = Physics.OverlapSphereNonAlloc
            (
                request.Position,
                request.Radius,
                _colliderResult
            );

            for (int i = 0; i < findCount; i++)
            {
                var col = _colliderResult[i];

                if (col == null) continue;
                
                DebugUtil.Print($"find collider {col.name}");

                if (!_entityColliders.TryGetValue(col, out EcsPackedEntity target)) continue;
                if (!target.Unpack(_world, out int damageEntity)) continue;

                ApplyDamage(ref request, attackEntity, damageEntity);                
            }
        }

        private void ApplyDamage(ref AttackRequestComponent request, int attackEntity, int damageEntity)
        {
            if (attackEntity == damageEntity) return;
                
            //apply damage... 
            DebugUtil.Print($"apply damage... ent{damageEntity}");           
        }
    }
}
