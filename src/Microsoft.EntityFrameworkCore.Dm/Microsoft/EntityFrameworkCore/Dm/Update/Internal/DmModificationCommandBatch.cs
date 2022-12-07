using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Update;

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

		protected virtual IDmUpdateSqlGenerator UpdateSqlGenerator => (IDmUpdateSqlGenerator)base.UpdateSqlGenerator;

		public DmModificationCommandBatch([NotNull] ModificationCommandBatchFactoryDependencies dependencies, int? maxBatchSize)
			: base(dependencies)
		{
			if (maxBatchSize.HasValue && maxBatchSize.Value <= 0)
			{
				throw new ArgumentOutOfRangeException("maxBatchSize", RelationalStrings.InvalidMaxBatchSize((object)maxBatchSize.Value));
			}
			_maxBatchSize = Math.Min(maxBatchSize ?? int.MaxValue, 1000);
		}

		protected override bool CanAddCommand(IReadOnlyModificationCommand modificationCommand)
		{
			if (((ModificationCommandBatch)this).ModificationCommands.Count >= _maxBatchSize)
			{
				return false;
			}
			int num = CountParameters(modificationCommand);
			if (_parameterCount + num >= 2100)
			{
				return false;
			}
			_parameterCount += num;
			return true;
		}

		protected override bool IsCommandTextValid()
		{
			if (--_commandsLeftToLengthCheck < 0)
			{
				int length = base.GetCommandText().Length;
				if (length >= 134217728)
				{
					return false;
				}
				int num = length / ((ModificationCommandBatch)this).ModificationCommands.Count;
				int num2 = (134217728 - length) / num;
				_commandsLeftToLengthCheck = Math.Max(1, num2 / 4);
			}
			return true;
		}

		protected override int GetParameterCount()
		{
			return _parameterCount;
		}

		private static int CountParameters(IReadOnlyModificationCommand modificationCommand)
		{
			int num = 0;
			for (int i = 0; i < modificationCommand.ColumnModifications.Count; i++)
			{
				IColumnModification val = modificationCommand.ColumnModifications[i];
				if (val.UseCurrentValueParameter)
				{
					num++;
				}
				if (val.UseOriginalValueParameter)
				{
					num++;
				}
			}
			return num;
		}

		protected override void ResetCommandText()
		{
			base.ResetCommandText();
			_bulkInsertCommands.Clear();
		}

		protected override string GetCommandText()
		{
			if (((ModificationCommandBatch)this).ModificationCommands.Count > 1)
			{
				return "BEGIN " + base.GetCommandText() + GetBulkInsertCommandText(((ModificationCommandBatch)this).ModificationCommands.Count) + " END; ";
			}
			return base.GetCommandText() + GetBulkInsertCommandText(((ModificationCommandBatch)this).ModificationCommands.Count);
		}

		private string GetBulkInsertCommandText(int lastIndex)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Invalid comparison between Unknown and I4
			if (_bulkInsertCommands.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			ResultSetMapping val = UpdateSqlGenerator.AppendBulkInsertOperation(stringBuilder, _bulkInsertCommands, lastIndex - _bulkInsertCommands.Count);
			for (int i = lastIndex - _bulkInsertCommands.Count; i < lastIndex; i++)
			{
				this.CommandResultSet[i] = val;
			}
			if ((int)val > 0)
			{
                this.CommandResultSet[lastIndex - 1] = (ResultSetMapping)2;
			}
			return stringBuilder.ToString();
		}

		protected override void UpdateCachedCommandText(int commandPosition)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Invalid comparison between Unknown and I4
			IReadOnlyModificationCommand val = ((ModificationCommandBatch)this).ModificationCommands[commandPosition];
			if ((int)val.EntityState == 4)
			{
				if (_bulkInsertCommands.Count > 0 && !CanBeInsertedInSameStatement(_bulkInsertCommands[0], val))
				{
					this.CachedCommandText.Append(GetBulkInsertCommandText(commandPosition));
					_bulkInsertCommands.Clear();
				}
				_bulkInsertCommands.Add(val);
                this.LastCachedCommandIndex=commandPosition;
			}
			else
			{
				this.CachedCommandText.Append(GetBulkInsertCommandText(commandPosition));
				_bulkInsertCommands.Clear();
                this.UpdateCachedCommandText(commandPosition);
			}
		}

		private static bool CanBeInsertedInSameStatement(IReadOnlyModificationCommand firstCommand, IReadOnlyModificationCommand secondCommand)
		{
			return string.Equals(firstCommand.TableName, secondCommand.TableName, StringComparison.Ordinal) && string.Equals(firstCommand.Schema, secondCommand.Schema, StringComparison.Ordinal) && (from o in firstCommand.ColumnModifications
				where o.IsWrite
				select o.ColumnName).SequenceEqual(from o in secondCommand.ColumnModifications
				where o.IsWrite
				select o.ColumnName) && (from o in firstCommand.ColumnModifications
				where o.IsRead
				select o.ColumnName).SequenceEqual(from o in secondCommand.ColumnModifications
				where o.IsRead
				select o.ColumnName);
		}
	}
}
