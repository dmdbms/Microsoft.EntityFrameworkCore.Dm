using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTransaction : RelationalTransaction
	{
		private static readonly bool _useOldBehavior = AppContext.TryGetSwitch("Microsoft.EntityFrameworkCore.Issue23305", out var isEnabled) && isEnabled;

		public DmTransaction([NotNull] IRelationalConnection connection, [NotNull] DbTransaction transaction, Guid transactionId, [NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Transaction> logger, bool transactionOwned)
			: base(connection, transaction, transactionId, logger, transactionOwned)
		{
		}

		protected override string GetReleaseSavepointSql([NotNull] string name)
		{
			return "RELEASE_SAVEPOINT('" + name + "')";
		}
	}
}
