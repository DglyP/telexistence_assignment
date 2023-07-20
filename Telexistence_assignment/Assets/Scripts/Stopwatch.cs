using System;
using UniRx;

public class Stopwatch : IStopwatch
    {
    public class StopwatchLapData
        {
        public TimeSpan ElapsedTime { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        }

    private ISystemClock systemClock;
    private IDisposable stopwatchDisposable;
    private Subject<TimeSpan> elapsedTimeSubject;
    private Subject<StopwatchLapData> lapDataSubject;

    private DateTimeOffset startTime;
    private TimeSpan totalElapsedTime;

    public Stopwatch(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        elapsedTimeSubject = new Subject<TimeSpan>();
        lapDataSubject = new Subject<StopwatchLapData>();
        ElapsedTime = elapsedTimeSubject.AsObservable();
        LapData = lapDataSubject.AsObservable();
        }

    public IObservable<TimeSpan> ElapsedTime { get; private set; }
    public IObservable<StopwatchLapData> LapData { get; private set; }

    public void StartStopwatch()
        {
        StopStopwatch();

        startTime = DateTimeOffset.Now;
        totalElapsedTime = TimeSpan.Zero;

        stopwatchDisposable = Observable.EveryUpdate()
            .Select(_ => DateTimeOffset.Now - startTime + totalElapsedTime)
            .Do(elapsedTime =>
            {
                elapsedTimeSubject.OnNext(elapsedTime);
            })
            .Publish()
            .RefCount()
            .Subscribe();
        }

    public void StopStopwatch()
        {
        stopwatchDisposable?.Dispose();
        }

    public void Lap()
        {
        if (stopwatchDisposable != null)
            {
            var lapElapsedTime = DateTimeOffset.Now - startTime + totalElapsedTime;
            var lapTimestamp = DateTimeOffset.Now;

            totalElapsedTime += (DateTimeOffset.Now - startTime);
            startTime = DateTimeOffset.Now;

            var lapData = new StopwatchLapData
                {
                ElapsedTime = lapElapsedTime,
                Timestamp = lapTimestamp
                };

            lapDataSubject.OnNext(lapData);
            }
        }

    public void ResetStopwatch()
        {
        totalElapsedTime = TimeSpan.Zero;
        startTime = DateTimeOffset.Now;
        elapsedTimeSubject.OnNext(totalElapsedTime);
        StopStopwatch();
        }
    }
