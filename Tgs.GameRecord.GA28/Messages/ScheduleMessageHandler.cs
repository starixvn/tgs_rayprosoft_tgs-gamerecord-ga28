using System.Text;
using System.Text.Json;
using Tgs.GameRecord.GA28.Common;
using Tgs.GameRecord.GA28.Core;
using Tgs.GameRecord.GA28.Core.ApplicationServices;
using Valhalla.MessageQueue;
using Tgs.GameRecord.GA28.Messages;

namespace Tgs.GameRecord.GA28.Messages;

public class ScheduleMessageHandler(
	ILogger<ScheduleMessageHandler> logger,
	IPullRecordDispatcher dispatcher) : IMessageHandler<ReadOnlyMemory<byte>>
{
	public ValueTask HandleAsync(string subject, ReadOnlyMemory<byte> data, IEnumerable<MessageHeaderValue>? headerValues, CancellationToken cancellationToken = default)
	{
		var json = Encoding.UTF8.GetString(data.Span);
		var message = JsonSerializer.Deserialize<GA28GameRecordMessage>(json)
			?? throw new NullReferenceException("The request data resolves to a null value");

		logger.LogInformation("{logInformation}",
			JsonSerializer.Serialize(new
			{
				LogOn = DateTime.Now.ToString("u"),
				Subject = subject,
				Data = new
				{
					message.ApiUrl,
					message.PrivateKey,
					message.PartnerAccountId,
					StartTime = message.StartTime.ToString("u"),
					EndTime = message.EndTime.ToString("u")
				},
				Message = "This receive message handler."
			}));

		return dispatcher.PushCommandAsync(
			command: new PullRecordCommand(
			ApiUrl: message.ApiUrl,
			PrivateKey: message.PrivateKey,
			PartnerAccountId: message.PartnerAccountId,
			StartTime: message.StartTime,
			EndTime: message.EndTime),
			cancellationToken: cancellationToken);
	}
}
