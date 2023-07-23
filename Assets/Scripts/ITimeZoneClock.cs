using System;
using UniRx;

public interface ITimeZoneClock
    {
    IObservable<DateTimeOffset> CurrentTime { get; }
    IObservable<TimeZoneInfo> CurrentTimeZone { get; }
    TimeZoneInfo SelectedTimeZone { get; set; }
    }