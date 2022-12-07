// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal.DmValueGeneratorSelector
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;



namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
  public class DmValueGeneratorSelector : RelationalValueGeneratorSelector
  {
    private readonly IDmSequenceValueGeneratorFactory _sequenceFactory;
    private readonly IDmRelationalConnection _connection;
    private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
    private readonly IRelationalCommandDiagnosticsLogger _commandLogger;

    public DmValueGeneratorSelector(
      [NotNull] ValueGeneratorSelectorDependencies dependencies,
      [NotNull] IDmSequenceValueGeneratorFactory sequenceFactory,
      [NotNull] IDmRelationalConnection connection,
      [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder,
      [NotNull] IRelationalCommandDiagnosticsLogger commandLogger)
      : base(dependencies)
    {
      this._sequenceFactory = sequenceFactory;
      this._connection = connection;
      this._rawSqlCommandBuilder = rawSqlCommandBuilder;
      this._commandLogger = commandLogger;
    }

    public virtual IDmValueGeneratorCache Cache => (IDmValueGeneratorCache) ((ValueGeneratorSelector) this).Cache;

    public virtual ValueGenerator Select(IProperty property, IEntityType entityType)
    {
      Check.NotNull<IProperty>(property, nameof (property));
      Check.NotNull<IEntityType>(entityType, nameof (entityType));
      return ((IReadOnlyProperty) property).GetValueGeneratorFactory() != null || ((IReadOnlyProperty) property).GetValueGenerationStrategy() != DmValueGenerationStrategy.SequenceHiLo ? ((ValueGeneratorSelector) this).Select(property, entityType) : this._sequenceFactory.Create(property, this.Cache.GetOrAddSequenceState(property), this._connection, this._rawSqlCommandBuilder, this._commandLogger);
    }

    public virtual ValueGenerator Create(IProperty property, IEntityType entityType)
    {
      Check.NotNull<IProperty>(property, nameof (property));
      Check.NotNull<IEntityType>(entityType, nameof (entityType));
      return ((IReadOnlyPropertyBase) property).ClrType.UnwrapNullableType() == typeof (Guid) ? (((IReadOnlyProperty) property).ValueGenerated == null || RelationalPropertyExtensions.GetDefaultValueSql((IReadOnlyProperty) property) != null ? (ValueGenerator) new TemporaryGuidValueGenerator() : (ValueGenerator) new GuidValueGenerator()) : base.Create(property, entityType);
    }
  }
}
