// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal.DmSequenceValueGeneratorFactory
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Dm.Update.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;



namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
  public class DmSequenceValueGeneratorFactory : IDmSequenceValueGeneratorFactory
  {
    private readonly IDmUpdateSqlGenerator _sqlGenerator;

    public DmSequenceValueGeneratorFactory([NotNull] IDmUpdateSqlGenerator sqlGenerator)
    {
      Check.NotNull<IDmUpdateSqlGenerator>(sqlGenerator, nameof (sqlGenerator));
      this._sqlGenerator = sqlGenerator;
    }

    public virtual ValueGenerator Create(
      IProperty property,
      DmSequenceValueGeneratorState generatorState,
      IDmRelationalConnection connection,
      IRawSqlCommandBuilder rawSqlCommandBuilder,
      IRelationalCommandDiagnosticsLogger commandLogger)
    {
      Check.NotNull<IProperty>(property, nameof (property));
      Check.NotNull<DmSequenceValueGeneratorState>(generatorState, nameof (generatorState));
      Check.NotNull<IDmRelationalConnection>(connection, nameof (connection));
      Type type = ((IReadOnlyPropertyBase) property).ClrType.UnwrapNullableType().UnwrapEnumType();
      if (type == typeof (long))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<long>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (int))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<int>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (short))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<short>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (byte))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<byte>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (char))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<char>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (ulong))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<ulong>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (uint))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<uint>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (ushort))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<ushort>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      if (type == typeof (sbyte))
        return (ValueGenerator) new DmSequenceHiLoValueGenerator<sbyte>(rawSqlCommandBuilder, this._sqlGenerator, generatorState, connection, commandLogger);
      throw new ArgumentException(CoreStrings.InvalidValueGeneratorFactoryProperty((object) nameof (DmSequenceValueGeneratorFactory), (object) ((IReadOnlyPropertyBase) property).Name, (object) ((IReadOnlyTypeBase) property.DeclaringEntityType).DisplayName()));
    }
  }
}
