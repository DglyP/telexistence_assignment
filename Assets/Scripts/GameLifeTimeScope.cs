using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
    {
    // The Configure method is called when the lifetime scope is being created
    protected override void Configure(IContainerBuilder builder)
        {
        // Register the implementation classes and their lifetimes with the container

        // Register the SystemClock as ISystemClock with Singleton lifetime
        builder.Register<ISystemClock, SystemClock>(Lifetime.Singleton);

        // Register the SystemClock as ITimeZoneClock with Singleton lifetime
        builder.Register<ITimeZoneClock, SystemClock>(Lifetime.Singleton);

        // Register the Countdown class as ICountdown with Singleton lifetime
        builder.Register<ICountdown, Countdown>(Lifetime.Singleton);

        // Register the Stopwatch class as IStopwatch with Singleton lifetime
        builder.Register<IStopwatch, Stopwatch>(Lifetime.Singleton);
        }
    }
