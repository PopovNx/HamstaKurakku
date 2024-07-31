using System.Text.Json.Serialization;
using HamsterCrack.Dto;
using HamsterCrack.Dto.Api;
using HamsterCrack.Dto.Api.Boost;
using HamsterCrack.Dto.Api.Cipher;
using HamsterCrack.Dto.Api.Config;
using HamsterCrack.Dto.Api.Tap;
using HamsterCrack.Dto.Api.Tasks;
using HamsterCrack.Dto.Api.Upgrades;

namespace HamsterCrack;

[JsonSerializable(typeof(ApiErrorResult))]
[JsonSerializable(typeof(ApiSuccessResponse<Empty>))]
[JsonSerializable(typeof(CipherStatus))]
[JsonSerializable(typeof(Empty))]
[JsonSerializable(typeof(GetConfigResponse))]
[JsonSerializable(typeof(BuyBoostRequest))]
[JsonSerializable(typeof(BuyUpgradeRequest))]
[JsonSerializable(typeof(CheckTaskRequest))]
[JsonSerializable(typeof(DoDailyCipherRequest))]
[JsonSerializable(typeof(HamstaTasksResponse))]
[JsonSerializable(typeof(TapRequest))]
[JsonSerializable(typeof(TapResponse))]
[JsonSerializable(typeof(UpgradeContent))]
[JsonSerializable(typeof(UpgradesForBuyResponse))]
public sealed partial class JsonContext : JsonSerializerContext;