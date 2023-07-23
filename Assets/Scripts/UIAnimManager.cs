using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIAnimManager : MonoBehaviour
    {
    public RectTransform clockPanel, stopwatchPanel, timerPanel;

    // Start is called before the first frame update
    void Start()
        {
        // Initialize the UI panels' positions
        clockPanel.anchoredPosition = Vector2.zero;
        stopwatchPanel.anchoredPosition = new Vector2(1200, 0);
        timerPanel.anchoredPosition = new Vector2(2400, 0);

        // Activate the clock panel and deactivate the others
        clockPanel.gameObject.SetActive(true);
        stopwatchPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(false);
        }

    public void clockButton()
        {
        // Animate the panels to their respective positions
        clockPanel.DOAnchorPos(Vector2.zero, 0.25f);
        stopwatchPanel.DOAnchorPos(new Vector2(1200, 0), 0.25f);
        timerPanel.DOAnchorPos(new Vector2(2400, 0), 0.25f);

        // Activate the clock panel and deactivate the others
        clockPanel.gameObject.SetActive(true);
        stopwatchPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(false);
        }

    public void stopWatchButton()
        {
        // Animate the panels to their respective positions
        clockPanel.DOAnchorPos(new Vector2(-1200, 0), 0.25f);
        stopwatchPanel.DOAnchorPos(Vector2.zero, 0.25f);
        timerPanel.DOAnchorPos(new Vector2(2400, 0), 0.25f);

        // Activate the stopwatch panel and deactivate the others
        clockPanel.gameObject.SetActive(false);
        stopwatchPanel.gameObject.SetActive(true);
        timerPanel.gameObject.SetActive(false);
        }

    public void timerButton()
        {
        // Animate the panels to their respective positions
        clockPanel.DOAnchorPos(new Vector2(-2400, 0), 0.25f);
        stopwatchPanel.DOAnchorPos(new Vector2(-1200, 0), 0.25f);
        timerPanel.DOAnchorPos(Vector2.zero, 0.25f);

        // Activate the timer panel and deactivate the others
        clockPanel.gameObject.SetActive(false);
        stopwatchPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(true);
        }
    }
