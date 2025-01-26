using BT.Runtime.Data.Configs;
using BT.Runtime.Services.Player;
using Game.Runtime.Services.LoadingOperations;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace BT
{
    public class RootGameLifetimeScope : LifetimeScope
    {
        [SerializeField] private MainConfig _mainConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainConfig);
            builder.RegisterInstance(_mainConfig.LevelDataBase);
            builder.RegisterInstance(_mainConfig.UI);
            builder.RegisterInstance(_mainConfig.Camera);
            builder.RegisterInstance(_mainConfig.Factory);

            builder.Register<ILoadingScreenProvider, LoadingScreenProvider>(Lifetime.Scoped);
            
            builder.Register<PlayerDataStorageService>(Lifetime.Transient)
                .AsImplementedInterfaces()
                .AsSelf(); 
        }
    }
}
