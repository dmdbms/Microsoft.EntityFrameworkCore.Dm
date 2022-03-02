using System.Data.Common;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
	public static class DmDataReaderExtension
	{
		public static T GetValueOrDefault<T>([JetBrains.Annotations.NotNull] this DbDataReader reader, [JetBrains.Annotations.NotNull] string name)
		{
			int ordinal = reader.GetOrdinal(name);
			return reader.IsDBNull(ordinal) ? default(T) : reader.GetFieldValue<T>(ordinal);
		}

		public static T GetValueOrDefault<T>([JetBrains.Annotations.NotNull] this DbDataRecord record, [JetBrains.Annotations.NotNull] string name)
		{
			int ordinal = record.GetOrdinal(name);
			return record.IsDBNull(ordinal) ? default(T) : ((T)record.GetValue(ordinal));
		}
	}
}
