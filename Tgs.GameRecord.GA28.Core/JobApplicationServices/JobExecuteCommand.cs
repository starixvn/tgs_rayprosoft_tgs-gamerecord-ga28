namespace Tgs.GameRecord.GA28.Core.JobApplicationServices;

public record JobExecuteCommand(
	Uri ApiUrl,
	string PrivateKey,
	string PartnerAccountId);
