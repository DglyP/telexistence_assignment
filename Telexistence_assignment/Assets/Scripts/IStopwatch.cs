using System;
using UniRx;
using static Stopwatch;

public interface IStopwatch
    {
    IObservable<TimeSpan> ElapsedTime { get; }
    IObservable<StopwatchLapData> LapData { get; } 
    void StartStopwatch();
    void StopStopwatch();
    void Lap();
    void ResetStopwatch();
    }
