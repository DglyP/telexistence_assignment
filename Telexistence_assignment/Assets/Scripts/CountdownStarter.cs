using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CountdownStarter : MonoBehaviour
    {
    [SerializeField] private float countdownDurationInput;
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip finishSound;

    private ICountdown countdown;
    private bool isCountdownRunning = false;
    private bool isCountdownPaused = false;

    [Inject]
    public void Construct(ICountdown countdown)
        {
        this.countdown = countdown;
        }

    private void Start()
        {
        startButton.onClick.AddListener(StartCountdown);
        stopButton.onClick.AddListener(StopCountdown);
        resetButton.onClick.AddListener(ResetCountdown);
        pauseButton.onClick.AddListener(TogglePauseCountdown);
        }

    public void StartCountdown()
        {
        if (!isCountdownRunning)
            {
            if (isCountdownPaused)
                {
                countdown.ResumeCountdown();
                }
            else
                {
                countdown.StartCountdown(TimeSpan.FromSeconds(countdownDurationInput));
                }

            isCountdownRunning = true;
            isCountdownPaused = false;
            }
        }

    public void StopCountdown()
        {
        countdown.StopCountdown();
        isCountdownRunning = false;
        isCountdownPaused = false;
        }

    public void ResetCountdown()
        {
        countdown.ResetCountdown();
        isCountdownRunning = false;
        isCountdownPaused = false;
        }

    public void TogglePauseCountdown()
        {
        if (isCountdownRunning)
            {
            if (isCountdownPaused)
                {
                countdown.ResumeCountdown();
                }
            else
                {
                countdown.PauseCountdown();
                }

            isCountdownPaused = !isCountdownPaused;
            }
        }

    public void SetCountdownDuration(float duration)
        {
        countdownDurationInput = duration;
        }

    public void OnCountdownDurationInputValueChanged(string value)
        {
        if (float.TryParse(value, out float duration))
            {
            countdownDurationInput = duration;
            }
        }

    public void PlayFinishSound()
        {
        audioSource.PlayOneShot(finishSound);
        }
    }
