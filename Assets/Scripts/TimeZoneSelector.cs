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
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        dropdown.ClearOptions();
        dropdown.AddOptions(timeZones.Select(tz => tz.DisplayName).ToList());

        systemClock.SelectedTimeZone = timeZones.FirstOrDefault();

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
