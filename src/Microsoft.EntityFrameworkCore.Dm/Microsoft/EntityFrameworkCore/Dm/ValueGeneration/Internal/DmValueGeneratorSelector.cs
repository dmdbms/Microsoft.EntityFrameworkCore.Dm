using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public class DmValueGeneratorSelector : RelationalValueGeneratorSelector
	{
		private readonly IDmSequenceValueGeneratorFactory _sequenceFactory;

		private readonly IDmRelationalConnection _connection;

		private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;

		private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _commandLogger;

		public new virtual IDmValueGeneratorCache Cache => (IDmValueGeneratorCache)base.Cache;

		public DmValueGeneratorSelector([JetBrains.Annotations.NotNull] ValueGeneratorSelectorDependencies dependencies, [JetBrains.Annotations.NotNull] IDmSequenceValueGeneratorFactory sequenceFactory, [JetBrains.Annotations.NotNull] IDmRelationalConnection connection, [JetBrains.Annotations.NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Database.Command> commandLogger)
			: base(dependencies)
		{
			_sequenceFactory = sequenceFactory;
			_connection = connection;
			_rawSqlCommandBuilder = rawSqlCommandBuilder;
			_commandLogger = commandLogger;
		}

		public override ValueGenerator Select(IProperty property, IEntityType entityType)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
			return (property.GetValueGeneratorFactory() == null && property.GetValueGenerationStrategy() == DmValueGenerationStrategy.SequenceHiLo) ? _sequenceFactory.Create(property, Cache.GetOrAddSequenceState(property), _connection, _rawSqlCommandBuilder, _commandLogger) : base.Select(property, entityType);
		}

		public override ValueGenerator Create(IProperty property, IEntityType entityType)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(property, "property");
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(entityType, "entityType");
			return (!(property.ClrType.UnwrapNullableType() == typeof(Guid))) ? base.Create(property, entityType) : ((property.ValueGenerated == ValueGenerated.Never || property.GetDefaultValueSql() != null) ? new TemporaryGuidValueGenerator() : new GuidValueGenerator());
		}
	}
}
