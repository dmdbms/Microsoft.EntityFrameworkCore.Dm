using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Dm.Update.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
	public class DmSequenceValueGeneratorFactory : IDmSequenceValueGeneratorFactory
	{
		private readonly IDmUpdateSqlGenerator _sqlGenerator;

		public DmSequenceValueGeneratorFactory([NotNull] IDmUpdateSqlGenerator sqlGenerator)
		{
			Check.NotNull(sqlGenerator, "sqlGenerator");
			_sqlGenerator = sqlGenerator;
		}

		public virtual ValueGenerator Create(IProperty property, DmSequenceValueGeneratorState generatorState, IDmRelationalConnection connection, IRawSqlCommandBuilder rawSqlCommandBuilder, IRelationalCommandDiagnosticsLogger commandLogger)
		{
			Check.NotNull<IProperty>(property, "property");
			Check.NotNull(generatorState, "generatorState");
			Check.NotNull(connection, "connection");
			Type type = ((IReadOnlyPropertyBase)property).ClrType.UnwrapNullableType().UnwrapEnumType();
			if (type == typeof(long))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<long>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(int))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<int>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(short))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<short>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(byte))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<byte>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(char))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<char>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(ulong))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<ulong>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(uint))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<uint>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(ushort))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<ushort>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			if (type == typeof(sbyte))
			{
				return (ValueGenerator)(object)new DmSequenceHiLoValueGenerator<sbyte>(rawSqlCommandBuilder, _sqlGenerator, generatorState, connection, commandLogger);
			}
			throw new ArgumentException(CoreStrings.InvalidValueGeneratorFactoryProperty((object)"DmSequenceValueGeneratorFactory", (object)((IReadOnlyPropertyBase)property).Name, (object)((IReadOnlyTypeBase)property.DeclaringEntityType).DisplayName()));
		}
	}
}
