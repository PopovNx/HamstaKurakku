using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;
using ComposableAsync;
using HamsterCrack.Dto;
using HamsterCrack.Dto.Api;
using HamsterCrack.Dto.Api.Boost;
using HamsterCrack.Dto.Api.Cipher;
using HamsterCrack.Dto.Api.Config;
using HamsterCrack.Dto.Api.Tap;
using HamsterCrack.Dto.Api.Tasks;
using HamsterCrack.Dto.Api.Upgrades;
using RateLimiter;

namespace HamsterCrack.Services;

public class HamstaApi(HttpClient client, ILogger<HamstaApi> logger)
{
    private const string BuyBoostEndpoint = "buy-boost";
    private const string BuyUpgradeEndpoint = "buy-upgrade";
    private const string UpgradesForBuyEndpoint = "upgrades-for-buy";
    private const string TasksEndpoint = "list-tasks";
    private const string TapEndpoint = "tap";
    private const string ConfigEndpoint = "config";
    private const string DoDailyCipherEndpoint = "claim-daily-cipher";
    private const string CompleteTaskEndpoint = "check-task";

    private static JsonContext JContext => JsonContext.Default;

    private readonly TimeLimiter _limiter = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(2));


    private async Task<ApiResponse> PerformRequestAsync<TRq, TRs>(
        string endpoint,
        TRq requestBody,
        JsonTypeInfo<TRq> jsonTypeInfo,
        JsonTypeInfo<TRs> responseJsonTypeInfo,
        CancellationToken cancellationToken = default)
    {
        await _limiter;
        var response = await client.PostAsJsonAsync(endpoint, requestBody, jsonTypeInfo, cancellationToken)
            .ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync(JContext.ApiErrorResult, cancellationToken);
            logger.LogInformation("Failed to perform request: {content}", content);
            return content ?? ApiErrorResult.CreateUnknownError();
        }

        var result = await response.Content.ReadFromJsonAsync(responseJsonTypeInfo, cancellationToken);
        if (result is not null) return new ApiSuccessResponse<TRs>(result);
        logger.LogInformation("Failed to deserialize response");
        return ApiErrorResult.CreateUnknownError();
    }


    public async Task BuyBoostAsync(string boostId, CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(BuyBoostEndpoint, BuyBoostRequest.Create(boostId),
            JContext.BuyBoostRequest, JContext.Empty, cancellationToken);
        if (response is ApiSuccessResponse<Empty>)
            logger.LogInformation("Bought boost: {boostId}", boostId);
        else
            logger.LogInformation("Failed to buy boost: {response}", response);
    }

    public async Task BuyUpgradeAsync(string upgradeId, CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(BuyUpgradeEndpoint, BuyUpgradeRequest.Create(upgradeId),
            JContext.BuyUpgradeRequest, JContext.Empty, cancellationToken);
        if (response is ApiSuccessResponse<Empty>)
            logger.LogInformation("Bought upgrade: {upgradeId}", upgradeId);
        else
            logger.LogInformation("Failed to buy upgrade: {response}", response);
    }


    public async Task<UpgradeContent[]> GetUpgradesAsync(CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(UpgradesForBuyEndpoint, Empty.Instance, JContext.Empty,
            JContext.UpgradesForBuyResponse, cancellationToken);
        if (response is ApiSuccessResponse<UpgradesForBuyResponse> { Data.Upgrades: var upgrades })
            return upgrades;
        logger.LogInformation("Failed to get upgrades: {response}", response);
        return [];
    }


    public async Task<HamstaTask[]> GetTasksAsync(CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(TasksEndpoint, Empty.Instance, JContext.Empty,
            JContext.HamstaTasksResponse,
            cancellationToken);
        if (response is ApiSuccessResponse<HamstaTasksResponse> { Data.Tasks: var tasks })
            return tasks;

        logger.LogInformation("Failed to get tasks: {response}", response);
        return [];
    }

    public async Task<HamstaState?> PostClickAsync(CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(TapEndpoint, TapRequest.CreateOptimal(), JContext.TapRequest,
            JContext.TapResponse, cancellationToken);
        if (response is ApiSuccessResponse<TapResponse> { Data: var tapResponse })
            return tapResponse.State;
        logger.LogInformation("Failed to tap: {response}", response);

        return null;
    }

    public async Task CompleteTaskAsync(string theBestId, CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(CompleteTaskEndpoint, new CheckTaskRequest(theBestId),
            JContext.CheckTaskRequest, JContext.Empty, cancellationToken);

        if (response is ApiSuccessResponse<Empty>)
            logger.LogInformation("Completed task: {theBestId}", theBestId);
        else
            logger.LogInformation("Failed to complete task: {response}", response);
    }

    public async Task DoDailyCipherAsync(string cipher, CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(DoDailyCipherEndpoint, new DoDailyCipherRequest(cipher),
            JContext.DoDailyCipherRequest, JContext.Empty, cancellationToken);

        if (response is ApiSuccessResponse<Empty>)
            logger.LogInformation("Claimed daily cipher: {cipher}", cipher);
        else
            logger.LogInformation("Failed to claim daily cipher: {response}", response);
    }

    public async Task<CipherStatus?> FetchConfigAsync(CancellationToken cancellationToken = default)
    {
        var response = await PerformRequestAsync(ConfigEndpoint, Empty.Instance, JContext.Empty,
            JContext.GetConfigResponse, cancellationToken);
        if (response is ApiSuccessResponse<GetConfigResponse> { Data: var config })
            return config.Status;

        logger.LogInformation("Failed to fetch config: {response}", response);
        return null;
    }
}