using System.Collections.Generic;
using BT.Runtime.Gameplay.Combat.Components;
using BT.Runtime.Gameplay.Combat.Services;
using BT.Runtime.Gameplay.General.Components;
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
        private EcsFilter _hpFilter;
        private EcsPool<AttackRequest> _attackRequestPool;
        private EcsPool<DamageRequest> _damageRequestPool;
        private Collider[] _colliderResult;

        public void Init(IEcsSystems systems)
        {
            _entityColliders = systems.GetShared<SharedData>().EntityColliders;
            _detectTargetService = systems.GetShared<SharedData>().DetectTargetService;

            _world = systems.GetWorld();

            _filter = _world.Filter<AttackRequest>().End();
            _hpFilter = _world.Filter<HealthComponent>().End();

            _attackRequestPool = _world.GetPool<AttackRequest>();
            _damageRequestPool = _world.GetPool<DamageRequest>();

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

        private void TryApplyDamageTargets(ref AttackRequest request, int attackEntity)
        {
            var colliders = _detectTargetService.FindCollidersInRadius(request.Position, request.Radius);

            foreach(var col in colliders)
            {
                DebugUtil.Print($"find collider {col.name}");

                if (!_entityColliders.TryGetValue(col, out EcsPackedEntity target)) continue;
                if (!target.Unpack(_world, out int targetEntity)) continue;

                ApplyDamage(ref request, attackEntity, targetEntity, target);                
            }
        }

        private void ApplyDamage(ref AttackRequest request, int attackEntity, int targetEntity, EcsPackedEntity packedTargetEntity)
        {
            //exclude self
            if (attackEntity == targetEntity) return;
                
            foreach(var ent in _hpFilter)
            {
                if (ent != targetEntity) continue;

                ref var damageRequest = ref _damageRequestPool.Add(_world.NewEntity());                
                damageRequest.Source = _world.PackEntity(attackEntity);
                damageRequest.Target = packedTargetEntity;
                damageRequest.Damage = request.Damage;
            }        
        }
    }
}
