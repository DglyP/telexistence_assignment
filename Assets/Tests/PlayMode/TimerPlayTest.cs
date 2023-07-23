using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

public class TimerPlayTest
    {

    private ICountdown countdown;
    [SetUp]
    public void SetUp()
        {
        countdown = new Countdown();
        }

    [Test]
    public async Task Able_to_Create_Countdown()
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
        Countdown countdown = new Countdown();
        await Task.Delay(3000);

        return new Timer();
        }
    public struct Timer
        {

        }
    }