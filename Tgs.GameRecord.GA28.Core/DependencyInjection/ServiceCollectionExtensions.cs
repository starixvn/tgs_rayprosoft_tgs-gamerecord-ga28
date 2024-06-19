using System.Reflection;

using Tgs.GameCommand.Client;
using Tgs.GameInterface.Ga28.Client;
using Tgs.GameRecord.GA28.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameRecordCore(
		this IServiceCollection services,
		Action<GameCommandClientOptions, IServiceProvider> gameCommandClientOptions,
		Action<GameInterfaceGa28ClientOptions, IServiceProvider> gameInterfaceGa28ClientOptions)
		=> services
		.RegisterAllServiceUtilityTypes(
			Assembly.GetExecutingAssembly(),
			ServiceLifetime.Transient)
		.AddGameCommandClient(gameCommandClientOptions)
		.AddGameInterfaceGa28Client(gameInterfaceGa28ClientOptions)
		.AddSingleton(TimeProvider.System)
		.AddSingleton<IPullRecordDispatcher, PullRecordDispatcher>()
		.AddTransient<IHangfireUtensil, HangfireUtensil>();
}