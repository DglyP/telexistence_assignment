using System;
using NUnit.Framework;
using UniRx;
using UnityEngine;

public class CountdownTests
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
    public void Countdown_StartCountdown_EmitsRemainingTime()
        {
        // Arrange
        TimeSpan duration = TimeSpan.FromSeconds(5);
        TimeSpan expectedRemainingTime = duration;

        // Act
        countdown.StartCountdown(duration);

        // Assert
        // Check if the total elapsed time is zero
        countdown.RemainingTime.Subscribe(remainingTime =>
        {
            Debug.Log(remainingTime.ToString());  
            Assert.AreEqual(expectedRemainingTime, remainingTime);
        });
        }

    [Test]
    public void Countdown_StartCountdown_EmitsUpdatedRemainingTime()
        {
        // Arrange
        TimeSpan duration = TimeSpan.FromSeconds(5);
        TimeSpan elapsedTime = TimeSpan.FromSeconds(2);
        TimeSpan expectedRemainingTime = duration - elapsedTime;

        // Act
        countdown.StartCountdown(duration);
        Time.timeScale = 1f; // Reset time scale to 1
        Observable.Timer(TimeSpan.FromSeconds(2)).Subscribe(_ => Time.timeScale = 0f); // Pause for 2 seconds
        Observable.Timer(TimeSpan.FromSeconds(4)).Subscribe(_ => Time.timeScale = 1f); // Resume after 4 seconds

        // Assert
        countdown.RemainingTime.Subscribe(remainingTime =>
        {
            Assert.AreEqual(expectedRemainingTime, remainingTime);
        });
        }

    [Test]
    public void Countdown_ResetCountdown_SetsRemainingTimeToZero()
        {
        // Arrange
        TimeSpan duration = TimeSpan.FromSeconds(5);

        // Act
        countdown.StartCountdown(duration);
        countdown.ResetCountdown();

        // Assert
        countdown.RemainingTime.Subscribe(remainingTime =>
        {
            Assert.AreEqual(TimeSpan.Zero, remainingTime);
        });
        }
    }
