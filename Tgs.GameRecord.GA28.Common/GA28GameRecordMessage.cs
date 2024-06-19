using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.GA28.Common;

public record GA28GameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	public string PrivateKey { get; init; } = default!;
	[Required]
	public string PartnerAccountId { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	public DateTimeOffset StartTime { get; init; }
	[Required]
	[DataType(DataType.DateTime)]
	public DateTimeOffset EndTime { get; init; }
}