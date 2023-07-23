// This interface defines the contract for implementing a countdown timer.

using System;
using UniRx;

public interface ICountdown
    {
    // A read-only reactive property indicating whether the countdown is currently running or not.
    // True if the countdown is running, false otherwise.
    IReadOnlyReactiveProperty<bool> IsRunning { get; }

    // An observable sequence of TimeSpan representing the remaining time of the countdown.
    // This sequence emits updates on the remaining time at regular intervals.
    IObservable<TimeSpan> RemainingTime { get; }

    // An observable sequence of bool indicating when the countdown is done (reached 0 duration).
    // It emits a value of true when the countdown is complete.
    IObservable<bool> CountdownDone { get; }

    // Starts the countdown with the specified duration.
    // The countdown will run for the specified duration, and updates on the remaining time will be emitted.
    void StartCountdown(TimeSpan duration);

    // Pauses the running countdown, temporarily stopping the emission of remaining time updates.
    void PauseCountdown();

    // Resumes a paused countdown to continue emitting updates on the remaining time.
    void ResumeCountdown();

    // Stops the countdown, terminating the emission of remaining time updates.
    void StopCountdown();

    // Resets the countdown to its original duration and stops it.
    // The remaining time will be set to zero, and no further updates will be emitted until started again.
    void ResetCountdown();
    }
