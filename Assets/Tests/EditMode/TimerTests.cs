using System;
using NUnit.Framework;
using UniRx;

public class TimerTests
    {
    private TimerClock timerClock;

    [SetUp]
    public void SetUp()
        {
        timerClock = new TimerClock();
        }

    [Test]
    public void TimerClock_CurrentTime_NotNull()
        {
        // Arrange & Act
        var currentTime = timerClock.CurrentTime;

        // Assert
        Assert.NotNull(currentTime);
        }

    [Test]
    public void TimerClock_CurrentTime_DistinctUntilChanged()
        {
        // Arrange & Act
        var currentTime = timerClock.CurrentTime;

        // Assert
        bool valuesAreDistinct = true;
        currentTime.Buffer(2, 1).Subscribe(buffer =>
        {
            if (buffer.Count == 2 && buffer[0] == buffer[1])
                valuesAreDistinct = false;
        });

        Assert.IsTrue(valuesAreDistinct);
        }

    [Test]
    public void TimerClock_MyTime_NotNull()
        {
        // Arrange & Act
        var myTime = timerClock.MyTime;

        // Assert
        Assert.NotNull(myTime);
        }

    [Test]
    public void TimerClock_MyTime_DistinctUntilChanged()
        {
        // Arrange & Act
        var myTime = timerClock.MyTime;

        // Assert
        bool valuesAreDistinct = true;
        myTime.Buffer(2, 1).Subscribe(buffer =>
        {
            if (buffer.Count == 2 && buffer[0] == buffer[1])
                valuesAreDistinct = false;
        });

        Assert.IsTrue(valuesAreDistinct);
        }

    [Test]
    public void TimerClock_StartTimer_TimerTicks()
        {
        // Arrange & Act
        bool timerTicked = false;
        timerClock.Timer.Subscribe(_ => timerTicked = true);
        timerClock.StartTimer();

        // Assert
        Assert.IsTrue(timerTicked);
        }

    [Test]
    public void TimerClock_Timer_DistinctUntilChanged()
        {
        // Arrange & Act
        var timer = timerClock.Timer;

        // Assert
        bool valuesAreDistinct = true;
        timer.Buffer(2, 1).Subscribe(buffer =>
        {
            if (buffer.Count == 2 && buffer[0] == buffer[1])
                valuesAreDistinct = false;
        });

        Assert.IsTrue(valuesAreDistinct);
        }
    }
