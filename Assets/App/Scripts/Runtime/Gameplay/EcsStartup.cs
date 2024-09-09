using BT.Runtime.Gameplay.Combat.Services;
using BT.Runtime.Gameplay.Combat.Systems;
using BT.Runtime.Gameplay.Enemy.Systems;
using BT.Runtime.Gameplay.General.Systems;
using BT.Runtime.Gameplay.Hero.Systems;
using BT.Runtime.Gameplay.Map.Systems;
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
                DIResolver = _resolver,
                DetectTargetService = new DetectTargetService()
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

                //Map
                .Add(new SpawnMapSystem())

                //Hero
                .Add(new SpawnHeroSystem())
                .Add(new HeroApplyInputSystem())
                .Add(new CharacterContrellerCheckGroundSystem())
                .Add(new HeroSetLookSystem())
                .Add(new BodyRotateSystem())
                .Add(new CharacterGravitySystem())
                .Add(new CharacterAttackSystem())
                .Add(new CharacterJumpSystem())
                .Add(new ChangeHorizontalVelocitySystem())
                .Add(new MovementCharacterControllerSystem())
                .Add(new HeroIKFootIKSystem())
                .Add(new HeroAnimationSystem())

                //combat
                .Add(new RotateToNearTargetSystem())
                .Add(new AttackRequestHandleSystem())

                //enemy
                .Add(new EnemySpawnSystem())
                .Add(new EnemyIdleStateSystem())
                .Add(new EnemyChaseStateSystem())
                .Add(new EnemyAttackStateSystem())
                .Add(new EnemyAnimationSystem())

        #if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
        #endif
                .Init();
        }

        private void InitUpdateFixedSystems(SharedData sharedData)
        {
            _fixedSystems = new EcsSystems(_world, sharedData);
            _fixedSystems

        #if UNITY_EDITOR
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