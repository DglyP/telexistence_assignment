using System;

public interface ISystemClock
    {
    IObservable<DateTimeOffset> CurrentTime { get; }
    IObservable<DateTimeOffset> MyTime { get; }
    TimeZoneInfo SelectedTimeZone { get; set; }
    }