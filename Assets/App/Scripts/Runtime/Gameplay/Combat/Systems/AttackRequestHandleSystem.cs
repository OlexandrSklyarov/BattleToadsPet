using System.Collections.Generic;
using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Combat.Services;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using Util;

namespace BT.Runtime.Gameplay.Combat.Systems
{
    public sealed class AttackRequestHandleSystem : IEcsInitSystem, IEcsRunSystem
    {
        private Dictionary<Collider, EcsPackedEntity> _entityColliders;
        private DetectTargetService _detectTargetService;
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<AttackRequestComponent> _attackRequestPool;
        private Collider[] _colliderResult;

        public void Init(IEcsSystems systems)
        {
            _entityColliders = systems.GetShared<SharedData>().EntityColliders;
            _detectTargetService = systems.GetShared<SharedData>().DetectTargetService;

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

                TryApplyDamageTargets(ref request, ent);

                _attackRequestPool.Del(ent);
            }
        }

        private void TryApplyDamageTargets(ref AttackRequestComponent request, int attackEntity)
        {
            var colliders = _detectTargetService.FindCollidersInRadius(request.Position, request.Radius);

            foreach(var col in colliders)
            {
                DebugUtil.Print($"find collider {col.name}");

                if (!_entityColliders.TryGetValue(col, out EcsPackedEntity target)) continue;
                if (!target.Unpack(_world, out int damageEntity)) continue;

                ApplyDamage(ref request, attackEntity, damageEntity);                
            }
        }

        private void ApplyDamage(ref AttackRequestComponent request, int attackEntity, int damageEntity)
        {
            //exclude self
            if (attackEntity == damageEntity) return;
                
            //apply damage... 
            DebugUtil.Print($"apply damage... ent{damageEntity}");           
        }
    }
}
