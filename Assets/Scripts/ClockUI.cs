using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using VContainer;
using System;
using System.Linq;
using System.Collections.Generic;

public class ClockUI : MonoBehaviour
    {
    private TMP_Text[] textMeshPros;
    //[SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text stopwatchText;
    [SerializeField] private TMP_Text lapTimestampText;
    [SerializeField] private GameObject clockGameObject;
    [SerializeField] private GameObject timeZoneGameObject;

    [SerializeField] private TMP_Text lapTextPrefab; // Reference to the TMP_Text prefab
    [SerializeField] private Transform lapTextParent; // Parent transform to hold the lap text objects

    [SerializeField] private GameObject timerbuttonPrefab; // Reference to the button prefab
    [SerializeField] private Transform timerbuttonParent; // Parent transform to hold the buttons

    private List<TMP_Text> lapTexts = new List<TMP_Text>(); // List to keep track of lap text objects


    private ISystemClock systemClock;
    private ICountdown countdown;
    private IStopwatch stopwatch;
    private ITimeZoneClock timeZoneClock;

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

        stopwatch.LapData
            .Subscribe(lapData => CreateLapText(FormatTimeSpan(lapData.ElapsedTime)))
            .AddTo(this);

        CreateButtons(10, 0, 5); // Instantiate 10 buttons starting from 0 and incrementing by 5

        countdown.RemainingTime
            .Subscribe(time => countdownText.text = FormatTimeSpan(time))
            .AddTo(this);
        }

    private void CreateLapText(string lapTime)
        {
        // Instantiate the lap text prefab and set its text
        TMP_Text lapText = Instantiate(lapTextPrefab, lapTextParent);
        lapText.text = lapTime;
        lapText.gameObject.SetActive(true);

        // Add the lap text to the list
        lapTexts.Add(lapText);

        // Adjust the position of the lap texts (you may need to customize this based on your layout)
        for (int i = 0; i < lapTexts.Count; i++)
            {
            lapTexts[i].rectTransform.anchoredPosition = new Vector2(0f, -30f * i);
            }
        }

    private void CreateButtons(int numberOfButtons, int startingValue, int step)
        {
        // Instantiate buttons
        for (int i = 0; i < numberOfButtons; i++)
            {
            // Calculate the value for the current button
            int buttonValue = startingValue + (i * step);

            // Create a new button
            GameObject newButton = Instantiate(timerbuttonPrefab, timerbuttonParent);
            newButton.SetActive(true);

            // Set the button name using the calculated value
            newButton.GetComponentInChildren<TMP_Text>().text = buttonValue.ToString();

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
