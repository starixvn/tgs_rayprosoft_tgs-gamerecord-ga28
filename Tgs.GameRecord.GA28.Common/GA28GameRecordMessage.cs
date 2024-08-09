using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.GA28.Common;

public record GA28GameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "http://partner-api.dev.ga28.cc")]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "8456c9de-d589-4e8a-876f-c9c8e48dda3e")]
	public string PrivateKey { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "99a6f8ef-38a6-4f31-b69a-e222c2a54771")]
	public string PartnerAccountId { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2024-06-18T10:00:00+08:00")]
	public DateTimeOffset StartTime { get; init; }
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2024-06-18T11:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; }
}
