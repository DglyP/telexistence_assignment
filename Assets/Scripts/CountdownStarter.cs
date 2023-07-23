using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class CountdownStarter : MonoBehaviour
    {
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownDurationInput;
    [SerializeField] private Button startButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip finishSound;
    [SerializeField] private GameObject timerbuttonPrefab; 
    [SerializeField] private Transform timerbuttonParent;

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

        countdown.RemainingTime
            .Subscribe(time => countdownText.text = FormatTimeSpan(time))
            .AddTo(this);

        startButton.onClick.AddListener(() => StartCountdown(TimeSpan.FromSeconds(countdownDurationInput)));
        resetButton.onClick.AddListener(ResetCountdown);
        pauseButton.onClick.AddListener(TogglePauseCountdown);


        CreateButtons(20, 5, 5);

        //countdown.RemainingTime.Subscribe(OnRemainingTimeChanged).AddTo(this);
        countdown.CountdownDone.Subscribe(isDone =>
        {
            if (isDone)
                {
                countdown.ResetCountdown();
                Debug.Log("Countdown is done!");
                PlayFinishSound();
                ResetCountdown();
                // Perform any actions you want when the countdown is done.
                }
        });
        }

    private void PlayFinishSound()
        {
        if (audioSource != null && finishSound != null)
            {
            countdown.StopCountdown();
            audioSource.PlayOneShot(finishSound);
            }
        }

    public void StartCountdown(TimeSpan duration)
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

    public void StopCountdown()
        {
        isCountdownRunning = false;
        isCountdownPaused = false;
        countdown.ResetCountdown();
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
        pauseButton.GetComponent<Image>().color = Color.red;
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
    private string FormatTimeSpan(TimeSpan timeSpan)
        {
        return string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        }

    }
