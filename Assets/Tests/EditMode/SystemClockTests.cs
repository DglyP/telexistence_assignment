using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using System.Collections;

public class SystemClockTests
    {
    private SystemClock systemClock;

    [SetUp]
    public void SetUp()
        {
        systemClock = new SystemClock();
        systemClock.SelectedTimeZone = TimeZoneInfo.Local;
        }

    [TearDown]
    public void TearDown()
        {
        systemClock = null;
        }

    [UnityTest]
    public IEnumerator TimerClock_CurrentTime_DistinctUntilChanged()
        {
        // Arrange: Set up your test environment here
        DateTimeOffset initialTime = DateTimeOffset.Now;

        // Act: Perform actions to test the behavior
        DateTimeOffset resultTime = DateTimeOffset.MinValue;
        systemClock.CurrentTime.Subscribe(time => resultTime = time);

        // Wait for a short time (a single frame) to allow the CurrentTime observable to emit a value
        yield return null;

        // Assert: Make assertions about the expected outcome
        Assert.AreEqual(initialTime, resultTime);
        }

    [UnityTest]
    public IEnumerator TimerClock_MyTime_DistinctUntilChanged()
        {
        // Arrange: Set up your test environment here
        DateTimeOffset initialTime = DateTimeOffset.Now;

        // Act: Perform actions to test the behavior
        DateTimeOffset resultTime = DateTimeOffset.MinValue;
        systemClock.MyTime.Subscribe(time => resultTime = time);

        // Wait for a short time (a single frame) to allow the MyTime observable to emit a value
        yield return null;

        // Assert: Make assertions about the expected outcome
        Assert.AreEqual(initialTime, resultTime);
        }
    }
