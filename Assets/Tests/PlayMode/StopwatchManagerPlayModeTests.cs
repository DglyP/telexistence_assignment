using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

public class StopwatchManagerPlayModeTests
    {

    private IStopwatch stopwatch;
    [SetUp]
    public void SetUp()
        {
        stopwatch = new Stopwatch();
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
        Stopwatch stopwatch= new Stopwatch();
        await Task.Delay(3000);

        return new Timer();
        }
    public struct Timer
        {

        }
    }