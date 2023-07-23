using System;
using UniRx;
using static Stopwatch;

public interface IStopwatch
    {
    IReadOnlyReactiveProperty<bool> IsRunning { get; }
    IObservable<TimeSpan> ElapsedTime { get; }
    IObservable<StopwatchLapData> LapData { get; } 
    void StartStopwatch();
    void PauseStopwatch();
    void ResumeStopwatch();
    void StopStopwatch();
    void Lap();
    void ResetStopwatch();
    }
