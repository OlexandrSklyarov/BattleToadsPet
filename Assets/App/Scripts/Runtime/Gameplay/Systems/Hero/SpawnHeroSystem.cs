using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Gameplay.Systems.Character;
using BT.Runtime.Gameplay.Views.Camera;
using BT.Runtime.Gameplay.Views.World;
using Leopotam.EcsLite;
using VContainer;

namespace BT.Runtime.Gameplay.Systems.Hero
{
    public sealed class SpawnHeroSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var data = systems.GetShared<SharedData>();
            var config = data.DIResolver.Resolve<MainConfig>().Hero;
            var spawnPoint = data.DIResolver.Resolve<SpawnPointTag>().transform;
            var camera = data.DIResolver.Resolve<ICameraController>();

            var world = systems.GetWorld();

            var heroView = UnityEngine.Object.Instantiate(config.HeroPrefab, spawnPoint);           

            camera.FollowTarget(heroView);

            var entity = world.NewEntity();

            //character controller
            ref var cc = ref  world.GetPool<CharacterControllerComponent>().Add(entity);
            cc.CCRef = heroView;

            //character transform
            ref var tr = ref  world.GetPool<TranslateComponent>().Add(entity);
            tr.TrRef = heroView.transform;

            //character body transform (model)
            ref var body = ref  world.GetPool<BodyTransformComponent>().Add(entity);
            body.BodyTrRef = heroView.Model;

            //movement
            world.GetPool<MovementDataComponent>().Add(entity);

            //input
            world.GetPool<InputDataComponent>().Add(entity);
        }
    }
}
