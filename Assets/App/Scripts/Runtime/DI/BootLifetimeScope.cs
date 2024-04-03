using BT.Runtime.Boot;
using BT.Runtime.Data.Configs;
using BT.Runtime.Services.Player;
using Game.Runtime.Services.LoadingOperations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootLifetimeScope : LifetimeScope
{
    [SerializeField] private MainConfig _mainConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_mainConfig.LevelDataBase);
        builder.RegisterInstance(_mainConfig.UI);

        builder.Register<ILoadingScreenProvider, LoadingScreenProvider>(Lifetime.Scoped);
        
        builder.Register<PlayerDataStorageService>(Lifetime.Scoped)
            .AsImplementedInterfaces()
            .AsSelf();

        builder.RegisterComponentInHierarchy<AppStartup>()
            .AsImplementedInterfaces()
            .AsSelf();
    }
}
