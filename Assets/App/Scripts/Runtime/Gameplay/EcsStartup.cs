using BT.Runtime.Gameplay.Hero.Systems;
using BT.Runtime.Gameplay.Services.GameWorldData;
using Leopotam.EcsLite;
using UnityEngine;
using VContainer;

namespace BT.Runtime.Gameplay 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        private EcsWorld _world;        
        private IEcsSystems _systems;
        private IEcsSystems _fixedSystems;
        private IObjectResolver _resolver;
        private bool _isInitialized;

        [Inject]
        private void Conctruct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public void Init()
        {
            var sharedData = new SharedData()
            {
                DIResolver = _resolver
            };

            _world = new EcsWorld();

            InitUpdateSystems(sharedData);
            InitUpdateFixedSystems(sharedData);

            _isInitialized = true;
        }        

        private void InitUpdateSystems(SharedData sharedData)
        {
            _systems = new EcsSystems(_world, sharedData);
            _systems
                .AddWorld(new EcsWorld(), "events")

                .Add(new SpawnHeroSystem())
                .Add(new CharacterCheckGroundSystem())
                .Add(new CharacterCheckBumpedHeadSystem())
                .Add(new HeroApplyInputSystem())
                .Add(new BodyRotateSystem())
                .Add(new CharacterAttackSystem())
                .Add(new CharacterGravitySystem())
                .Add(new CharacterJumpSystemNew())
                .Add(new CharacterMoveSystem())
                //.Add(new CharacterJumpChecksSystem())
                .Add(new HeroIKFootIKSystem())
                .Add(new HeroAnimationSystem())

        #if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
        #endif
                .Init();
        }

        private void InitUpdateFixedSystems(SharedData sharedData)
        {
            _fixedSystems = new EcsSystems(_world, sharedData);
            _fixedSystems
                .AddWorld(new EcsWorld(), "events")                
                //.Add(new CharacterJumpSystem())

        #if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events"))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
        #endif
                .Init();
        }

        private void Update () 
        {
            if (!_isInitialized) return;

            _systems?.Run ();
        }

        private void FixedUpdate () 
        {
            if (!_isInitialized) return;

            _fixedSystems?.Run ();
        }

        private void OnDestroy () 
        {            
            _systems?.Destroy();
            _systems = null;

            _fixedSystems?.Destroy();
            _fixedSystems = null;
           
            _world?.Destroy();
            _world = null;            
        }        
    }
}