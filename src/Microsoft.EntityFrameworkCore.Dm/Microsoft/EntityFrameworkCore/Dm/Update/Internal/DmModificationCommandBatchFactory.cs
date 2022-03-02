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

		public DmModificationCommandBatchFactory([JetBrains.Annotations.NotNull] ModificationCommandBatchFactoryDependencies dependencies, [JetBrains.Annotations.NotNull] IDbContextOptions options)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
			_dependencies = dependencies;
			_options = options;
		}

		public virtual ModificationCommandBatch Create()
		{
			DmOptionsExtension dmOptionsExtension = _options.Extensions.OfType<DmOptionsExtension>().FirstOrDefault();
			return new DmModificationCommandBatch(_dependencies, dmOptionsExtension?.MaxBatchSize);
		}
	}
}
