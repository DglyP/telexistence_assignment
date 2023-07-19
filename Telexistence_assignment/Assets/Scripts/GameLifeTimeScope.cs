using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
    {
    protected override void Configure(IContainerBuilder builder)
        {
        // Register your dependencies here
        builder.Register<ISystemClock, SystemClock>(Lifetime.Singleton);
        builder.Register<ISystemClock, TimerClock>(Lifetime.Singleton);
        }
    }
