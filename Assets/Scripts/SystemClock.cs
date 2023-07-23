using System;
using UniRx;

public class SystemClock : ISystemClock, ITimeZoneClock
    {
    // Property to store the selected time zone
    public TimeZoneInfo SelectedTimeZone { get; set; }

    // Observable that emits the selected time zone
    public IObservable<TimeZoneInfo> CurrentTimeZone => Observable.Return(SelectedTimeZone);

    // Observable that emits the current time in the selected time zone
    public IObservable<DateTimeOffset> CurrentTime => Observable.EveryUpdate()
        .Select(_ => TimeZoneInfo.ConvertTime(DateTimeOffset.Now, SelectedTimeZone))
        .DistinctUntilChanged();

    // Observable that emits the current time in the system's local time zone
    public IObservable<DateTimeOffset> MyTime => Observable.EveryUpdate()
        .Select(_ => DateTimeOffset.Now)
        .DistinctUntilChanged();
    }
