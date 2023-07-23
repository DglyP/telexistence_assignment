// This class represents a Stopwatch and implements the IStopwatch interface.

using System;
using UniRx;

public class Stopwatch : IStopwatch
    {
    // Inner class to hold lap data for the stopwatch.
    public class StopwatchLapData
        {
        public TimeSpan ElapsedTime { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public bool resetLaps { get; set; }
        }

    private IDisposable stopwatchDisposable;

    private Subject<TimeSpan> elapsedTimeSubject;
    public IObservable<TimeSpan> ElapsedTime { get; private set; }

    private Subject<StopwatchLapData> lapDataSubject;
    private Subject<bool> isPausedSubject = new Subject<bool>();

    public IObservable<bool> IsPaused => isPausedSubject;
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

    // Starts the stopwatch.
    public void StartStopwatch()
        {
        StopStopwatch();

        // If not paused, start from the current time, else continue from the paused time.
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

    // Pauses the stopwatch and records the elapsed time at pause.
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

    // Resumes the stopwatch from the paused state.
    public void ResumeStopwatch()
        {
        // Set the start time to continue from the paused time and resume the stopwatch.
        startTime = DateTimeOffset.Now - pausedElapsedTime;
        isPaused = false;
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

    // Resets the stopwatch to zero.
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

    // Stops the stopwatch without resetting it.
    public void StopStopwatch()
        {
        stopwatchDisposable?.Dispose();
        isRunningProperty.Value = false;
        isPaused = false;
        }

    // Records a lap in the stopwatch.
    public void Lap()
        {
        if (stopwatchDisposable != null)
            {
            // Calculate the elapsed time since the last lap and record the current lap.
            lapElapsedTime = DateTimeOffset.Now - startTime + totalElapsedTime - lapStartTime;
            lapStartTime = DateTimeOffset.Now - startTime + totalElapsedTime;
            var lapData = new StopwatchLapData
                {
                ElapsedTime = lapElapsedTime,
                Timestamp = DateTimeOffset.Now
                };
            lapDataSubject.OnNext(lapData);
            }
        }
    }
