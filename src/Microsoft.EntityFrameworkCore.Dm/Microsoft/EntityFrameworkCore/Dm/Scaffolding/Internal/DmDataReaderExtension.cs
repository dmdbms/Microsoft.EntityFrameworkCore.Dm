using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
	public static class DmDataReaderExtension
	{
		public static T GetValueOrDefault<T>([NotNull] this DbDataReader reader, [NotNull] string name)
		{
			int ordinal = reader.GetOrdinal(name);
			return reader.IsDBNull(ordinal) ? default(T) : reader.GetFieldValue<T>(ordinal);
		}

		public static T GetValueOrDefault<T>([NotNull] this DbDataRecord record, [NotNull] string name)
		{
			int ordinal = record.GetOrdinal(name);
			return record.IsDBNull(ordinal) ? default(T) : ((T)record.GetValue(ordinal));
		}
	}
}
