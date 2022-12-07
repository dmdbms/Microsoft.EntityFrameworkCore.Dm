using System;
using System.Data.Common;
using Dm;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmRelationalConnection : RelationalConnection, IDmRelationalConnection, IRelationalConnection, IRelationalTransactionManager, IDbContextTransactionManager, IResettableService, IDisposable, IAsyncDisposable
	{
		public const string EFPDBAdminUser = "SYSDBA";

		internal const int DefaultMasterConnectionCommandTimeout = 60;

		protected override bool SupportsAmbientTransactions => false;

		public DmRelationalConnection([NotNull] RelationalConnectionDependencies dependencies)
			: base(dependencies)
		{
		}

		protected override DbConnection CreateDbConnection()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return (DbConnection)new DmConnection(((RelationalConnection)this).ConnectionString, true);
		}

		public virtual IDmRelationalConnection CreateMasterConnection()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			DmConnectionStringBuilder val = new DmConnectionStringBuilder(((RelationalConnection)this).ConnectionString);
			val.User=("SYSDBA");
			val.Password=("SYSDBA");
			DmConnectionStringBuilder val2 = val;
			DbContextOptions options = DmDbContextOptionsExtensions.UseDm(new DbContextOptionsBuilder(), ((DbConnectionStringBuilder)(object)val2).ConnectionString, delegate(DmDbContextOptionsBuilder b)
			{
				((RelationalDbContextOptionsBuilder<DmDbContextOptionsBuilder, DmOptionsExtension>)b).CommandTimeout((int?)(((RelationalConnection)this).CommandTimeout ?? 60));
			}).Options;
            //RelationalConnectionDependencies obj = ((RelationalConnection)this).Dependencies._003CClone_003E_0024();
            RelationalConnectionDependencies obj = base.Dependencies;
			RelationalConnectionDependencies newObj = new RelationalConnectionDependencies(options, obj.TransactionLogger, obj.ConnectionLogger, obj.ConnectionStringResolver, obj.RelationalTransactionFactory, obj.CurrentContext, obj.RelationalCommandBuilderFactory);
            //obj.ContextOptions=((IDbContextOptions)(object)options);
			return new DmRelationalConnection(newObj);
		}
	}
}
