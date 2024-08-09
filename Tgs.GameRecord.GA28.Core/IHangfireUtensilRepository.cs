using Tgs.GameRecord.GA28.Core.JobApplicationServices;

namespace Tgs.GameRecord.GA28.Core;

public interface IHangfireUtensilRepository
{
	public Task JobCheckAsync();

	public Task JobExecuteAsync(JobExecuteCommand source);

	public Task JobInitAsync();
}
