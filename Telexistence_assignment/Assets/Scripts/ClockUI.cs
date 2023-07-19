using UnityEngine;
using UniRx;
using TMPro;
using VContainer;

public class ClockUI : MonoBehaviour
    {
    [SerializeField] private TMP_Text clockText;
    [SerializeField] private TMP_Text clockText2;
    [SerializeField] private TMP_Text countdownText;

    private ISystemClock systemClock;
    private CountdownButton countdownButton;

    [Inject]
    public void Construct(ISystemClock systemClock, CountdownButton countdownButton)
        {
        this.systemClock = systemClock;
        this.countdownButton = countdownButton;
        }

    private void Start()
        {
        systemClock.CurrentTime
            .Subscribe(time => clockText.text = time.ToString("HH:mm:ss"))
            .AddTo(this);

        systemClock.MyTime
            .Subscribe(time => clockText2.text = time.ToString("HH:mm:ss"))
            .AddTo(this);
        }

    private void Update()
        {
        if (countdownText != null)
            {
            if (countdownButton != null && countdownButton.IsCountingDown)
                {
                float remainingTime = countdownButton.CountdownDuration - (Time.time - countdownButton.CountdownStartTime);
                countdownText.text = FormatTime(remainingTime);
                }
            else
                {
                countdownText.text = "No Countdown";
                }
            }
        }

    private string FormatTime(float time)
        {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
