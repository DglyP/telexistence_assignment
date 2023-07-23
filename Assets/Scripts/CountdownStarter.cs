using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CountdownStarter : MonoBehaviour
    {
    [SerializeField] private float countdownDurationInput;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip finishSound;

    private ICountdown countdown;
    private ISystemClock systemClock;
    private bool isCountdownRunning = false;
    private bool isCountdownPaused = false;

    [Inject]
    public void Construct(ICountdown countdown, ISystemClock systemClock)
        {
        this.countdown = countdown;
        this.systemClock = systemClock;
        }

    private void Start()
        {
        systemClock.MyTime
            .Subscribe(time => transform.Find("CurrentTime").gameObject.GetComponentInChildren<TMP_Text>().text = time.ToString("HH:mm:ss"))
            .AddTo(this);

        startButton.onClick.AddListener(StartCountdown);
        resetButton.onClick.AddListener(ResetCountdown);
        pauseButton.onClick.AddListener(TogglePauseCountdown);

        countdown.RemainingTime.Subscribe(OnRemainingTimeChanged).AddTo(this);
        }

    private void OnRemainingTimeChanged(TimeSpan remainingTime)
        {
        // When the remaining time reaches zero, play the finish sound
        if (remainingTime <= TimeSpan.Zero && isCountdownRunning)
            {
            StopCountdown(); // Optional: Stop the countdown when it finishes
            }
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
        countdown.ResetCountdown();
        isCountdownRunning = false;
        isCountdownPaused = false;
        }

    public void ResetCountdown()
        {
        Debug.Log("ResetCountdown");
        isCountdownRunning = false;
        isCountdownPaused = false;
        countdown.ResetCountdown();
        startButton.gameObject.SetActive(true);
        pauseButton?.gameObject.SetActive(false);
        resetButton?.gameObject.SetActive(false);
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
        }

    public void TogglePauseCountdown()
        {
        Debug.Log("TogglePauseCountdown");
        if (isCountdownRunning)
            {
            if (isCountdownPaused)
                {
                countdown.ResumeCountdown();
                pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
                }
            else
                {
                countdown.PauseCountdown();
                pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
                resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";
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

    }
