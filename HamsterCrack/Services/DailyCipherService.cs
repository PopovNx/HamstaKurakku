namespace HamsterCrack.Services;

public sealed class DailyCipherService(HamstaApi api, ILogger<DailyCipherService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(4));
        while (!stoppingToken.IsCancellationRequested)
        {
            await periodicTimer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false);
            if (!Directory.Exists("cipher"))
                continue;
            var files = Directory.GetFiles("cipher", "*");
            await ApplyCiphers(files, stoppingToken);
        }
    }

    private async Task ApplyCiphers(string[] files, CancellationToken stoppingToken)
    {
        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            logger.LogInformation("Sending cipher: {cipher}", name);
            File.Delete(file);
            await api.DoDailyCipherAsync(name, stoppingToken);
        }
    }
}