using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public interface IDmSequenceValueGeneratorFactory
	{
		ValueGenerator Create([JetBrains.Annotations.NotNull] IProperty property, [JetBrains.Annotations.NotNull] DmSequenceValueGeneratorState generatorState, [JetBrains.Annotations.NotNull] IDmRelationalConnection connection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger);
	}
}
