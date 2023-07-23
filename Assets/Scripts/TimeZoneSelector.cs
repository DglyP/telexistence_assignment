using UnityEngine;
using TMPro;
using System;
using System.Linq;
using VContainer;
using System.Collections.Generic;
using UniRx;

public class TimeZoneSelector : MonoBehaviour
    {
    [SerializeField] private GameObject clockGameObject;
    [SerializeField] private GameObject timeZonePrefab; // Reference to the prefab containing TextMeshProUGUI
    [SerializeField] private Transform timeZoneParent; // Parent transform to hold the TextMeshProUGUI objects

    private ISystemClock systemClock;
    private TMP_Text[] textMeshPros;
    private List<GameObject> timeZoneObjects = new List<GameObject>();
    private List<TimeZoneInfo> timeZones;

    private TimeZoneInfo selectedTimeZone;

    [Inject]
    public void Construct(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        }

    private void Start()
        {

        textMeshPros = clockGameObject.GetComponentsInChildren<TMP_Text>();
        TMP_Text myTimeZone = textMeshPros[0];
        TMP_Text currentTimeZone = textMeshPros[1];

        systemClock.MyTime
            .Subscribe(time => myTimeZone.text = FormatTime(time))
            .AddTo(this);

        systemClock.MyTime
            .Subscribe(time => currentTimeZone.text = FormatTimeZone(time))
            .AddTo(this);

        timeZones = TimeZoneInfo.GetSystemTimeZones().ToList();

        // Initialize the selectedTimeZone
        selectedTimeZone = timeZones.FirstOrDefault();
        systemClock.SelectedTimeZone = selectedTimeZone;

        CreateTimeZones(timeZones); // Call the CreateTimeZones method with List<TimeZoneInfo>

        // Subscribe to the systemClock.CurrentTime once
        systemClock.CurrentTime
            .Subscribe(new TimeZoneTimeUpdater(this))
            .AddTo(this);
        }

    private void CreateTimeZones(List<TimeZoneInfo> timeZones)
        {
        foreach (var timeZone in timeZones)
            {
            // Instantiate the timeZonePrefab and set its parent to timeZoneParent
            GameObject newTimeZoneObject = Instantiate(timeZonePrefab, timeZoneParent);

            // Find the child objects with the TMP_Text components named "TimeZoneLabel" and "TimeZoneTime"
            TMP_Text newTimeZoneText = newTimeZoneObject.transform.Find("TimeZoneLabel")?.GetComponent<TMP_Text>();
            TMP_Text newTimeZoneTime = newTimeZoneObject.transform.Find("TimeZoneTime")?.GetComponent<TMP_Text>();

            // Set the text of the TextMeshProUGUI to the display name of the time zone
            if (newTimeZoneText != null)
                {
                newTimeZoneText.text = timeZone.StandardName;
                newTimeZoneText.gameObject.SetActive(true);
                }

            // Add the TimeZoneObject component to store the TimeZoneInfo in the GameObject
            TimeZoneObject timeZoneObject = newTimeZoneObject.AddComponent<TimeZoneObject>();
            timeZoneObject.TimeZone = timeZone;

            // Add the newTimeZoneObject to the list
            timeZoneObjects.Add(newTimeZoneObject);

            // Activate the newly instantiated object
            newTimeZoneObject.SetActive(true);
            }
        }


    // Helper class that implements IObserver<DateTimeOffset>
    private class TimeZoneTimeUpdater : IObserver<DateTimeOffset>
        {
        private TimeZoneSelector timeZoneSelector;

        public TimeZoneTimeUpdater(TimeZoneSelector timeZoneSelector)
            {
            this.timeZoneSelector = timeZoneSelector;
            }

        public void OnCompleted()
            {
            }

        public void OnError(Exception error)
            {
            }

        public void OnNext(DateTimeOffset currentTime)
            {
            timeZoneSelector.UpdateTimeZoneTimes(currentTime);
            }
        }

    private void UpdateTimeZoneTimes(DateTimeOffset currentTime)
        {
        // Update the "TimeZoneTime" TMP_Text for all time zones with their respective local time
        foreach (var timeZoneObject in timeZoneObjects)
            {
            // Find the "TimeZoneTime" TMP_Text component in the timeZoneObject
            TMP_Text timeZoneTimeText = timeZoneObject.transform.Find("TimeZoneTime")?.GetComponent<TMP_Text>();
            if (timeZoneTimeText != null)
                {
                // Retrieve the stored TimeZoneInfo from the TimeZoneObject component
                TimeZoneInfo timeZone = timeZoneObject.GetComponent<TimeZoneObject>().TimeZone;
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(currentTime, timeZone);
                timeZoneTimeText.text = localTime.ToString("HH:mm:ss");
                }
            }
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

// Add a new component to store the TimeZoneInfo in each GameObject
public class TimeZoneObject : MonoBehaviour
    {
    public TimeZoneInfo TimeZone { get; set; }
    }
