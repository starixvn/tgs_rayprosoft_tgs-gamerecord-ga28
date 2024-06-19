using Tgs.GameRecord.GA28.Core.ApplicationServices;

namespace Tgs.GameRecord.GA28.Core;

public interface IPullRecordDispatcher
{
	ValueTask PushCommandAsync(PullRecordCommand command, CancellationToken cancellationToken = default);
}
