// This interface defines the contract for an observable system clock.

using System;
using UniRx;

public interface ISystemClock
    {
    // An observable sequence of DateTimeOffset representing the current system time.
    // This sequence emits updates on the current time at regular intervals.
    IObservable<DateTimeOffset> CurrentTime { get; }

    // An observable sequence of DateTimeOffset representing the time in a specific timezone.
    // This sequence emits updates on the time in the selected timezone at regular intervals.
    IObservable<DateTimeOffset> MyTime { get; }

    // The currently selected timezone for the MyTime sequence.
    // This property allows setting the timezone to convert the current system time to the selected timezone.
    TimeZoneInfo SelectedTimeZone { get; set; }
    }
