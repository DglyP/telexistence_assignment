using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VContainer;
using UniRx;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

public class StopwatchManager : MonoBehaviour
    {
    [SerializeField] private Button startButton;
    [SerializeField] private Button lapButton;

    [SerializeField] private TMP_Text lapTimestampText;
    [SerializeField] private TMP_Text lapTextPrefab; // Reference to the TMP_Text prefab
    [SerializeField] private Transform lapTextParent; // Parent transform to hold the lap text objects

    private bool isPaused;
    private int lapNumber = 0;
    private List<TMP_Text> lapTexts = new List<TMP_Text>(); // List to keep track of lap text objects

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
        startButton.onClick.AddListener(StartStopwatch);
        lapButton.onClick.AddListener(LapStopwatch);

        systemClock.MyTime
            .Subscribe(time => transform.Find("CurrentTime").gameObject.GetComponentInChildren<TMP_Text>().text = time.ToString("HH:mm:ss"))
            .AddTo(this);

        stopwatch.LapData
            .Subscribe(lapData => lapTimestampText.text = FormatTimeSpan(lapData.ElapsedTime))
            .AddTo(this);

        stopwatch.LapData
            .Subscribe(lapData => CreateLapText(FormatTimeSpan(lapData.ElapsedTime)))
            .AddTo(this);
        }

    public void StartStopwatch()
        {
        if (!stopwatch.IsRunning.Value && !isPaused)
            {
            startButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            lapButton.interactable = true;
            lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
            stopwatch.StartStopwatch();
            }
        else if (!stopwatch.IsRunning.Value && isPaused)
            {
            startButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            lapButton.interactable = true;
            isPaused = false;
            lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
            stopwatch.ResumeStopwatch();
            }
        else
            {
            stopwatch.PauseStopwatch();
            startButton.GetComponentInChildren<TMP_Text>().text = "Resume";
            lapButton.GetComponentInChildren<TMP_Text>().text = "Reset";
            isPaused = true;
            }
        }


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
        }

    public void LapStopwatch()
        {
        if (stopwatch.IsRunning.Value)
            {
            stopwatch.Lap();
            }
        else
            {
            ResetStopwatch();
            }
        }

    private void CreateLapText(string lapTime)
        {
        // Increment the lap number for each new lap
        lapNumber++;

        // Create the lap text with lap number and timestamp
        string lapTextContent = $"Lap Number {lapNumber}: {lapTime}";

        // Instantiate the lap text prefab and set its text
        TMP_Text lapText = Instantiate(lapTextPrefab, lapTextParent);
        lapText.text = lapTextContent;
        lapText.gameObject.SetActive(true);

        // Add the lap text to the list
        lapTexts.Add(lapText);

        // Adjust the position of the lap texts (you may need to customize this based on your layout)
        for (int i = 0; i < lapTexts.Count; i++)
            {
            lapTexts[i].rectTransform.anchoredPosition = new Vector2(0f, -30f * i);
            }
        }

    private string FormatTimeSpan(TimeSpan timeSpan)
        {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }
    }
