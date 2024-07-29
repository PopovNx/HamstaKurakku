using System.Text.Json;
using System.Text.Json.Serialization;
using HamsterCrack.Dto;

namespace HamsterCrack;

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