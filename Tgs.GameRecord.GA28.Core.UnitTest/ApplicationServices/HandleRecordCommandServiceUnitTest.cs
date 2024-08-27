using System.Text.Json.Nodes;

using Google.Protobuf.WellKnownTypes;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using NSubstitute;

using Tgs.GameRecord.GA28.Core.ApplicationServices;
using Tgs.GameRecord.Handle.Client;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.GA28.Core.UnitTest.ApplicationServices;

public class HandleRecordCommandServiceUnitTest
{
	[Fact]
	public async Task Win()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "GA28",
			GameProviderConfiguration = "GA28Configuration",
			GameExternalId = "cockfighting",
			HangfirePrefix = "gamerecord-ga28-hangfire:",
			HangfireLocking = "gamerecord-ga28-locking:",
			IntervalMinutes = 10d,
			IntervalMinutesMax = 60d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			GameRecord: new GameInterface.Ga28.Protos.GameRecordInfo
			{
				PlayerCode = "4ucmqplxb029",
				ExternalRoundId = "ph#74 M000478",
				ExternalBetId = "190103969DD96J562D",
				ExternalCreatedOn = DateTimeOffset.Parse("2024-06-13T06:11:56.510124Z").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2024-06-13T06:12:29.238152Z").ToTimestamp(),
				Result = GameInterface.Ga28.Protos.GameRecordResult.Win,
				Bet = 10.00m,
				ValidBet = 9.85m,
				Settle = 16.548m,
				WinLoss = 6.698m,
				AdditionalData = JsonNode.Parse("""
				[
					{
						"sid": "190103969DD96J562D"
					}
				]
				""")!.ToJsonString()
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Win,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 10.00m,
				ValidBet = 9.85m,
				Settle = 16.548m,
				Commission = 0.0m,
				Turnover = 9.85m,
				WinLoss = 6.698m,
				Jackpot = 0.0m,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Loss()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "GA28",
			GameProviderConfiguration = "GA28Configuration",
			GameExternalId = "cockfighting",
			HangfirePrefix = "gamerecord-ga28-hangfire:",
			HangfireLocking = "gamerecord-ga28-locking:",
			IntervalMinutes = 10d,
			IntervalMinutesMax = 60d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			GameRecord: new GameInterface.Ga28.Protos.GameRecordInfo
			{
				PlayerCode = "4ucmqplxb029",
				ExternalRoundId = "ph#67 M000478",
				ExternalBetId = "19010170AE3VA8I515",
				ExternalCreatedOn = DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2024-06-13T05:37:15.305415Z").ToTimestamp(),
				Result = GameInterface.Ga28.Protos.GameRecordResult.Loss,
				Bet = 1.00m,
				ValidBet = 0.985m,
				Settle = 0.00m,
				WinLoss = -0.985m,
				AdditionalData = JsonNode.Parse("""
				[
					{
						"sid": "19010170AE3VA8I515"
					}
				]
				""")!.ToJsonString()
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Loss,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 1.00m,
				ValidBet = 0.985m,
				Settle = 0.00m,
				Commission = 0.00m,
				Turnover = 0.985m,
				WinLoss = -0.985m,
				Jackpot = 0.00m,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Void()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "GA28",
			GameProviderConfiguration = "GA28Configuration",
			GameExternalId = "cockfighting",
			HangfirePrefix = "gamerecord-ga28-hangfire:",
			HangfireLocking = "gamerecord-ga28-locking:",
			IntervalMinutes = 10d,
			IntervalMinutesMax = 60d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			GameRecord: new GameInterface.Ga28.Protos.GameRecordInfo
			{
				PlayerCode = "4ucmqplxb029",
				ExternalRoundId = "ph#25 M000485",
				ExternalBetId = "190291442B0CIF1309",
				ExternalCreatedOn = DateTimeOffset.Parse("2024-06-18T02:01:52.049269Z").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2024-06-18T02:19:08.847127Z").ToTimestamp(),
				Result = GameInterface.Ga28.Protos.GameRecordResult.Void,
				Bet = 9.85m,
				ValidBet = 0.00m,
				Settle = 0.00m,
				WinLoss = 0.00m,
				AdditionalData = JsonNode.Parse("""
				[
					{
						"sid": "190291442B0CIF1309"
					}
				]
				""")!.ToJsonString()
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Void,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 9.85m,
				ValidBet = 0.00m,
				Settle = 0.00m,
				Commission = 0.00m,
				Turnover = 0.00m,
				WinLoss = 0.00m,
				Jackpot = 0.00m,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task Cancelled()
	{
		var faceLogger = NullLogger<HandleRecordCommandService>.Instance;
		var fakeGameRecordInfoOptions = Options.Create(new GameRecordInfoOptions
		{
			GameProviderCode = "GA28",
			GameProviderConfiguration = "GA28Configuration",
			GameExternalId = "cockfighting",
			HangfirePrefix = "gamerecord-ga28-hangfire:",
			HangfireLocking = "gamerecord-ga28-locking:",
			IntervalMinutes = 10d,
			IntervalMinutesMax = 60d,
			IntervalMinutesMinimum = 1d
		});
		var fakePolicySettingsOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetrySeconds = 1,
		});
		var fakeGameRecordHandleClient = Substitute.For<IGameRecordHandleClient>();

		var command = new HandleRecordCommand(
			GameRecord: new GameInterface.Ga28.Protos.GameRecordInfo
			{
				PlayerCode = "4ucmqplxb029",
				ExternalRoundId = "ph#23 M000485",
				ExternalBetId = "190290D99C1D4UQ2E3",
				ExternalCreatedOn = DateTimeOffset.Parse("2024-06-18T01:54:35.585921Z").ToTimestamp(),
				ExternalClosedOn = DateTimeOffset.Parse("2024-06-18T02:55:37.310049Z").ToTimestamp(),
				Result = GameInterface.Ga28.Protos.GameRecordResult.Cancelled,
				Bet = 10.00m,
				ValidBet = 0.00m,
				Settle = 0.00m,
				WinLoss = 0.00m,
				AdditionalData = JsonNode.Parse("""
				[
					{
						"sid": "190290D99C1D4UQ2E3"
					}
				]
				""")!.ToJsonString()
			});

		var sut = new HandleRecordCommandService(
			faceLogger,
			fakeGameRecordInfoOptions,
			fakePolicySettingsOptions,
			fakeGameRecordHandleClient);

		await sut.ExecuteAsync(command);

		_ = fakeGameRecordHandleClient
			.Received(1)
			.HandleAsync(
			source: Arg.Is(new Handle.Common.ViewModels.GameRecordHandleViewModel
			{
				GameProviderCode = fakeGameRecordInfoOptions.Value.GameProviderCode,
				PlayerCode = command.GameRecord.PlayerCode,
				ExternalRoundId = command.GameRecord.ExternalRoundId,
				ExternalBetId = command.GameRecord.ExternalBetId,
				ExternalGameId = fakeGameRecordInfoOptions.Value.GameExternalId,
				ExternalCreatedOn = command.GameRecord.ExternalCreatedOn.ToDateTimeOffset(),
				ExternalClosedOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalPayoffOn = command.GameRecord.ExternalClosedOn.ToDateTimeOffset(),
				ExternalResettleOn = DateTimeOffset.UnixEpoch,
				BetState = Handle.Common.Protos.BetState.Cancel,
				BetType = Handle.Common.Protos.BetType.Normal,
				FreeGameState = Handle.Common.Protos.FreeGameState.None,
				GambleState = Handle.Common.Protos.GambleState.None,
				CashoutState = Handle.Common.Protos.CashoutState.None,
				ResettleState = Handle.Common.Protos.ResettleState.None,
				JackpotState = Handle.Common.Protos.JackpotState.None,
				Bet = 10.00m,
				ValidBet = 0.00m,
				Settle = 0.00m,
				Commission = 0.00m,
				Turnover = 0.00m,
				WinLoss = 0.00m,
				Jackpot = 0.00m,
				AdditionalData = command.GameRecord.AdditionalData
			}),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
