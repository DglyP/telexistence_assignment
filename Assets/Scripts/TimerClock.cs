using System;
using UniRx;

public class TimerClock : ISystemClock
    {
    private Subject<Unit> timerSubject = new Subject<Unit>();
    private DateTimeOffset startTime;

    public TimeZoneInfo SelectedTimeZone { get; set; }

    public IObservable<DateTimeOffset> CurrentTime => Observable.EveryUpdate()
        .Select(_ => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, SelectedTimeZone))
        .DistinctUntilChanged();

    public IObservable<DateTimeOffset> MyTime => Observable.EveryUpdate()
    .Select(_ => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, SelectedTimeZone))
    .DistinctUntilChanged();

    public IObservable<TimeSpan> Timer => timerSubject.Select(_ => GetElapsedTime()).DistinctUntilChanged();

    public void StartTimer()
        {
        startTime = DateTimeOffset.Now;
        timerSubject.OnNext(Unit.Default);
        }


    private TimeSpan GetElapsedTime()
        {
        return DateTimeOffset.Now - startTime;
        }
    }
