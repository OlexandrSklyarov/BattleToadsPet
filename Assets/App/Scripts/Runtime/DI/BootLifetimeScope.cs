using BT.Runtime.Boot;
using VContainer;
using VContainer.Unity;

namespace BT
{
    public class BootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {           
            builder.RegisterComponentInHierarchy<AppStartup>()
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}

