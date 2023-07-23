using System;
using UniRx;
using UnityEngine;
using VContainer;

public class Countdown : ICountdown
    {
    private ISystemClock systemClock;
    private IDisposable countdownDisposable;
    private DateTimeOffset startTime;
    private TimeSpan remainingDuration;
    private TimeSpan originalDuration;

    // Subject to emit the countdown done state
    private Subject<bool> countdownDoneSubject = new Subject<bool>();
    public IObservable<bool> CountdownDone => countdownDoneSubject.AsObservable();

    // Subject to emit the remaining time during countdown
    private Subject<TimeSpan> remainingTimeSubject;
    public IObservable<TimeSpan> RemainingTime { get; private set; }

    // Reactive property to expose the countdown running state
    private ReactiveProperty<bool> isRunningProperty = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsRunning => isRunningProperty;

    [Inject]
    public void Construct(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        remainingTimeSubject = new Subject<TimeSpan>();
        RemainingTime = remainingTimeSubject.AsObservable();
        }

    // Start the countdown with the given duration
    public void StartCountdown(TimeSpan duration)
        {
        // Stop the current countdown if running
        StopCountdown();

        // Store the original and remaining duration
        originalDuration = duration;
        startTime = DateTimeOffset.Now;
        remainingDuration = duration;

        // Set the countdown as running
        isRunningProperty.Value = true;

        // Subscribe to updates and emit the remaining time
        countdownDisposable = Observable.EveryUpdate()
            .Select(_ => startTime.Add(remainingDuration) - DateTimeOffset.Now)
            .Scan((acc, time) => time > TimeSpan.Zero ? time : TimeSpan.Zero)
            .Do(remainingTime =>
            {
                remainingTimeSubject.OnNext(remainingTime);

                // Check if the countdown is done and emit a value through the countdownDoneSubject
                if (remainingTime <= TimeSpan.Zero)
                    {
                    countdownDoneSubject.OnNext(true);
                    }
            })
            .TakeWhile(time => time > TimeSpan.Zero)
            .Publish()
            .RefCount()
            .Subscribe();
        }

    // Pause the countdown and store the remaining duration
    public void PauseCountdown()
        {
        if (countdownDisposable != null)
            {
            var elapsedDuration = DateTimeOffset.Now - startTime;
            remainingDuration -= elapsedDuration;
            countdownDisposable.Dispose();
            isRunningProperty.Value = false;
            }
        }

    // Resume the countdown if there is remaining duration
    public void ResumeCountdown()
        {
        if (remainingDuration > TimeSpan.Zero)
            {
            StartCountdown(remainingDuration);
            isRunningProperty.Value = true;
            }
        }

    // Stop the countdown and clear resources
    public void StopCountdown()
        {
        countdownDisposable?.Dispose();
        isRunningProperty.Value = false;
        }

    // Reset the countdown to its original duration
    public void ResetCountdown()
        {
        StopCountdown();
        remainingTimeSubject.OnNext(TimeSpan.Zero);
        remainingDuration = originalDuration;
        }
    }
