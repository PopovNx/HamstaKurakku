using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using ComposableAsync;
using HamsterCrack.Dto;
using RateLimiter;

namespace HamsterCrack.Services;

public class HamstaApi(HttpClient client, ILogger<HamstaApi> logger)
{
    private const string BuyBoostEndpoint = "buy-boost";
    private const string BuyUpgradeEndpoint = "buy-upgrade";
    private const string UpgradesForBuyEndpoint = "upgrades-for-buy";
    private const string TasksEndpoint = "list-tasks";
    private const string TapEndpoint = "tap";
    private const string DoDailyCipherEndpoint = "claim-daily-cipher";
    private const string CompleteTaskEndpoint = "check-task";

    private static JsonContext JContext => JsonContext.Default;

    private readonly TimeLimiter _limiter = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(2));

    private async Task<HttpResponseMessage> SendPostRequestAsync<T>(string endpoint, T requestBody,
        JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancellationToken)
    {
        await _limiter;
        return await client.PostAsJsonAsync(endpoint, requestBody, jsonTypeInfo, cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<HttpResponseMessage> SendEmptyPostRequestAsync(string endpoint,
        CancellationToken cancellationToken)
    {
        await _limiter;
        return await client.PostAsync(endpoint, null, cancellationToken)
            .ConfigureAwait(false);
    }


    public async Task BuyBoostAsync(string boostId, CancellationToken cancellationToken = default)
    {
        using var response = await SendPostRequestAsync(BuyBoostEndpoint, BuyBoostRequest.Create(boostId),
            JContext.BuyBoostRequest, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            logger.LogInformation("Failed to buy boost: {content}", content);
        else
            logger.LogInformation("Bought boost: {boostId}", boostId);
    }

    public async Task BuyUpgradeAsync(string upgradeId, CancellationToken cancellationToken = default)
    {
        using var response = await SendPostRequestAsync(BuyUpgradeEndpoint, BuyUpgradeRequest.Create(upgradeId),
            JContext.BuyUpgradeRequest, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            logger.LogInformation("Failed to buy upgrade: {content}", content);
        else
            logger.LogInformation("Bought upgrade: {upgradeId}", upgradeId);
    }


    public async Task<UpgradeContent[]> GetUpgradesAsync(CancellationToken cancellationToken = default)
    {
        using var response = await SendEmptyPostRequestAsync(UpgradesForBuyEndpoint, cancellationToken);
        var upgradesResponse =
            await response.Content.ReadFromJsonAsync(JContext.UpgradesForBuyResponse, cancellationToken);
        return upgradesResponse?.Upgrades ?? [];
    }


    public async Task<HamstaTask[]> GetTasksAsync(CancellationToken cancellationToken = default)
    {
        using var response = await SendEmptyPostRequestAsync(TasksEndpoint, cancellationToken);
        var tasksResponse =
            await response.Content.ReadFromJsonAsync(JContext.HamstaTasksResponse,
                cancellationToken);
        var tasks = tasksResponse?.Tasks ?? [];

        return tasks;
    }

    public async Task<HamstaState> PostClickAsync(CancellationToken cancellationToken = default)
    {
        using var response = await SendPostRequestAsync(TapEndpoint, TapRequest.CreateOptimal(), JContext.TapRequest,
            cancellationToken);
        var content = await response.Content.ReadFromJsonAsync(JContext.TapResponse, cancellationToken);

        return content?.State ?? throw new Exception("Failed to get state");
    }

    public async Task CompleteTaskAsync(string theBestId, CancellationToken cancellationToken = default)
    {
        using var response = await SendPostRequestAsync(CompleteTaskEndpoint, new CheckTaskRequest(theBestId),
            JContext.CheckTaskRequest, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            logger.LogInformation("Failed to complete task: {content}", content);
        else
            logger.LogInformation("Completed task: {theBestId}", theBestId);
    }

    public async Task DoDailyCipherAsync(string cipher, CancellationToken cancellationToken = default)
    {
        using var response = await SendPostRequestAsync(DoDailyCipherEndpoint, new DoDailyCipherRequest(cipher),
            JContext.DoDailyCipherRequest, cancellationToken);
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            logger.LogInformation("Failed to claim daily cipher: {content}", content);
        else
            logger.LogInformation("Claimed daily cipher: {cipher}", cipher);
    }
}