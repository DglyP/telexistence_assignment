using System;
using UniRx;
using UnityEngine;
using static Stopwatch;

public class Stopwatch : IStopwatch
    {
    public class StopwatchLapData
        {
        public TimeSpan ElapsedTime { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        }

    private IDisposable stopwatchDisposable;
    private Subject<TimeSpan> elapsedTimeSubject;
    private Subject<StopwatchLapData> lapDataSubject;

    public IObservable<TimeSpan> ElapsedTime { get; private set; }
    public IObservable<StopwatchLapData> LapData { get; private set; }
    private ReactiveProperty<bool> isRunningProperty = new ReactiveProperty<bool>(false);

    public IReadOnlyReactiveProperty<bool> IsRunning => isRunningProperty;

    private DateTimeOffset startTime;
    private TimeSpan totalElapsedTime;
    private bool isPaused = false;
    private TimeSpan pausedElapsedTime;
    private TimeSpan lapStartTime;
    private TimeSpan lapElapsedTime;

    public Stopwatch()
        {
        elapsedTimeSubject = new Subject<TimeSpan>();
        lapDataSubject = new Subject<StopwatchLapData>();
        ElapsedTime = elapsedTimeSubject.AsObservable();
        LapData = lapDataSubject.AsObservable();
        }

    public void StartStopwatch()
        {
        StopStopwatch();

        if (!isPaused)
            {
            startTime = DateTimeOffset.Now;
            }
        else
            {
            startTime = DateTimeOffset.Now - pausedElapsedTime;
            isPaused = false;
            }

        isRunningProperty.Value = true;

        stopwatchDisposable = Observable.EveryUpdate()
            .Select(_ => DateTimeOffset.Now - startTime + totalElapsedTime)
            .Do(elapsedTime =>
            {
                elapsedTimeSubject.OnNext(elapsedTime);
            })
            .Publish()
            .RefCount()
            .Subscribe(_ => { });
        }

    public void PauseStopwatch()
        {
        if (stopwatchDisposable != null)
            {
            pausedElapsedTime = DateTimeOffset.Now - startTime + totalElapsedTime;
            stopwatchDisposable.Dispose();
            isRunningProperty.Value = false;
            isPaused = true;
            }
        }

    public void ResumeStopwatch()
        {
        if (stopwatchDisposable == null)
            {
            startTime = DateTimeOffset.Now - pausedElapsedTime;
            isRunningProperty.Value = true;

            stopwatchDisposable = Observable.EveryUpdate()
                .Select(_ => DateTimeOffset.Now - startTime + totalElapsedTime)
                .Do(elapsedTime =>
                {
                    elapsedTimeSubject.OnNext(elapsedTime);
                })
                .Publish()
                .RefCount()
                .Subscribe(_ => { });

            isPaused = false;
            }
        }

    public void ResetStopwatch()
        {
        StopStopwatch();
        totalElapsedTime = TimeSpan.Zero;
        startTime = DateTimeOffset.Now;
        elapsedTimeSubject.OnNext(totalElapsedTime);
        stopwatchDisposable?.Dispose();
        isRunningProperty.Value = false;
        isPaused = false;
        }

    public void StopStopwatch()
        {
        stopwatchDisposable?.Dispose();
        isRunningProperty.Value = false;
        isPaused = false;
        }

    public void Lap()
        {
        if (stopwatchDisposable != null)
            {
            lapElapsedTime = DateTimeOffset.Now - startTime + totalElapsedTime - lapStartTime;
            lapStartTime = DateTimeOffset.Now - startTime + totalElapsedTime;
            var lapData = new StopwatchLapData
                {
                ElapsedTime = lapElapsedTime,
                Timestamp = DateTimeOffset.Now
                };
            lapDataSubject.OnNext(lapData);
            Debug.Log("Lap Elapsed Time: " + lapElapsedTime.ToString());
            }
        }
    }
