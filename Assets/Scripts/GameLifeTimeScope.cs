using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
    {
    protected override void Configure(IContainerBuilder builder)
        {
        builder.Register<ISystemClock, SystemClock>(Lifetime.Singleton);
        builder.Register<ITimeZoneClock, SystemClock>(Lifetime.Singleton);
        builder.Register<ICountdown, Countdown>(Lifetime.Singleton);
        builder.Register<IStopwatch, Stopwatch>(Lifetime.Singleton);
        }
    }
