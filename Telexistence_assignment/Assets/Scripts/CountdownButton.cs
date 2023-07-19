using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using VContainer;

public class CountdownButton : MonoBehaviour
    {
    [SerializeField] private int countdownDuration = 10;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private Button button;

    private bool isCountingDown = false;
    private float countdownStartTime;

    public bool IsCountingDown => isCountingDown;
    public int CountdownDuration => countdownDuration;
    public float CountdownStartTime => countdownStartTime;

    private ISystemClock systemClock;

    [Inject]
    public void Construct(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        }

    private void Start()
        {
        button.onClick.AddListener(StartCountdown);
        }

    private void Update()
        {
        if (isCountingDown)
            {
            float elapsedTime = Time.time - countdownStartTime;
            float remainingTime = countdownDuration - elapsedTime;
            if (remainingTime <= 0f)
                {
                isCountingDown = false;
                countdownText.text = "Countdown Finished";
                // Additional actions when countdown finishes
                }
            else
                {
                countdownText.text = FormatTime(remainingTime);
                }
            }
        }

    private void StartCountdown()
        {
        isCountingDown = true;
        countdownStartTime = Time.time;
        }

    private string FormatTime(float time)
        {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
