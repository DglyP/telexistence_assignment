using UnityEngine;
using TMPro;
using System;
using System.Linq;
using VContainer;

public class TimeZoneSelector : MonoBehaviour
    {
    [SerializeField] private TMP_Dropdown dropdown;
    private ISystemClock systemClock;

    [Inject]
    public void Construct(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        }

    private void Start()
        {
        // Initialize the dropdown options with available timezones
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        dropdown.ClearOptions();
        dropdown.AddOptions(timeZones.Select(tz => tz.DisplayName).ToList());

        // Set the default selected timezone
        systemClock.SelectedTimeZone = timeZones.FirstOrDefault();

        // Subscribe to dropdown value changes and update the selected timezone
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        }

    private void OnDropdownValueChanged(int index)
        {
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        if (index >= 0 && index < timeZones.Count)
            {
            systemClock.SelectedTimeZone = timeZones[index];
            }
        }
    }
