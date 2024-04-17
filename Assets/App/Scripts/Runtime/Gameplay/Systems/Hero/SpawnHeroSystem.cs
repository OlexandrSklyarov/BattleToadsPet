using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Gameplay.Views.Camera;
using BT.Runtime.Gameplay.Views.Hero;
using BT.Runtime.Gameplay.Views.World;
using BT.Runtime.Services.Spawn;
using Leopotam.EcsLite;
using VContainer;

namespace BT.Runtime.Gameplay.Systems.Hero
{
    public sealed class SpawnHeroSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            var spawnPoint = data.DIResolver.Resolve<SpawnPointTag>().transform;
            var camera = data.DIResolver.Resolve<ICameraController>();
            var itemGenerator =  data.DIResolver.Resolve<IItemGenerator>();

            var world = systems.GetWorld();

            var view = itemGenerator.GetHero(HeroType.Rash, spawnPoint);         

            camera.FollowTarget(view);

            var entity = world.NewEntity();

            //Hero
            ref var configComp = ref world.GetPool<CharacterConfigComponent>().Add(entity);
            configComp.ConfigRef = view.Config;

            //character controller
            ref var cc = ref  world.GetPool<CharacterEngineComponent>().Add(entity);
            cc.CharacterControllerRef = view;

            //character transform
            ref var tr = ref  world.GetPool<TranslateComponent>().Add(entity);
            tr.TrRef = view.transform;

            //character body transform (model)
            ref var body = ref  world.GetPool<BodyTransformComponent>().Add(entity);
            body.BodyTrRef = view.Model;

            //movement
            world.GetPool<MovementDataComponent>().Add(entity);

            //input
            world.GetPool<InputDataComponent>().Add(entity);
        }
    }
}
