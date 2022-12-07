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
		ValueGenerator Create([NotNull] IProperty property, [NotNull] DmSequenceValueGeneratorState generatorState, [NotNull] IDmRelationalConnection connection, [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [NotNull] IRelationalCommandDiagnosticsLogger commandLogger);
	}
}
