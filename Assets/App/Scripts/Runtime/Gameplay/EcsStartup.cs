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

            _world = new EcsWorld ();
            _systems = new EcsSystems (_world, sharedData);
            _systems
                .AddWorld (new EcsWorld (), "events")

                .Add(new SpawnHeroSystem())
                .Add(new CharacterCheckGroundSystem())
                .Add(new HeroApplyInputSystem())
                .Add(new ChangeSpeedSystem())
                .Add(new CharacterControllerMoveSystem())
                .Add(new BodyRotateSystem())
                .Add(new CharacterJumpSystem())
                .Add(new CharacterGravitySystem())
                .Add(new HeroAnimationSystem())                
                
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();

            _isInitialized = true;
        }

        private void Update () 
        {
            if (!_isInitialized) return;

            _systems?.Run ();
        }

        private void OnDestroy () 
        {            
            _systems?.Destroy();
            _systems = null;
           
            _world?.Destroy();
            _world = null;            
        }        
    }
}