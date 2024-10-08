using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay;
using BT.Runtime.Gameplay.Views.Camera;
using BT.Runtime.Gameplay.Views.World;
using BT.Runtime.Services.Input;
using BT.Runtime.Services.Player;
using BT.Runtime.Services.Spawn;
using BT.Runtime.Services.Spawn.Factory;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BT
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainConfig _mainConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);

            RegisterInHierarchy(builder);

            RegisterServices(builder);

            RegisterFactories(builder);
        }

        private void RegisterFactories(IContainerBuilder builder)
        {
            builder.Register<ICharacterFactory, CharacterFactory>(Lifetime.Singleton);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IInputService, DeviceInputService>(Lifetime.Singleton);
            builder.Register<IItemGenerator, ItemGenerator>(Lifetime.Singleton);

            builder.Register<PlayerDataStorageService>(Lifetime.Scoped)
            .AsImplementedInterfaces()
            .AsSelf();
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainConfig);
            builder.RegisterInstance(_mainConfig.Factory);
            builder.RegisterInstance(_mainConfig.Camera);
        }

        private void RegisterInHierarchy(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<EcsStartup>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterComponentInHierarchy<SpawnPointTag>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterComponentInHierarchy<CameraController>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterComponentInHierarchy<WorldTeg>()
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}
