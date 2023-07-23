using UnityEngine;
using TMPro;
using System;
using System.Linq;
using VContainer;
using System.Collections.Generic;
using UniRx;

public class SystemClockManager : MonoBehaviour
    {
    [SerializeField] private GameObject clockGameObject;
    [SerializeField] private GameObject timeZonePrefab;
    [SerializeField] private Transform timeZoneParent;

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

        // Subscribe to the MyTime observable to update the "My Time" text
        systemClock.MyTime
            .Subscribe(time => myTimeZone.text = FormatTime(time))
            .AddTo(this);

        // Subscribe to the CurrentTime observable to update the "Current Time Zone" text
        systemClock.CurrentTime
            .Subscribe(time => currentTimeZone.text = FormatTimeZone(time))
            .AddTo(this);

        // Get the list of available time zones and set the selected time zone to the first one in the list
        timeZones = TimeZoneInfo.GetSystemTimeZones().ToList();
        selectedTimeZone = timeZones.FirstOrDefault();
        systemClock.SelectedTimeZone = selectedTimeZone;

        // Create the time zone objects in the UI
        CreateTimeZones(timeZones);

        // Subscribe to the CurrentTime observable using the custom TimeZoneTimeUpdater observer to update the time zone times
        systemClock.CurrentTime
            .Subscribe(new TimeZoneTimeUpdater(this))
            .AddTo(this);
        }

    private void CreateTimeZones(List<TimeZoneInfo> timeZones)
        {
        foreach (var timeZone in timeZones)
            {
            // Instantiate a new time zone object from the prefab
            GameObject newTimeZoneObject = Instantiate(timeZonePrefab, timeZoneParent);

            // Get the TMP_Text components from the time zone object to update their texts
            TMP_Text newTimeZoneText = newTimeZoneObject.transform.Find("TimeZoneLabel")?.GetComponent<TMP_Text>();
            TMP_Text newTimeZoneTime = newTimeZoneObject.transform.Find("TimeZoneTime")?.GetComponent<TMP_Text>();

            // Set the time zone label text to display the standard name of the time zone
            if (newTimeZoneText != null)
                {
                newTimeZoneText.text = timeZone.StandardName;
                newTimeZoneText.gameObject.SetActive(true);
                }

            // Add the TimeZoneObject script component to the time zone object and set the time zone
            TimeZoneObject timeZoneObject = newTimeZoneObject.AddComponent<TimeZoneObject>();
            timeZoneObject.TimeZone = timeZone;

            // Add the time zone object to the list for later reference
            timeZoneObjects.Add(newTimeZoneObject);

            // Activate the time zone object in the UI
            newTimeZoneObject.SetActive(true);
            }
        }

    private class TimeZoneTimeUpdater : IObserver<DateTimeOffset>
        {
        private SystemClockManager timeZoneSelector;

        public TimeZoneTimeUpdater(SystemClockManager timeZoneSelector)
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
        foreach (var timeZoneObject in timeZoneObjects)
            {
            TMP_Text timeZoneTimeText = timeZoneObject.transform.Find("TimeZoneTime")?.GetComponent<TMP_Text>();
            if (timeZoneTimeText != null)
                {
                // Get the time zone from the TimeZoneObject script component
                TimeZoneInfo timeZone = timeZoneObject.GetComponent<TimeZoneObject>().TimeZone;

                // Convert the current time to the local time of the selected time zone
                DateTimeOffset localTime = TimeZoneInfo.ConvertTime(currentTime, timeZone);

                // Update the time zone time text with the formatted local time
                timeZoneTimeText.text = FormatTime(localTime);
                }
            }
        }

    private string FormatTime(DateTimeOffset time)
        {
        return time.ToString("HH:mm:ss");
        }

    private string FormatTimeZone(DateTimeOffset time)
        {
        // Find the time zone that matches the current time's offset
        TimeZoneInfo matchedTimeZone = timeZones.FirstOrDefault(zone => zone.BaseUtcOffset == time.Offset);
        string timeZoneName = matchedTimeZone != null ? matchedTimeZone.StandardName : "Unknown Time Zone";
        return timeZoneName;
        }
    }

public class TimeZoneObject : MonoBehaviour
    {
    // Property to store the time zone information for this object
    public TimeZoneInfo TimeZone { get; set; }
    }
