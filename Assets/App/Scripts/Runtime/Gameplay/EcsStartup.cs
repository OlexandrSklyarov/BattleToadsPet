using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Gameplay.Systems.Character;
using BT.Runtime.Gameplay.Systems.Hero;
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
            _isInitialized = true;
        }

        private void Start()
        {
            var sharedData = new SharedData()
            {
                DIResolver = _resolver
            };

            _world = new EcsWorld ();
            _systems = new EcsSystems (_world, sharedData);
            _systems
                .Add(new SpawnHeroSystem())
                .Add(new CharacterCheckGroundSystem())
                .Add(new HeroApplyInputSystem())
                .Add(new CharacterControllerMoveSystem())
                .Add(new BodyRotateSystem())
                .Add(new CharacterJumpSystem())
                .Add(new CharacterGravitySystem())
                
                // register additional worlds here, for example:
                .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();
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