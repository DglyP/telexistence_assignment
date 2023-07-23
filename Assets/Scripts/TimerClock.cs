using System;
using UniRx;

public class TimerClock : ISystemClock
    {
    private Subject<Unit> timerSubject = new Subject<Unit>();
    private DateTimeOffset startTime;

    public TimeZoneInfo SelectedTimeZone { get; set; }

    // Observable representing the current time in the selected time zone
    public IObservable<DateTimeOffset> CurrentTime => Observable.EveryUpdate()
        .Select(_ => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, SelectedTimeZone))
        .DistinctUntilChanged();

    // Observable representing the current time in the local time zone
    public IObservable<DateTimeOffset> MyTime => Observable.EveryUpdate()
        .Select(_ => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, SelectedTimeZone))
        .DistinctUntilChanged();

    // Observable representing the elapsed time since the timer was started
    public IObservable<TimeSpan> Timer => timerSubject.Select(_ => GetElapsedTime()).DistinctUntilChanged();

    // Starts the timer
    public void StartTimer()
        {
        startTime = DateTimeOffset.Now;
        timerSubject.OnNext(Unit.Default);
        }

    // Calculates and returns the elapsed time since the timer was started
    private TimeSpan GetElapsedTime()
        {
        return DateTimeOffset.Now - startTime;
        }
    }
