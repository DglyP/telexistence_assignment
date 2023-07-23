using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using VContainer;

public class StopwatchManager : MonoBehaviour
    {
    [SerializeField] private Button startButton;
    [SerializeField] private Button lapButton;

    private IStopwatch stopwatch;

    [Inject]
    public void Construct(IStopwatch stopwatch)
        {
        this.stopwatch = stopwatch;
        }

    private void Start()
        {
        startButton.onClick.AddListener(StartStopwatch);
        lapButton.onClick.AddListener(LapStopwatch);
        }

    public void StartStopwatch()
        {
        if (!stopwatch.IsRunning.Value)
            {
            stopwatch.StartStopwatch();
            startButton.GetComponentInChildren<TMP_Text>().text = "Pause";
            lapButton.interactable = true;
            lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
            }
        else
            {
            stopwatch.PauseStopwatch();
            startButton.GetComponentInChildren<TMP_Text>().text = "Resume";
            lapButton.GetComponentInChildren<TMP_Text>().text = "Reset";
            }
        }
    public void ResetStopwatch()
        {

        // Reset button states
        startButton.GetComponentInChildren<TMP_Text>().text = "Start";
        lapButton.interactable = false;
        lapButton.GetComponentInChildren<TMP_Text>().text = "Lap";
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
    }
