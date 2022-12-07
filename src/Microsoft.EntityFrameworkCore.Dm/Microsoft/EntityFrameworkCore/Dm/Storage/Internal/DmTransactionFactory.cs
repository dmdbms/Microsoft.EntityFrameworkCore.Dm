using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTransactionFactory : IRelationalTransactionFactory
	{
		protected virtual RelationalTransactionFactoryDependencies Dependencies { get; }

		public DmTransactionFactory(RelationalTransactionFactoryDependencies dependencies)
		{
			Check.NotNull<RelationalTransactionFactoryDependencies>(dependencies, "dependencies");
			Dependencies = dependencies;
		}

		public virtual RelationalTransaction Create(IRelationalConnection connection, DbTransaction transaction, Guid transactionId, IDiagnosticsLogger<Transaction> logger, bool transactionOwned)
		{
			return (RelationalTransaction)(object)new DmTransaction(connection, transaction, transactionId, logger, transactionOwned, Dependencies.SqlGenerationHelper);
		}
	}
}
