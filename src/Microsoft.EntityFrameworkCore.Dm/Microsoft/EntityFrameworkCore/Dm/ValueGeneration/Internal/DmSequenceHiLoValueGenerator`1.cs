// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal.DmSequenceHiLoValueGenerator`1
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Storage.Internal;
using Microsoft.EntityFrameworkCore.Dm.Update.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;



namespace Microsoft.EntityFrameworkCore.Dm.ValueGeneration.Internal
{
  public class DmSequenceHiLoValueGenerator<TValue> : HiLoValueGenerator<TValue>
  {
    private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
    private readonly IDmUpdateSqlGenerator _sqlGenerator;
    private readonly IDmRelationalConnection _connection;
    private readonly IReadOnlySequence _sequence;
    private readonly IRelationalCommandDiagnosticsLogger _commandLogger;

    public DmSequenceHiLoValueGenerator(
      [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder,
      [NotNull] IDmUpdateSqlGenerator sqlGenerator,
      [NotNull] DmSequenceValueGeneratorState generatorState,
      [NotNull] IDmRelationalConnection connection,
      [NotNull] IRelationalCommandDiagnosticsLogger commandLogger)
      : base((HiLoValueGeneratorState) generatorState)
    {
      this._sequence = generatorState.Sequence;
      this._rawSqlCommandBuilder = rawSqlCommandBuilder;
      this._sqlGenerator = sqlGenerator;
      this._connection = connection;
      this._commandLogger = commandLogger;
    }

    protected override long GetNewLowValue() => (long) Convert.ChangeType(this._rawSqlCommandBuilder.Build(((IUpdateSqlGenerator) this._sqlGenerator).GenerateNextSequenceValueOperation(this._sequence.Name, this._sequence.Schema)).ExecuteScalar(new RelationalCommandParameterObject((IRelationalConnection) this._connection, (IReadOnlyDictionary<string, object>) null, (IReadOnlyList<ReaderColumn>) null, (DbContext) null, this._commandLogger, (CommandSource) 6)), typeof (long), (IFormatProvider) CultureInfo.InvariantCulture);

    protected override async Task<long> GetNewLowValueAsync(CancellationToken cancellationToken = default (CancellationToken))
    {
      object obj = await this._rawSqlCommandBuilder.Build(((IUpdateSqlGenerator) this._sqlGenerator).GenerateNextSequenceValueOperation(this._sequence.Name, this._sequence.Schema)).ExecuteScalarAsync(new RelationalCommandParameterObject((IRelationalConnection) this._connection, (IReadOnlyDictionary<string, object>) null, (IReadOnlyList<ReaderColumn>) null, (DbContext) null, this._commandLogger), cancellationToken);
      return (long) Convert.ChangeType(obj, typeof (long), (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public override bool GeneratesTemporaryValues => false;
  }
}
