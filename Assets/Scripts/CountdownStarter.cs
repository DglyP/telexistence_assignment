using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CountdownStarter : MonoBehaviour
    {
    // Reference to the countdown text
    [SerializeField] private TMP_Text countdownText;

    // Input field for countdown duration
    [SerializeField] private float countdownDurationInput;

    // Buttons for controlling the countdown
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button pauseButton;

    // Audio source and clip for finish sound
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip finishSound;

    // Prefab and parent transform for timer buttons
    [SerializeField] private GameObject timerbuttonPrefab;
    [SerializeField] private Transform timerbuttonParent;

    // Reference to the countdown implementation
    private ICountdown countdown;

    // Reference to the system clock
    private ISystemClock systemClock;

    // Flags to track countdown status
    public bool isCountdownRunning = false;
    public bool isCountdownPaused = false;

    [Inject]
    public void Construct(ICountdown countdown, ISystemClock systemClock)
        {
        this.countdown = countdown;
        this.systemClock = systemClock;
        }

    private void Start()
        {
        // Subscribe to the system clock time and update the "CurrentTime" UI element
        systemClock.MyTime
            .Subscribe(time => transform.Find("CurrentTime").gameObject.GetComponentInChildren<TMP_Text>().text = time.ToString("HH:mm:ss"))
            .AddTo(this);

        // Subscribe to the countdown remaining time and update the countdown text
        countdown.RemainingTime
            .Subscribe(time => countdownText.text = FormatTimeSpan(time))
            .AddTo(this);

        // Bind button click events to corresponding methods
        startButton.onClick.AddListener(() => StartCountdown(TimeSpan.FromSeconds(countdownDurationInput)));
        resetButton.onClick.AddListener(ResetCountdown);
        pauseButton.onClick.AddListener(TogglePauseCountdown);

        // Create timer buttons and associate them with different countdown durations
        CreateButtons(20, 5, 5);

        // Subscribe to the countdown done event and perform actions when countdown is finished
        countdown.CountdownDone.Subscribe(isDone =>
        {
            if (isDone)
                {
                countdown.ResetCountdown();
                PlayFinishSound();
                ResetCountdown();
                // Perform any actions you want when the countdown is done.
                }
        });
        }

    // Play the finish sound using the attached audio source
    private void PlayFinishSound()
        {
        if (audioSource != null && finishSound != null)
            {
            countdown.StopCountdown();
            audioSource.PlayOneShot(finishSound);
            }
        }

    // Start the countdown with the given duration
    public void StartCountdown(TimeSpan duration)
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
                countdown.StartCountdown(duration);
                startButton.gameObject.SetActive(false);

                resetButton?.gameObject.SetActive(true);
                resetButton.GetComponent<Image>().color = new Color(0.18f, 0.18f, 0.18f);
                resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";

                pauseButton?.gameObject.SetActive(true);
                pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
                pauseButton.GetComponent<Image>().color = Color.red;
                }

            isCountdownRunning = true;
            isCountdownPaused = false;
            }
        }

    // Stop the countdown and reset the flags
    public void StopCountdown()
        {
        isCountdownRunning = false;
        isCountdownPaused = false;
        countdown.ResetCountdown();
        }

    // Reset the countdown and UI elements
    public void ResetCountdown()
        {
        isCountdownRunning = false;
        isCountdownPaused = false;
        countdown.ResetCountdown();
        startButton.gameObject.SetActive(true);
        pauseButton?.gameObject.SetActive(false);
        resetButton?.gameObject.SetActive(false);
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
        pauseButton.GetComponent<Image>().color = Color.red;
        }

    // Toggle pause/resume of the countdown
    public void TogglePauseCountdown()
        {
        if (isCountdownRunning)
            {
            if (isCountdownPaused)
                {
                countdown.ResumeCountdown();
                pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
                pauseButton.GetComponent<Image>().color = Color.red;
                }
            else
                {
                countdown.PauseCountdown();
                pauseButton.GetComponent<Image>().color = new Color(0f, 0.466f, 0.133f);
                pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = "Resume";
                resetButton.GetComponent<Image>().color = new Color(0.18f, 0.18f, 0.18f);
                resetButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cancel";
                }
            isCountdownPaused = !isCountdownPaused;
            }
        }

    // Set the countdown duration from the input field
    public void SetCountdownDuration(float duration)
        {
        countdownDurationInput = duration;
        }

    // Update the countdown duration from the input field value
    public void OnCountdownDurationInputValueChanged(string value)
        {
        if (float.TryParse(value, out float duration))
            {
            countdownDurationInput = duration;
            }
        }

    // Helper method to create timer buttons with different countdown durations
    private void CreateButtons(int numberOfButtons, int startingValue, int step)
        {
        // Instantiate buttons
        for (int i = 0; i < numberOfButtons; i++)
            {
            // Calculate the value for the current button in TimeSpan format
            int buttonValue = startingValue + (i * step);
            TimeSpan timeSpanValue = TimeSpan.FromSeconds(buttonValue);

            // Create a new button
            GameObject newButton = Instantiate(timerbuttonPrefab, timerbuttonParent);
            newButton.SetActive(true);

            // Set the button name using the calculated value in TimeSpan format
            newButton.GetComponentInChildren<TMP_Text>().text = FormatTimeSpan(timeSpanValue);

            // Add button click listener to start the countdown
            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                // Start the countdown with the corresponding button value
                StartCountdown(TimeSpan.FromSeconds(buttonValue));
            });
            }
        }

    // Helper method to format the TimeSpan into a string
    private string FormatTimeSpan(TimeSpan timeSpan)
        {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }
    }
