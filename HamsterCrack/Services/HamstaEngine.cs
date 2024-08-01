using HamsterCrack.Dto;

namespace HamsterCrack.Services;

public sealed class HamstaEngine(HamstaApi api, ILogger<HamstaEngine> logger)
{
    private HamstaState? _state;

    private DateTimeOffset _lastTap = DateTimeOffset.MinValue;

    public async Task TryBuyNewUpgrades(CancellationToken cancellationToken = default)
    {
        var passiveEarnLimit = (_state?.EarnPassivePerHour ?? 0) * 10 + 10000;
        if (_state?.BalanceCoins < passiveEarnLimit)
        {
            logger.LogInformation(
                "WAITING FOR {passiveEarnLimit} c to buy upgrades, currently have {BalanceCoins} c ({Percentage}%)",
                passiveEarnLimit, _state?.BalanceCoins, _state?.BalanceCoins / passiveEarnLimit * 100);
            return;
        }

        var upgrades = await api.GetUpgradesAsync(cancellationToken);

        var theBest = upgrades
            .Where(x => x.IsAvailable)
            .Where(x => !x.IsExpired)
            .Where(x => x.Price <= (_state?.BalanceCoins ?? 0))
            .OrderByDescending(x => x.ProfitPerCostRate)
            .FirstOrDefault();
        if (theBest is not null)
        {
            logger.LogInformation(
                "BUYING UPGRADE: {Name} because of {ProfitPerHour} c/h and cost {Price} c",
                theBest.Name, theBest.ProfitPerHour, theBest.Price);
            await api.BuyUpgradeAsync(theBest.Id, cancellationToken);
        }
    }

    public async Task TryFullClickAsync(CancellationToken cancellationToken = default)
    {
        var needMoreTaps = (_state?.AvailableTaps ?? 0) < 10;
        if (!needMoreTaps)
            return;
        await api.BuyBoostAsync("BoostFullAvailableTaps", cancellationToken);
    }


    public async Task TryCompleteTasks(CancellationToken cancellationToken = default)
    {
        var tasks = await api.GetTasksAsync(cancellationToken);
        var theBest = tasks
            .Where(x => x.Id != "invite_friends")
            .FirstOrDefault(x => !x.IsCompleted);
        if (theBest is not null)
        {
            await api.CompleteTaskAsync(theBest.Id, cancellationToken);
        }
    }

    public async Task PerformTapAsync(CancellationToken cancellationToken = default)
    {
        var newState = await api.PostClickAsync(cancellationToken);
        var deltaCoins = newState.BalanceCoins - _state?.BalanceCoins;

        if (deltaCoins.HasValue)
        {
            var deltaTime = DateTimeOffset.UtcNow - _lastTap;
            var coinsPerSecond = deltaCoins.Value / deltaTime.TotalSeconds;
            var coinPerHour = coinsPerSecond * 3600;

            logger.LogInformation("COINS: {BalanceCoins} c (+{deltaCoins})", newState.BalanceCoins, deltaCoins);
            logger.LogInformation("COPHR: {CoinPerHour} c/h", coinPerHour);
        }

        _state = newState;
        _lastTap = DateTimeOffset.UtcNow;
    }
}