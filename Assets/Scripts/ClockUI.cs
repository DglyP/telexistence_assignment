using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using VContainer;
using System;
using System.Linq;
using System.Collections.Generic;
using Codice.Client.Common;

public class ClockUI : MonoBehaviour
    {
    [SerializeField] private GameObject clockGameObject;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip finishSound;

    private bool isCountdownRunning = false;

    private ISystemClock systemClock;
    private bool makingSound;

    [Inject]
    public void Construct(ISystemClock systemClock)
        {
        this.systemClock = systemClock;
        }

    private void Start()
        {


        }

    }