using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal;

namespace Microsoft.EntityFrameworkCore.Migrations
{
	public static class DmMigrationBuilderExtensions
	{
		public static bool IsDm([NotNull] this MigrationBuilder migrationBuilder)
		{
			return string.Equals(migrationBuilder.ActiveProvider, typeof(DmOptionsExtension).GetTypeInfo().Assembly.GetName().Name, StringComparison.Ordinal);
		}
	}
}
