using Hangfire.Dashboard;

namespace Tgs.GameRecord.GA28;

public class IgnoreHangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => true;
}
