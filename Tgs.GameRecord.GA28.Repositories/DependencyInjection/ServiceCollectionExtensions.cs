using StackExchange.Redis;
using Tgs.GameRecord.GA28.Core;
using Tgs.GameRecord.GA28.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecordRepository(
		this IServiceCollection services,
		Func<IServiceProvider, IDatabase> redisDatabaseResolver)
		=> services
		.AddTransient<IHangfireLock>(sp => ActivatorUtilities.CreateInstance<HangfireLock>(sp, redisDatabaseResolver(sp)));
}
