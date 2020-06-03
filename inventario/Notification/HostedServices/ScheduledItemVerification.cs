using System;
using System.Threading;
using System.Threading.Tasks;
using NCrontab;

public abstract class ScheduledItemVerification : BackgroundService
{
    private CrontabSchedule _scheduleItemVerification;
    private DateTime _nextRun;
    protected abstract string ScheduleRun { get; }
    public ScheduledItemVerification()
    {
        _scheduleItemVerification = CrontabSchedule.Parse(ScheduleRun);
        _nextRun = _scheduleItemVerification.GetNextOccurrence(DateTime.Now);
    }

    // Override this BackgroundService method.
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        do
        {
            var now = DateTime.Now;

            if (now > _nextRun)
            {
                // At execution time, I make the asynchronous call of the CheckItemExpirationDate and schedule next run.
                await CheckItemExpirationDate();
                _nextRun = _scheduleItemVerification.GetNextOccurrence(DateTime.Now);
            }
            //5 seconds delay.
            await Task.Delay(5000, stoppingToken); 
        }
        while (!stoppingToken.IsCancellationRequested);
    }

    public abstract Task CheckItemExpirationDate();
}