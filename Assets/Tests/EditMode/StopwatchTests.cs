using System;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using UniRx.Triggers;
using NUnit.Framework;
using System.Collections;

public class StopwatchTests
    {
    private Stopwatch stopwatch;

    [SetUp]
    public void SetUp()
        {
        stopwatch = new Stopwatch();
        }

    [TearDown]
    public void TearDown()
        {
        stopwatch.StopStopwatch();
        stopwatch = null;
        }

    [Test]
    public void Stopwatch_StartStopwatch()
        {
        stopwatch.StartStopwatch();

        Assert.IsTrue(stopwatch.IsRunning.Value);
        }

    [Test]
    public void Stopwatch_StopStopwatch()
        {
        stopwatch.StartStopwatch();
        stopwatch.StopStopwatch();

        Assert.IsFalse(stopwatch.IsRunning.Value);
        }

    [Test]
    public void Stopwatch_LapDataEmitsMultipleValues()
        {
        // Start the stopwatch
        stopwatch.StartStopwatch();

        // Wait for some time to pass (for example, 2 seconds)
        System.Threading.Thread.Sleep(2000);

        int lapDataCount = 0;
        IDisposable subscription = stopwatch.LapData.Subscribe(_ => lapDataCount++);

        // Perform multiple laps
        stopwatch.Lap();
        stopwatch.Lap();
        stopwatch.Lap();

        // Wait for a moment to handle asynchronous events
        System.Threading.Thread.Sleep(100);

        subscription.Dispose();

        // Check if LapData emitted the correct number of values
        Assert.AreEqual(3, lapDataCount);
        }

    [Test]
    public void Stopwatch_ResetStopwatch()
        {
        // Start the stopwatch
        stopwatch.StartStopwatch();

        // Wait for some time to pass (for example, 1 second)
        System.Threading.Thread.Sleep(1000);

        // Reset the stopwatch
        stopwatch.ResetStopwatch();

        // Check if the stopwatch is not running
        Assert.IsFalse(stopwatch.IsRunning.Value);

        // Check if the total elapsed time is zero
        TimeSpan elapsedTimeAfterReset = TimeSpan.Zero;
        stopwatch.ElapsedTime.Subscribe(time => elapsedTimeAfterReset = time);

        Assert.AreEqual(TimeSpan.Zero, elapsedTimeAfterReset);
        }

    }
