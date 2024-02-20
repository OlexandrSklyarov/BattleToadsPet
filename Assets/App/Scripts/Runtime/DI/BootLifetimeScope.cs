using BT.Runtime.Data.Configs;
using BT.Runtime.Services.Levels;
using BT.Runtime.Services.Player;
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

        builder.Register<ISceneLoadService, SceneLoadService>(Lifetime.Scoped);
        builder.Register<PlayerDataStorageService>(Lifetime.Scoped)
            .AsImplementedInterfaces()
            .AsSelf();
    }
}
