using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmExecutionStrategyFactory : RelationalExecutionStrategyFactory
	{
		public DmExecutionStrategyFactory([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override IExecutionStrategy CreateDefaultStrategy(ExecutionStrategyDependencies dependencies)
		{
			return new DmExecutionStrategy(dependencies);
		}
	}
}
