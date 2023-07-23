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
        resetButton.onClick.AddListener(ResetCountdown);
        pauseButton.onClick.AddListener(TogglePauseCountdown);
        stopButton.onClick.AddListener(StopCountdown);
        }

    public void StartCountdown()
        {
        Debug.Log("StartCountdown");
        if (!isCountdownRunning)
            {
            if (isCountdownPaused)
                {
                countdown.ResumeCountdown();
                }
            else
                {
                countdown.StartCountdown(TimeSpan.FromSeconds(countdownDurationInput));
                startButton.gameObject.SetActive(false);
                resetButton?.gameObject.SetActive(true);
                pauseButton?.gameObject.SetActive(true);
                }

            isCountdownRunning = true;
            isCountdownPaused = false;
            }
        }

    public void StopCountdown()
        {
        Debug.Log("StopCountdown");
        countdown.StopCountdown();
        isCountdownRunning = false;
        isCountdownPaused = false;
        }

    public void ResetCountdown()
        {
        Debug.Log("ResetCountdown");
        countdown.ResetCountdown();
        startButton.gameObject.SetActive(true);
        pauseButton?.gameObject.SetActive(false);
        resetButton?.gameObject.SetActive(false);
        stopButton?.gameObject.SetActive(false);
        isCountdownRunning = false;
        isCountdownPaused = false;
        }

    public void TogglePauseCountdown()
        {
        Debug.Log("TogglePauseCountdown");
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
        Debug.Log("SetCountdownDuration: " + duration);
        countdownDurationInput = duration;
        }

    public void OnCountdownDurationInputValueChanged(string value)
        {
        if (float.TryParse(value, out float duration))
            {
            Debug.Log("OnCountdownDurationInputValueChanged: " + duration);
            countdownDurationInput = duration;
            }
        }

    public void PlayFinishSound()
        {
        Debug.Log("PlayFinishSound");
        audioSource.PlayOneShot(finishSound);
        }
    }
