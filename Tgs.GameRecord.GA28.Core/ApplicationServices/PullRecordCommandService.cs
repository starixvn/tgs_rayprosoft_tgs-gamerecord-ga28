using System.Text.Json;
using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;
using Tgs.GameInterface.Ga28.Client;
using Tgs.GameInterface.Ga28.Protos;

namespace Tgs.GameRecord.GA28.Core.ApplicationServices;

internal class PullRecordCommandService(
	ILogger<PullRecordCommandService> logger,
	IGameInterfaceGa28Client client,
	ICommandService<HandleRecordCommand> service) : ICommandService<PullRecordCommand>
{
	public async ValueTask ExecuteAsync(PullRecordCommand command, CancellationToken cancellationToken = default)
	{
		var page = 1;
		IEnumerable<GameRecordInfo> gameRecords;
		do
		{
			gameRecords = await client.GameRecordAsync(
				apiUrl: command.ApiUrl,
				privateKey: command.PrivateKey,
				partnerAccountId: command.PartnerAccountId,
				startTime: command.StartTime,
				endTime: command.EndTime,
				page: page,
				pageSize: 32,
				cancellationToken: cancellationToken)
			   .ToArrayAsync(
				cancellationToken: cancellationToken)
			   .ConfigureAwait(false)
			   ?? Enumerable.Empty<GameRecordInfo>();

			foreach (var gameRecord in gameRecords)
			{
				try
				{
					await service.ExecuteAsync(
						command: new(gameRecord),
						cancellationToken: cancellationToken)
						.ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "{logError}",
						JsonSerializer.Serialize(new
						{
							LogOn = DateTime.Now.ToString("u"),
							Data = new
							{
								gameRecord.PlayerCode,
								gameRecord.ExternalBetId,
								ExternalCreatedOn = gameRecord.ExternalCreatedOn.ToDateTimeOffset(),
								ExternalClosedOn = gameRecord.ExternalClosedOn.ToDateTimeOffset()
							},
							Message = "An exception occurred in this game wager."
						}));
				}
			}

			page++;
		} while (gameRecords.Any());
	}
}