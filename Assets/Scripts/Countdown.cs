using System;
using UniRx;
using UnityEngine;
using VContainer;

public class Countdown : ICountdown
    {
    private ISystemClock systemClock;
    private IDisposable countdownDisposable;
    private Subject<TimeSpan> remainingTimeSubject;
    private DateTimeOffset startTime;
    private TimeSpan remainingDuration;
    private TimeSpan originalDuration;
    private Subject<bool> countdownDoneSubject = new Subject<bool>();
    public IObservable<bool> CountdownDone => countdownDoneSubject.AsObservable();

    [Inject]
    public void Construct(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        remainingTimeSubject = new Subject<TimeSpan>();
        RemainingTime = remainingTimeSubject.AsObservable();
        }

    public IObservable<TimeSpan> RemainingTime { get; private set; }

public void StartCountdown(TimeSpan duration)
{
    StopCountdown();

    originalDuration = duration;
    startTime = DateTimeOffset.Now;
    remainingDuration = duration;

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


    public void PauseCountdown()
        {
        if (countdownDisposable != null)
            {
            var elapsedDuration = DateTimeOffset.Now - startTime;
            remainingDuration -= elapsedDuration;
            countdownDisposable.Dispose();
            }
        }

    public void ResumeCountdown()
        {
        if (remainingDuration > TimeSpan.Zero)
            {
            StartCountdown(remainingDuration);
            }
        }

    public void StopCountdown()
        {
        countdownDisposable?.Dispose();
        }

    public void ResetCountdown()
        {
        StopCountdown();
        remainingTimeSubject.OnNext(TimeSpan.Zero);
        remainingDuration = originalDuration;
        }
    }
