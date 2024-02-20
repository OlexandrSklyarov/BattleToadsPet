using Leopotam.EcsLite;
using UnityEngine;

namespace BT 
{
    sealed class EcsStartup : MonoBehaviour 
    {
        EcsWorld _world;        
        IEcsSystems _systems;

        public void Init()
        {
            _world = new EcsWorld ();
            _systems = new EcsSystems (_world);
            _systems
                // register your systems here, for example:
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
                
                // register additional worlds here, for example:
                // .AddWorld (new EcsWorld (), "events")
#if UNITY_EDITOR
                // add debug systems for custom worlds here, for example:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
#endif
                .Init ();
        }

        void Update () 
        {
            _systems?.Run ();
        }

        void OnDestroy () 
        {            
            _systems?.Destroy();
            _systems = null;
           

            _world?.Destroy();
            _world = null;            
        }
    }
}