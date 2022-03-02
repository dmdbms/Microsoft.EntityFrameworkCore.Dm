using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public interface IDmRelationalConnection : IRelationalConnection, IRelationalTransactionManager, IDbContextTransactionManager, IResettableService, IDisposable, IAsyncDisposable
	{
		IDmRelationalConnection CreateMasterConnection();
	}
}
