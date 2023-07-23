// This interface defines the contract for an observable timezone clock.

using System;
using UniRx;

public interface ITimeZoneClock
    {
    // An observable sequence of DateTimeOffset representing the current system time.
    // This sequence emits updates on the current time at regular intervals.
    IObservable<DateTimeOffset> CurrentTime { get; }

    // An observable sequence of TimeZoneInfo representing the current timezone.
    // This sequence emits updates on the current timezone at regular intervals.
    IObservable<TimeZoneInfo> CurrentTimeZone { get; }

    // The currently selected timezone for the clock.
    // This property allows setting the timezone to be used for displaying the time.
    TimeZoneInfo SelectedTimeZone { get; set; }
    }
