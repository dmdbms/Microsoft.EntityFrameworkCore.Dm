using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmDbContextOptionsExtensions
	{
		public static DbContextOptionsBuilder UseDm([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder optionsBuilder, [JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsBuilder, "optionsBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(connectionString, "connectionString");
			DmOptionsExtension extension = (DmOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
			((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
			ConfigureWarnings(optionsBuilder);
			dmOptionsAction?.Invoke(new DmDbContextOptionsBuilder(optionsBuilder));
			return optionsBuilder;
		}

		public static DbContextOptionsBuilder UseDm([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder optionsBuilder, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsBuilder, "optionsBuilder");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
			DmOptionsExtension extension = (DmOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnection(connection);
			((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
			ConfigureWarnings(optionsBuilder);
			dmOptionsAction?.Invoke(new DmDbContextOptionsBuilder(optionsBuilder));
			return optionsBuilder;
		}

		public static DbContextOptionsBuilder<TContext> UseDm<TContext>([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null) where TContext : DbContext
		{
			return (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)optionsBuilder).UseDm(connectionString, dmOptionsAction);
		}

		public static DbContextOptionsBuilder<TContext> UseDm<TContext>([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] Action<DmDbContextOptionsBuilder> dmOptionsAction = null) where TContext : DbContext
		{
			return (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)optionsBuilder).UseDm(connection, dmOptionsAction);
		}

		private static DmOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
		{
			return optionsBuilder.Options.FindExtension<DmOptionsExtension>() ?? new DmOptionsExtension();
		}

		private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
		{
			CoreOptionsExtension coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
			coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(coreOptionsExtension.WarningsConfiguration.TryWithExplicit(RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));
			((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
		}
	}
}
