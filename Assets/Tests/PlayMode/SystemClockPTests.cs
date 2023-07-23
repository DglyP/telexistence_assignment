using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UniRx;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SystemClockTests
    {
    private ISystemClock systemClock;

    [UnitySetUp]
    public IEnumerator SetUp()
        {
        systemClock = new SystemClock();
        yield break;
        }

    [UnityTearDown]
    public IEnumerator TearDown()
        {
        systemClock = null;
        yield break;
        }

    [Test]
    public async Task Able_to_Create_Clock()
        {
        var countdownTime = MakeTimerAsync(1);

        var tasks = new List<Task> { countdownTime };
        while (tasks.Count > 0)
            {
            Task finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            }
        }

    private static async Task<Timer> MakeTimerAsync(int howMany)
        {
        SystemClock systemClock= new SystemClock();
        await Task.Delay(3000);

        return new Timer();
        }
    public struct Timer
        {

        }

    [UnityTest]
    public IEnumerator System_Clock_MyTime_Test()
        {
        DateTimeOffset initialTime = DateTimeOffset.Now;
        DateTimeOffset resultTime = DateTimeOffset.MinValue;

        systemClock.MyTime.Subscribe(time => resultTime = time);

        yield return new WaitForSeconds(1.0f);

        Assert.AreNotEqual(initialTime, resultTime, "The system time should change when MyTime event is triggered.");
        }

    [UnityTest]
    public IEnumerator System_Clock_CurrentTime_Test()
        {
        DateTimeOffset initialTime = DateTimeOffset.Now;
        DateTimeOffset resultTime = DateTimeOffset.MinValue;

        TimeZoneInfo randomTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        systemClock.SelectedTimeZone = randomTimeZone;

        systemClock.CurrentTime.Subscribe(time => resultTime = time);

        yield return new WaitForSeconds(1.0f);


        Assert.AreNotEqual(initialTime, resultTime, "The system time should change when MyTime event is triggered.");
        }
    }
