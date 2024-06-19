using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tgs.GameRecord.GA28.Common;
using Valhalla.MessageQueue;

namespace Tgs.GameRecord.GA28.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GA28Controller : ControllerBase
{
	/// <summary>
	/// Games the record manual asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///				"apiUrl": "http://partner-api.dev.ga28.cc",
	///				"privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///				"partnerAccountId": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
	///				"startTime": "2024-06-18T10:00:00+08:00",
	///				"endTime": "2024-06-18T11:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("manual")]
	public ValueTask GameRecordManualAsync(
		[FromServices] IMessageSender messageSender,
		GA28GameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: GA28NatsConst.ManualPublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record schedule asynchronous.
	/// </summary>
	/// <param name="messageSender">The message sender.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	/// <remarks>
	/// <b>Example</b><br />
	///
	///     Post
	///         {
	///				"apiUrl": "http://partner-api.dev.ga28.cc",
	///				"privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///				"partnerAccountId": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
	///				"startTime": "2024-06-18T10:00:00+08:00",
	///				"endTime": "2024-06-18T11:00:00+08:00"
	///         }
	/// </remarks>
	[HttpPost("schedule")]
	public ValueTask GameRecordScheduleAsync(
		[FromServices] IMessageSender messageSender,
		GA28GameRecordMessage source)
		=> messageSender.PublishAsync(
			subject: GA28NatsConst.SchedulePublish,
			data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(source)),
			cancellationToken: HttpContext.RequestAborted);
}
