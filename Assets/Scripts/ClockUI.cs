using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using VContainer;
using System;
using System.Linq;
using System.Collections.Generic;
using Codice.Client.Common;

public class ClockUI : MonoBehaviour
    {
    private TMP_Text[] textMeshPros;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text stopwatchText;
    [SerializeField] private TMP_Text lapTimestampText;
    [SerializeField] private GameObject clockGameObject;
    [SerializeField] private GameObject timeZoneGameObject;

    [SerializeField] private GameObject timerbuttonPrefab; // Reference to the button prefab
    [SerializeField] private Transform timerbuttonParent; // Parent transform to hold the buttons
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip finishSound;

    private bool isCountdownRunning = false;

    private ISystemClock systemClock;
    private ICountdown countdown;
    private IStopwatch stopwatch;
    private ITimeZoneClock timeZoneClock;
    private bool makingSound;

    [Inject]
    public void Construct(ISystemClock systemClock, ICountdown countdown, IStopwatch stopwatch, ITimeZoneClock timeZoneClock)
        {
        this.systemClock = systemClock;
        this.countdown = countdown;
        this.stopwatch = stopwatch;
        this.timeZoneClock = timeZoneClock;

        }

    private void Start()
        {
        textMeshPros = clockGameObject.GetComponentsInChildren<TMP_Text>();
        TMP_Text myTimeZone = textMeshPros[0];
        TMP_Text currentTimeZone = textMeshPros[1];


        textMeshPros = timeZoneGameObject.GetComponentsInChildren<TMP_Text>();
        TMP_Text aTimeZone = textMeshPros[0];
        TMP_Text timeZoneName = textMeshPros[1];

        //timeZoneClock.CurrentTime
        //    .Subscribe(timeZone => timeZoneText.text = FormatTime(timeZone))
        //    .AddTo(this);

        systemClock.MyTime
            .Subscribe(time => myTimeZone.text = FormatTime(time))
            .AddTo(this);

        systemClock.MyTime
            .Subscribe(time => currentTimeZone.text = FormatTimeZone(time))
            .AddTo(this);

        systemClock.CurrentTime
            .Subscribe(time => aTimeZone.text = FormatTime(time))
             .AddTo(this);

        systemClock.CurrentTime
            .Subscribe(time => timeZoneName.text = FormatTimeZone(time))
            .AddTo(this);

        stopwatch.ElapsedTime
            .Subscribe(time => stopwatchText.text = FormatTimeSpan(time))
            .AddTo(this);

        stopwatch.LapData
            .Subscribe(lapData => lapTimestampText.text = FormatTimeSpan(lapData.ElapsedTime)) 
            .AddTo(this);

        CreateButtons(20, 5, 5); // Instantiate 4 buttons starting from 0 and incrementing by 5

        countdown.RemainingTime
            .Subscribe(time => UpdateCountdown(time))
            .AddTo(this);

        }


    private void CreateButtons(int numberOfButtons, int startingValue, int step)
        {
        // Instantiate buttons
        for (int i = 0; i < numberOfButtons; i++)
            {
            // Calculate the value for the current button in TimeSpan format
            int buttonValue = startingValue + (i * step);
            TimeSpan timeSpanValue = TimeSpan.FromSeconds(buttonValue);

            // Create a new button
            GameObject newButton = Instantiate(timerbuttonPrefab, timerbuttonParent);
            newButton.SetActive(true);

            // Set the button name using the calculated value in TimeSpan format
            newButton.GetComponentInChildren<TMP_Text>().text = FormatTimeSpan(timeSpanValue);

            // Add button click listener to start the countdown
            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("Button " + buttonValue + " clicked!");

                // Start the countdown with the corresponding button value
                StartCountdown(buttonValue);
            });
            }
        }

    private void StartCountdown(int durationInSeconds)
        {
        countdown.StartCountdown(TimeSpan.FromSeconds(durationInSeconds));
        }

    private void UpdateCountdown(TimeSpan time)
        {
        if (time <= TimeSpan.Zero && !makingSound)
        {
            makingSound = true;
            Debug.Log(makingSound);
            PlayFinishSound();
        }
        countdownText.text = FormatTimeSpan(time);
        }

    public void PlayFinishSound() //Sound Test
        {
            audioSource.PlayOneShot(finishSound);
            countdown.ResetCountdown();
            makingSound = false;
            Debug.Log(makingSound);
        }
    private string FormatTime(DateTimeOffset time)
        {
        return time.ToString("HH:mm:ss");
        }

    private string FormatTimeSpan(TimeSpan timeSpan)
        {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }


    private string FormatTimeZone(DateTimeOffset time)
        {
        var systemTimeZones = TimeZoneInfo.GetSystemTimeZones();
        TimeZoneInfo matchedTimeZone = systemTimeZones.FirstOrDefault(zone => zone.BaseUtcOffset == time.Offset);
        string timeZoneName = matchedTimeZone != null ? matchedTimeZone.StandardName : "Unknown Time Zone";
        return timeZoneName;
        }


    }
