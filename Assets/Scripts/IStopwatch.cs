// This interface defines the contract for implementing a stopwatch.

using System;
using UniRx;
using static Stopwatch;

public interface IStopwatch
    {
    // A read-only reactive property indicating whether the stopwatch is currently running or not.
    // True if the stopwatch is running, false otherwise.
    IReadOnlyReactiveProperty<bool> IsRunning { get; }

    // An observable sequence of TimeSpan representing the elapsed time of the stopwatch.
    // This sequence emits updates on the elapsed time at regular intervals while the stopwatch is running.
    IObservable<TimeSpan> ElapsedTime { get; }

    // An observable sequence of bool indicating whether the stopwatch is currently paused or not.
    // True if the stopwatch is paused, false otherwise.
    IObservable<bool> IsPaused { get; }

    // An observable sequence of StopwatchLapData representing lap data of the stopwatch.
    // This sequence emits lap data updates when the Lap method is called.
    IObservable<StopwatchLapData> LapData { get; }

    // Starts the stopwatch, beginning the measurement of elapsed time.
    void StartStopwatch();

    // Pauses the running stopwatch, temporarily stopping the emission of elapsed time updates.
    void PauseStopwatch();

    // Resumes a paused stopwatch to continue emitting updates on the elapsed time.
    void ResumeStopwatch();

    // Stops the stopwatch, terminating the measurement of elapsed time.
    void StopStopwatch();

    // Records a lap time for the current elapsed time of the stopwatch.
    // Lap data is emitted through the LapData observable sequence.
    void Lap();

    // Resets the stopwatch to zero elapsed time and stops it.
    // No further updates on the elapsed time will be emitted until started again.
    void ResetStopwatch();
    }
