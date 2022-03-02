using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTransactionFactory : IRelationalTransactionFactory
	{
		public virtual RelationalTransaction Create(IRelationalConnection connection, DbTransaction transaction, Guid transactionId, IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger, bool transactionOwned)
		{
			return new DmTransaction(connection, transaction, transactionId, logger, transactionOwned);
		}
	}
}
