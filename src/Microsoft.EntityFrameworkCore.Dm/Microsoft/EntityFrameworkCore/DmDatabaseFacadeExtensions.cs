using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
	public static class DmDatabaseFacadeExtensions
	{
		public static bool IsDm([JetBrains.Annotations.NotNull] this DatabaseFacade database)
		{
			return database.ProviderName.Equals(typeof(DmOptionsExtension).GetTypeInfo().Assembly.GetName().Name, StringComparison.Ordinal);
		}
	}
}
