using System.Runtime.InteropServices;
using BT.Runtime.Gameplay.Components;
using BT.Runtime.Gameplay.General.Components;
using BT.Runtime.Gameplay.Hero.Components;
using BT.Runtime.Gameplay.Services.GameWorldData;
using BT.Runtime.Gameplay.Views.Camera;
using BT.Runtime.Gameplay.Views.Hero;
using BT.Runtime.Gameplay.Views.World;
using BT.Runtime.Services.Spawn;
using Leopotam.EcsLite;
using VContainer;

namespace BT.Runtime.Gameplay.Hero.Systems
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

            view.Init(world, world.PackEntity(entity));

            //Hero
            world.GetPool<HeroTeg>().Add(entity);

            //hero config
            ref var configComp = ref world.GetPool<CharacterConfigComponent>().Add(entity);
            configComp.ConfigRef = view.Config;

            //character controller
            ref var cc = ref  world.GetPool<CharacterControllerEngineComponent>().Add(entity);
            cc.ControllerRef = view;

            //character transform
            ref var tr = ref  world.GetPool<TranslateComponent>().Add(entity);
            tr.TrRef = view.transform;

            //velocity
            world.GetPool<CharacterVelocityComponent>().Add(entity);

            //character body transform (model)
            ref var body = ref  world.GetPool<ViewModelTransformComponent>().Add(entity);
            body.ModelTransformRef = view.Model;

            //movement
            world.GetPool<MovementDataComponent>().Add(entity);

            //input
            world.GetPool<InputDataComponent>().Add(entity);

            //animations
            ref var animation = ref world.GetPool<AnimatorComponent>().Add(entity);
            animation.AnimatorRef = view.Animator;

            //IK
            ref var ik = ref world.GetPool<CharacterFootIKComponent>().Add(entity);
            ik.FootIKRef = view.FootIK;

            //attack
            ref var attack = ref world.GetPool<CharacterAttackComponent>().Add(entity);

            //check ground
            ref var ground = ref world.GetPool<CharacterCheckGroundComponent>().Add(entity);
            ground.FeetCollider = view.FeetCollider;
            ground.BodyBounds = view.CC.bounds;
            ground.GroundResult = new UnityEngine.Collider[10];
            ground.HeadBumpResult = new UnityEngine.Collider[10];
        }
    }
}
