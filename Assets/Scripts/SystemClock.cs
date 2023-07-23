using System;
using UniRx;

public class SystemClock : ISystemClock , ITimeZoneClock
    {
    public TimeZoneInfo SelectedTimeZone { get; set; }
    public IObservable<TimeZoneInfo> CurrentTimeZone => Observable.Return(SelectedTimeZone);

    public IObservable<DateTimeOffset> CurrentTime => Observable.EveryUpdate()
        .Select(_ => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, SelectedTimeZone))
        .DistinctUntilChanged();

    public IObservable<DateTimeOffset> MyTime => Observable.EveryUpdate()
        .Select(_ => DateTimeOffset.Now)
        .DistinctUntilChanged();
    }