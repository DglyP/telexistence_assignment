using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
public class SystemClockTests
    {
    private SystemClock systemClock;
    private List<TimeZoneInfo> timeZones;
    private TimeZoneInfo selectedTimeZone;

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

    //[Test]
    //public async Task System_Time_is_changing()
    //    {
    //    var timeTask = SystemClockTimeChanges(1);

    //    var breakfastTasks = new List<Task> { timeTask };
    //    while (breakfastTasks.Count > 0)
    //        {
    //        Task finishedTask = await Task.WhenAny(breakfastTasks);
    //        if (finishedTask == timeTask)
    //            {
    //            Debug.Log("time is ready");
    //            }
    //        breakfastTasks.Remove(finishedTask);
    //        }
    //    }

    //public async Task<MyTime> SystemClockTimeChanges(int howmany)
    //    {
    //    DateTimeOffset initialTime = DateTimeOffset.Now;
    //    DateTimeOffset resultTime = DateTimeOffset.MinValue;
    //    systemClock.MyTime
    //        .Subscribe(time => resultTime = time);
    //    await Task.Delay(3000);
    //    Debug.Log(resultTime);

    //    Debug.Log(initialTime);

    //    Assert.AreEqual(initialTime, resultTime);
    //    return new MyTime();
    //    }
    //public struct MyTime
    //    {

    //    }

    [Test]
    public async Task System_Clock_MyTime_Test()
        {
        // Arrange
        DateTimeOffset initialTime = DateTimeOffset.Now;
        DateTimeOffset resultTime = DateTimeOffset.MinValue;

        // Act: Subscribe to the MyTime event and set the resultTime when it's triggered
        systemClock.MyTime.Subscribe(time => resultTime = time);

        // Wait for a short delay (you can adjust the delay as needed for your test)
        await Task.Delay(1000);

        // Assert: Check if resultTime has been updated after subscribing to MyTime
        Assert.AreNotEqual(initialTime, resultTime, "The system time should change when MyTime event is triggered.");
        }


    [Test]
    public async Task System_Clock_CurrentTime_Test()
        {
        // Arrange
        DateTimeOffset initialTime = DateTimeOffset.Now;
        DateTimeOffset resultTime = DateTimeOffset.MinValue;

        // Get a random time zone (you can replace this with any valid TimeZoneInfo object)
        TimeZoneInfo randomTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        // Set the selected time zone
        systemClock.SelectedTimeZone = randomTimeZone;

        // Act: Subscribe to the MyTime event and set the resultTime when it's triggered
        systemClock.CurrentTime.Subscribe(time => resultTime = time);

        // Wait for a short delay (you can adjust the delay as needed for your test)
        await Task.Delay(1000);

        // Assert: Check if resultTime has been updated after subscribing to MyTime
        Assert.AreNotEqual(initialTime, resultTime, "The system time should change when MyTime event is triggered.");
        }

    }
