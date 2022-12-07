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

		private readonly IRelationalCommandDiagnosticsLogger _commandLogger;

		public virtual IDmValueGeneratorCache Cache => (IDmValueGeneratorCache)((ValueGeneratorSelector)this).Cache;

		public DmValueGeneratorSelector([NotNull] ValueGeneratorSelectorDependencies dependencies, [NotNull] IDmSequenceValueGeneratorFactory sequenceFactory, [NotNull] IDmRelationalConnection connection, [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder, [NotNull] IRelationalCommandDiagnosticsLogger commandLogger)
			: base(dependencies)
		{
			_sequenceFactory = sequenceFactory;
			_connection = connection;
			_rawSqlCommandBuilder = rawSqlCommandBuilder;
			_commandLogger = commandLogger;
		}

		public override ValueGenerator Select(IProperty property, IEntityType entityType)
		{
			Check.NotNull<IProperty>(property, "property");
			Check.NotNull<IEntityType>(entityType, "entityType");
			return (ValueGenerator)((((IReadOnlyProperty)property).GetValueGeneratorFactory() == null && ((IReadOnlyProperty)(object)property).GetValueGenerationStrategy() == DmValueGenerationStrategy.SequenceHiLo) ? ((object)_sequenceFactory.Create(property, Cache.GetOrAddSequenceState(property), _connection, _rawSqlCommandBuilder, _commandLogger)) : ((object)((ValueGeneratorSelector)this).Select(property, entityType)));
		}

		public override ValueGenerator Create(IProperty property, IEntityType entityType)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			Check.NotNull<IProperty>(property, "property");
			Check.NotNull<IEntityType>(entityType, "entityType");
			return (ValueGenerator)((!(((IReadOnlyPropertyBase)property).ClrType.UnwrapNullableType() == typeof(Guid))) ? ((RelationalValueGeneratorSelector)this).Create(property, entityType) : (((int)((IReadOnlyProperty)property).ValueGenerated == 0 || RelationalPropertyExtensions.GetDefaultValueSql((IReadOnlyProperty)(object)property) != null) ? ((object)new TemporaryGuidValueGenerator()) : ((object)new GuidValueGenerator())));
		}
	}
}
