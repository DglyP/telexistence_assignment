using UnityEngine;
using UniRx;
using TMPro;
using VContainer;
using System;

public class ClockUI : MonoBehaviour
    {
    [SerializeField] private TMP_Text timeZoneText;
    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text stopwatchText;
    [SerializeField] private TMP_Text lapTimestampText; 

    private ISystemClock systemClock;
    private ICountdown countdown;
    private IStopwatch stopwatch;

    [Inject]
    public void Construct(ISystemClock systemClock, ICountdown countdown, IStopwatch stopwatch)
        {
        this.systemClock = systemClock;
        this.countdown = countdown;
        this.stopwatch = stopwatch;
        }

    private void Start()
        {
        systemClock.CurrentTime
            .Subscribe(time => timeZoneText.text = FormatTime(time))
            .AddTo(this);

        systemClock.MyTime
            .Subscribe(time => currentTimeText.text = FormatTime(time))
            .AddTo(this);

        countdown.RemainingTime
            .Subscribe(time => countdownText.text = FormatTimeSpan(time))
            .AddTo(this);

        stopwatch.ElapsedTime
            .Subscribe(time => stopwatchText.text = FormatTimeSpan(time))
            .AddTo(this);

        stopwatch.LapData
            .Subscribe(lapData => lapTimestampText.text = FormatTime(lapData.Timestamp)) 
            .AddTo(this);
        }

    private string FormatTime(DateTimeOffset time)
        {
        return time.ToString("HH:mm:ss");
        }

    private string FormatTimeSpan(TimeSpan timeSpan)
        {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }
    }
