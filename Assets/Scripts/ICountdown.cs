using System;

public interface ICountdown
    {
    IObservable<TimeSpan> RemainingTime { get; }
    void StartCountdown(TimeSpan duration);
    void StopCountdown();
    void PauseCountdown();
    void ResumeCountdown();
    void ResetCountdown();
    }
