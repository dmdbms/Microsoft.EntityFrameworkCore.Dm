// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Update.Internal.DmModificationCommandBatch
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Update.Internal
{
  public class DmModificationCommandBatch : AffectedCountModificationCommandBatch
  {
    private const int DefaultNetworkPacketSizeBytes = 4096;
    private const int MaxScriptLength = 134217728;
    private const int MaxParameterCount = 2100;
    private const int MaxRowCount = 1000;
    private int _parameterCount = 1;
    private readonly int _maxBatchSize;
    private readonly List<IReadOnlyModificationCommand> _bulkInsertCommands = new List<IReadOnlyModificationCommand>();
    private int _commandsLeftToLengthCheck = 50;

    public DmModificationCommandBatch(
      [NotNull] ModificationCommandBatchFactoryDependencies dependencies,
      int? maxBatchSize)
      : base(dependencies)
    {
      if (maxBatchSize.HasValue && maxBatchSize.Value <= 0)
        throw new ArgumentOutOfRangeException(nameof (maxBatchSize), RelationalStrings.InvalidMaxBatchSize((object) maxBatchSize.Value));
      this._maxBatchSize = Math.Min(maxBatchSize ?? int.MaxValue, 1000);
    }

    protected override IDmUpdateSqlGenerator UpdateSqlGenerator => (IDmUpdateSqlGenerator) base.UpdateSqlGenerator;

    protected override bool CanAddCommand(IReadOnlyModificationCommand modificationCommand)
    {
      if (((IReadOnlyCollection<IReadOnlyModificationCommand>) ((ModificationCommandBatch) this).ModificationCommands).Count >= this._maxBatchSize)
        return false;
      int num = DmModificationCommandBatch.CountParameters(modificationCommand);
      if (this._parameterCount + num >= 2100)
        return false;
      this._parameterCount += num;
      return true;
    }

    protected override bool IsCommandTextValid()
    {
      if (--this._commandsLeftToLengthCheck < 0)
      {
        int length = base.GetCommandText().Length;
        if (length >= 134217728)
          return false;
        int num = length / ((IReadOnlyCollection<IReadOnlyModificationCommand>) ((ModificationCommandBatch) this).ModificationCommands).Count;
        this._commandsLeftToLengthCheck = Math.Max(1, (134217728 - length) / num / 4);
      }
      return true;
    }

    protected override int GetParameterCount() => this._parameterCount;

    private static int CountParameters(IReadOnlyModificationCommand modificationCommand)
    {
      int num = 0;
      for (int index = 0; index < ((IReadOnlyCollection<IColumnModification>) modificationCommand.ColumnModifications).Count; ++index)
      {
        IColumnModification columnModification = modificationCommand.ColumnModifications[index];
        if (columnModification.UseCurrentValueParameter)
          ++num;
        if (columnModification.UseOriginalValueParameter)
          ++num;
      }
      return num;
    }

    protected virtual void ResetCommandText()
    {
      base.ResetCommandText();
      this._bulkInsertCommands.Clear();
    }

    protected virtual string GetCommandText() => ((IReadOnlyCollection<IReadOnlyModificationCommand>) ((ModificationCommandBatch) this).ModificationCommands).Count > 1 ? "BEGIN " + base.GetCommandText() + this.GetBulkInsertCommandText(((IReadOnlyCollection<IReadOnlyModificationCommand>) ((ModificationCommandBatch) this).ModificationCommands).Count) + " END; " : base.GetCommandText() + this.GetBulkInsertCommandText(((IReadOnlyCollection<IReadOnlyModificationCommand>) ((ModificationCommandBatch) this).ModificationCommands).Count);

    private string GetBulkInsertCommandText(int lastIndex)
    {
      if (this._bulkInsertCommands.Count == 0)
        return string.Empty;
      StringBuilder commandStringBuilder = new StringBuilder();
      ResultSetMapping resultSetMapping = this.UpdateSqlGenerator.AppendBulkInsertOperation(commandStringBuilder, (IReadOnlyList<IReadOnlyModificationCommand>) this._bulkInsertCommands, lastIndex - this._bulkInsertCommands.Count);
      for (int index = lastIndex - this._bulkInsertCommands.Count; index < lastIndex; ++index)
        base.CommandResultSet[index] = resultSetMapping;
      if (resultSetMapping > 0)
       base.CommandResultSet[lastIndex - 1] = (ResultSetMapping) 2;
      return commandStringBuilder.ToString();
    }

    protected override void UpdateCachedCommandText(int commandPosition)
    {
      IReadOnlyModificationCommand modificationCommand = ((ModificationCommandBatch) this).ModificationCommands[commandPosition];
      if (modificationCommand.EntityState == (EntityState)4)
      {
        if (this._bulkInsertCommands.Count > 0 && !DmModificationCommandBatch.CanBeInsertedInSameStatement(this._bulkInsertCommands[0], modificationCommand))
        {
         base.CachedCommandText.Append(this.GetBulkInsertCommandText(commandPosition));
          this._bulkInsertCommands.Clear();
        }
        this._bulkInsertCommands.Add(modificationCommand);
                base.LastCachedCommandIndex = commandPosition;
      }
      else
      {
                base.CachedCommandText.Append(this.GetBulkInsertCommandText(commandPosition));
        this._bulkInsertCommands.Clear();
                base.UpdateCachedCommandText(commandPosition);
      }
    }

    private static bool CanBeInsertedInSameStatement(
      IReadOnlyModificationCommand firstCommand,
      IReadOnlyModificationCommand secondCommand)
    {
      return string.Equals(firstCommand.TableName, secondCommand.TableName, StringComparison.Ordinal) && string.Equals(firstCommand.Schema, secondCommand.Schema, StringComparison.Ordinal) && ((IEnumerable<IColumnModification>) firstCommand.ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)).Select<IColumnModification, string>((Func<IColumnModification, string>) (o => o.ColumnName)).SequenceEqual<string>(((IEnumerable<IColumnModification>) secondCommand.ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsWrite)).Select<IColumnModification, string>((Func<IColumnModification, string>) (o => o.ColumnName))) && ((IEnumerable<IColumnModification>) firstCommand.ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsRead)).Select<IColumnModification, string>((Func<IColumnModification, string>) (o => o.ColumnName)).SequenceEqual<string>(((IEnumerable<IColumnModification>) secondCommand.ColumnModifications).Where<IColumnModification>((Func<IColumnModification, bool>) (o => o.IsRead)).Select<IColumnModification, string>((Func<IColumnModification, string>) (o => o.ColumnName)));
    }
  }
}
