using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using UniRx;
using System.Threading.Tasks;
using System.Collections.Generic;
using UniRx.Triggers;
using System.Collections;

public class TimerTests
    {
    private Countdown countdown;

    [SetUp]
    public void SetUp()
        {
        countdown = new Countdown();
        }

    [TearDown]
    public void TearDown()
        {
        countdown.StopCountdown();
        countdown = null;
        }

    [Test]
    public void Countdown_StartTimer()
        {
        countdown.StartCountdown(TimeSpan.FromSeconds(10));

        Assert.IsTrue(countdown.IsRunning.Value);
        }

    [Test]
    public void Countdown_StopTimer()
        {
        countdown.StartCountdown(TimeSpan.FromSeconds(10));
        countdown.StopCountdown();

        Assert.IsFalse(countdown.IsRunning.Value);
        }

    [Test]
    public void Countdown_ResetTimer()
        { 
        TimeSpan elapsedTimeAfterReset = TimeSpan.Zero;
        countdown.StartCountdown(TimeSpan.FromSeconds(10));
        countdown.StopCountdown();

        // Check if the stopwatch is not running
        Assert.IsFalse(countdown.IsRunning.Value);
        }

    }
