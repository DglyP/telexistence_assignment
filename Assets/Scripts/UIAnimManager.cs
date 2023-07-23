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
        clockPanel.DOAnchorPos(Vector2.zero, 0.25f);
        clockPanel.gameObject.SetActive(true);
        stopwatchPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(false);
    }

    public void clockButton()
        {
        clockPanel.DOAnchorPos(new Vector2(0, 0), 0.25f);
        stopwatchPanel.DOAnchorPos(new Vector2(1200, 0), 0.25f);
        timerPanel.DOAnchorPos(new Vector2(2400, 0), 0.25f);
        clockPanel.gameObject.SetActive(true);
        stopwatchPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(false);
        }

    public void stopWatchButton()
        {
        clockPanel.DOAnchorPos(new Vector2(-1200, 0), 0.25f);
        stopwatchPanel.DOAnchorPos(new Vector2(0, 0), 0.25f);
        timerPanel.DOAnchorPos(new Vector2(2400, 0), 0.25f);
        clockPanel.gameObject.SetActive(false);
        stopwatchPanel.gameObject.SetActive(true);
        timerPanel.gameObject.SetActive(false);
        }

    public void timerButton()
        {
        clockPanel.DOAnchorPos(new Vector2(-2400, 0), 0.25f);
        stopwatchPanel.DOAnchorPos(new Vector2(-1200, 0), 0.25f);
        timerPanel.DOAnchorPos(new Vector2(0, 0), 0.25f);
        clockPanel.gameObject.SetActive(false);
        stopwatchPanel.gameObject.SetActive(false);
        timerPanel.gameObject.SetActive(true);
        }
    }
