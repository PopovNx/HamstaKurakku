namespace HamsterCrack.Services;

public class HamstaBackgroundService(HamstaEngine engine) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(15));
        while (!stoppingToken.IsCancellationRequested)
        {
            await engine.PerformTapAsync(stoppingToken);
            await engine.TryBuyNewUpgrades(stoppingToken);
            await engine.TryFullClickAsync(stoppingToken);
            await engine.TryCompleteTasks(stoppingToken);

            await periodicTimer.WaitForNextTickAsync(stoppingToken);
        }
    }
}