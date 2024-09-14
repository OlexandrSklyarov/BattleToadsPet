namespace BT.Runtime.Gameplay.General.Systems
{
    using System;
    using System.Runtime.InteropServices;
    using BT.Runtime.Gameplay.Combat.Components;
    using BT.Runtime.Gameplay.Extensions;
    using BT.Runtime.Gameplay.General.Components;
    using Leopotam.EcsLite;

    public sealed class ApplyDamageSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _damageEventFilter;
        private EcsFilter _requestFilter;
        private EcsFilter _hpFilter;
        private EcsPool<DamageRequest> _requestPool;
        private EcsPool<HealthComponent> _healthPool;
        private EcsPool<EntityDamageEvent> _damageEventPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _damageEventFilter = _world.Filter<EntityDamageEvent>().End();
            _requestFilter = _world.Filter<DamageRequest>().End();
            _hpFilter = _world.Filter<HealthComponent>().End();

            _requestPool = _world.GetPool<DamageRequest>();
            _healthPool = _world.GetPool<HealthComponent>();
            _damageEventPool = _world.GetPool<EntityDamageEvent>();
        }

        public void Run(IEcsSystems systems)
        {
            RemoveDamageEvents();
            
            foreach (var requestEnt in _requestFilter)
            {
                ref var request = ref _requestPool.Get(requestEnt); 

                if (request.Target.Unpack(_world, out int curTarget))
                {
                    foreach (var ent in _hpFilter)
                    {
                        if (ent != curTarget) continue;

                        CreateDamageEvent(request.Target);

                        //apply damage
                        ref var hp = ref _healthPool.Get(ent);   
                        hp.Value -= request.Damage;

                        
                        if (hp.Value > 0f) //stun
                        {
                            ref var stun = ref _world.TryAddComponent<StunTimer>(curTarget);
                            stun.Value = 1.5f;
                        }
                        else //kill target
                        {
                            _world.TryAddComponent<KillSelfRequest>(curTarget);
                        }
                    }
                }

                //destroy request entity
                _requestPool.Del(requestEnt);
                
            }            
        }

        private void CreateDamageEvent(EcsPackedEntity damageTarget)
        {
            ref var evt = ref _damageEventPool.Add(_world.NewEntity()); 
            evt.Target = damageTarget;
        }

        private void RemoveDamageEvents()
        {
            foreach (var ent in _damageEventFilter)
            {
                _damageEventPool.Del(ent); 
            }
        }
    }
}

