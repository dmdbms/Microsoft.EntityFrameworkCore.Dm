using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Update.Internal
{
	public class DmModificationCommandBatchFactory : IModificationCommandBatchFactory
	{
		private readonly ModificationCommandBatchFactoryDependencies _dependencies;

		private readonly IDbContextOptions _options;

		public DmModificationCommandBatchFactory([NotNull] ModificationCommandBatchFactoryDependencies dependencies, [NotNull] IDbContextOptions options)
		{
			Check.NotNull<ModificationCommandBatchFactoryDependencies>(dependencies, "dependencies");
			Check.NotNull<IDbContextOptions>(options, "options");
			_dependencies = dependencies;
			_options = options;
		}

		public virtual ModificationCommandBatch Create()
		{
			DmOptionsExtension dmOptionsExtension = _options.Extensions.OfType<DmOptionsExtension>().FirstOrDefault();
			return (ModificationCommandBatch)(object)new DmModificationCommandBatch(_dependencies, (dmOptionsExtension != null) ? ((RelationalOptionsExtension)dmOptionsExtension).MaxBatchSize : null);
		}
	}
}
