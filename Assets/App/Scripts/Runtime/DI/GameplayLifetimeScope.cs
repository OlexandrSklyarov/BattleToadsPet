using BT.Runtime.Data.Configs;
using BT.Runtime.Gameplay;
using BT.Runtime.Gameplay.Views.Camera;
using BT.Runtime.Gameplay.Views.World;
using BT.Runtime.Services.Input;
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
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<IInputService, DeviceInputService>(Lifetime.Singleton);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainConfig);
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
        }
    }
}
