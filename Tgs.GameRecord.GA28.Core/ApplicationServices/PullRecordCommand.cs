namespace Tgs.GameRecord.GA28.Core.ApplicationServices;
public record PullRecordCommand(
	Uri ApiUrl,
	string PrivateKey,
	string PartnerAccountId,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
