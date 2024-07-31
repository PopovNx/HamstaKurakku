namespace HamsterCrack.Services;

public sealed class DailyCipherService(HamstaApi api, ILogger<DailyCipherService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var periodicTimer = new PeriodicTimer(TimeSpan.FromHours(1));
        while (!stoppingToken.IsCancellationRequested)
        {
            var cipherStatus = await api.FetchConfigAsync(stoppingToken);
            if (cipherStatus is null)
            {
                continue;
            }

            if (cipherStatus.IsClaimed)
            {
                logger.LogInformation("Daily cipher already claimed");
                await periodicTimer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false);
                continue;
            }
            logger.LogInformation("Claiming daily cipher");
            await api.DoDailyCipherAsync(cipherStatus.DecodedCipher, stoppingToken);
            logger.LogInformation("Daily cipher claimed {cipher}", cipherStatus.DecodedCipher);
        }
    }
}