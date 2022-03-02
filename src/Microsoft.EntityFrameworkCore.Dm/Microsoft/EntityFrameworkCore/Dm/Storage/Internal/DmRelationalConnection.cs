using System;
using System.Data.Common;
using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmRelationalConnection : RelationalConnection, IDmRelationalConnection, IRelationalConnection, IRelationalTransactionManager, IDbContextTransactionManager, IResettableService, IDisposable, IAsyncDisposable
	{
		public const string EFPDBAdminUser = "SYSDBA";

		internal const int DefaultMasterConnectionCommandTimeout = 60;

		protected override bool SupportsAmbientTransactions => false;

		public DmRelationalConnection([JetBrains.Annotations.NotNull] RelationalConnectionDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override DbConnection CreateDbConnection()
		{
			return new DmConnection(ConnectionString, forEFCore: true);
		}

		public virtual IDmRelationalConnection CreateMasterConnection()
		{
			DmConnectionStringBuilder dmConnectionStringBuilder = new DmConnectionStringBuilder(ConnectionString)
			{
				User = "SYSDBA",
				Password = "SYSDBA"
			};
			DbContextOptions options = new DbContextOptionsBuilder().UseDm(dmConnectionStringBuilder.ConnectionString, delegate(DmDbContextOptionsBuilder b)
			{
				b.CommandTimeout(CommandTimeout ?? 60);
			}).Options;
			return new DmRelationalConnection(Dependencies.With(options));
		}
	}
}
