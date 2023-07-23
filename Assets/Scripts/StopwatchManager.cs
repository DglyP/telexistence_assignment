using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VContainer;
using UniRx;
using System.Collections.Generic;

public class StopwatchManager : MonoBehaviour
    {
    // References to UI elements
    [SerializeField] private TMP_Text stopwatchText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button lapButton;
    [SerializeField] private TMP_Text lapTimestampText;
    [SerializeField] private GameObject lapGameObject; // Reference to the TMP_Text prefab
    [SerializeField] private Transform lapTextParent; // Parent transform to hold the lap text objects

    // Variables to manage stopwatch state
    private bool isPaused;
    private int lapNumber = 0;
    private List<TMP_Text> lapTexts = new List<TMP_Text>(); // List to keep track of lap text objects
    private Stopwatch.StopwatchLapData lastLapData;

    // Dependencies
    private IStopwatch stopwatch;
    private ISystemClock systemClock;

    [Inject]
    public void Construct(IStopwatch stopwatch, ISystemClock systemClock)
        {
        this.stopwatch = stopwatch;
        this.systemClock = systemClock;
        }

    private void Start()
        {
        // Button event listeners
        startButton.onClick.AddListener(StartStopwatch);
        lapButton.onClick.AddListener(LapStopwatch);

        // Subscriptions to stopwatch observables
        systemClock.MyTime
            .Subscribe(time => transform.Find("CurrentTime").gameObject.GetComponentInChildren<TMP_Text>().text = time.ToString("HH:mm:ss"))
            .AddTo(this);

        stopwatch.LapData
            .Subscribe(lapData =>
            {
                CreateLapText(FormatTimeSpan(lapData.ElapsedTime));
                lapTimestampText.text = FormatTimeSpan(lapData.ElapsedTime);
                lastLapData = lapData; // Store the latest lap data
            })
            .AddTo(this);

        stopwatch.ElapsedTime
            .Subscribe(time => stopwatchText.text = FormatTimeSpan(time))
            .AddTo(this);
        }

    // Starts, pauses, or resumes the stopwatch based on its current state.
    public void StartStopwatch()
        {
        if (!stopwatch.IsRunning.Value && !isPaused)
            {
            // Start the stopwatch if it is not running and not paused.
            startButton.GetComponentInChildren<TMP_Text>().text = "Stop";
            startButton.GetComponent<Image>().color = Color.red;
            lapButton.interactable = true;
            lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
            lapButton.GetComponentInChildren<TMP_Text>().color = Color.white;
            lapButton.GetComponent<Image>().color = new Color(0.18f, 0.18f, 0.18f);
            stopwatch.StartStopwatch();
            }
        else if (!stopwatch.IsRunning.Value && isPaused)
            {
            // Resume the stopwatch if it is not running but paused.
            startButton.GetComponentInChildren<TMP_Text>().text = "Stop";
            lapButton.interactable = true;
            isPaused = false;
            lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
            stopwatch.ResumeStopwatch();
            }
        else
            {
            // Pause the stopwatch if it is running.
            stopwatch.PauseStopwatch();
            startButton.GetComponentInChildren<TMP_Text>().text = "Resume";
            startButton.GetComponent<Image>().color = new Color(0f, 0.466f, 0.133f);
            lapButton.GetComponentInChildren<TMP_Text>().text = "Reset";
            isPaused = true;
            }
        }

    // Resets the stopwatch and clears lap text objects.
    public void ResetStopwatch()
        {
        // Clear lap text objects from the scene
        foreach (var lapText in lapTexts)
            {
            Destroy(lapText.gameObject);
            }
        lapTexts.Clear();

        // Reset lap number
        lapNumber = 0;

        // Reset button states
        startButton.GetComponentInChildren<TMP_Text>().text = "Start";
        lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
        lapButton.interactable = false;
        isPaused = false;
        stopwatch.ResetStopwatch();

        // Reset last lap data
        lastLapData = null;
        }

    // Records a lap in the stopwatch or resets the stopwatch if it is paused.
    public void LapStopwatch()
        {
        if (stopwatch.IsRunning.Value)
            {
            // Record a lap if the stopwatch is running.
            stopwatch.Lap();
            }
        else
            {
            // Reset the stopwatch if it is paused.
            ResetStopwatch();
            }
        }

    // Creates and displays lap text with lap number and timestamp.
    private void CreateLapText(string lapTime)
        {
        // Increment the lap number for each new lap
        lapNumber++;

        // Create the lap text with lap number and timestamp
        string lapTextContent = $"{lapNumber}: {lapTime}";

        GameObject lapObject = Instantiate(lapGameObject, lapTextParent);

        // Instantiate the lap text prefab and set its text
        TMP_Text lapNumberText = lapObject.transform.Find("LapNumber")?.GetComponent<TMP_Text>();
        TMP_Text lapLabelText = lapObject.transform.Find("LapLabel")?.GetComponent<TMP_Text>();
        lapNumberText.text = $"{lapNumber}";
        lapLabelText.text = lapTime;
        lapNumberText.gameObject.SetActive(true);
        lapLabelText.gameObject.SetActive(true);

        // Compare the value of the last lap with the current lap
        if (lastLapData != null)
            {
            var currentLapTime = TimeSpan.ParseExact(lapTime, "hh\\:mm\\:ss\\:ff", null);
            var lastLapTime = lastLapData.ElapsedTime;

            // Change the color of the lap text to green if the current lap is shorter than the last lap.
            if (currentLapTime < lastLapTime)
                {
                lapLabelText.color = Color.green;
                }
            }

        // Add the lap text to the list
        lapTexts.Add(lapNumberText);
        lapTexts.Add(lapLabelText);
        lapObject.gameObject.SetActive(true);

        // Adjust the position of the lap texts (you may need to customize this based on your layout)
        for (int i = 0; i < lapTexts.Count; i++)
            {
            lapTexts[i].rectTransform.anchoredPosition = new Vector2(0f, -30f * i);
            }
        }

    // Formats a TimeSpan into a string with the format "HH:mm:ss:ff".
    private string FormatTimeSpan(TimeSpan timeSpan)
        {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }
    }
