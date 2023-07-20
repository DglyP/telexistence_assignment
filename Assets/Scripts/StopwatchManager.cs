using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StopwatchManager : MonoBehaviour
    {
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton; 
    [SerializeField] private Button resetButton; 
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
        stopButton.onClick.AddListener(StopStopwatch);
        resetButton.onClick.AddListener(ResetStopwatch);
        lapButton.onClick.AddListener(LapStopwatch);
        }
    public void StartStopwatch()
        {
        stopwatch.StartStopwatch();
        }

    public void StopStopwatch()
        {
        stopwatch.StopStopwatch();
        }

    public void ResetStopwatch()
        {
        stopwatch.ResetStopwatch();
        }
    public void LapStopwatch()
        {
        stopwatch.Lap();
        }
    }