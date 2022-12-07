using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTransaction : RelationalTransaction
	{
		private static readonly bool _useOldBehavior = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue23305", out var isEnabled) && isEnabled;

		public DmTransaction(IRelationalConnection connection, DbTransaction transaction, Guid transactionId, IDiagnosticsLogger<Transaction> logger, bool transactionOwned, ISqlGenerationHelper sqlGenerationHelper)
			: base(connection, transaction, transactionId, logger, transactionOwned, sqlGenerationHelper)
		{
		}

		public override void ReleaseSavepoint(string name)
		{
		}
	}
}
