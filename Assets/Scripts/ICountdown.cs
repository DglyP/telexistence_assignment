using System;
using UniRx;

public interface ICountdown
    {
    IObservable<TimeSpan> RemainingTime { get; }
    void StartCountdown(TimeSpan duration);
    void PauseCountdown();
    void ResumeCountdown();
    void StopCountdown();
    void ResetCountdown();
    }
