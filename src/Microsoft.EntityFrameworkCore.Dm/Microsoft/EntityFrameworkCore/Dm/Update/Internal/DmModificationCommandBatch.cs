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

		private readonly List<ModificationCommand> _bulkInsertCommands = new List<ModificationCommand>();

		private int _commandsLeftToLengthCheck = 50;

		protected new virtual IDmUpdateSqlGenerator UpdateSqlGenerator => (IDmUpdateSqlGenerator)base.UpdateSqlGenerator;

		public DmModificationCommandBatch([JetBrains.Annotations.NotNull] ModificationCommandBatchFactoryDependencies dependencies, int? maxBatchSize)
			: base(dependencies)
		{
			if (maxBatchSize.HasValue && maxBatchSize.Value <= 0)
			{
				throw new ArgumentOutOfRangeException("maxBatchSize", RelationalStrings.InvalidMaxBatchSize);
			}
			_maxBatchSize = Math.Min(maxBatchSize ?? int.MaxValue, 1000);
		}

		protected override bool CanAddCommand(ModificationCommand modificationCommand)
		{
			if (ModificationCommands.Count >= _maxBatchSize)
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
				int length = GetCommandText().Length;
				if (length >= 134217728)
				{
					return false;
				}
				int num = length / ModificationCommands.Count;
				int num2 = (134217728 - length) / num;
				_commandsLeftToLengthCheck = Math.Max(1, num2 / 4);
			}
			return true;
		}

		protected override int GetParameterCount()
		{
			return _parameterCount;
		}

		private static int CountParameters(ModificationCommand modificationCommand)
		{
			int num = 0;
			for (int i = 0; i < modificationCommand.ColumnModifications.Count; i++)
			{
				ColumnModification columnModification = modificationCommand.ColumnModifications[i];
				if (columnModification.UseCurrentValueParameter)
				{
					num++;
				}
				if (columnModification.UseOriginalValueParameter)
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
			if (ModificationCommands.Count > 1)
			{
				return "BEGIN " + base.GetCommandText() + GetBulkInsertCommandText(ModificationCommands.Count) + " END; ";
			}
			return base.GetCommandText() + GetBulkInsertCommandText(ModificationCommands.Count);
		}

		private string GetBulkInsertCommandText(int lastIndex)
		{
			if (_bulkInsertCommands.Count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			ResultSetMapping resultSetMapping = UpdateSqlGenerator.AppendBulkInsertOperation(stringBuilder, _bulkInsertCommands, lastIndex - _bulkInsertCommands.Count);
			for (int i = lastIndex - _bulkInsertCommands.Count; i < lastIndex; i++)
			{
				CommandResultSet[i] = resultSetMapping;
			}
			if (resultSetMapping != 0)
			{
				CommandResultSet[lastIndex - 1] = ResultSetMapping.LastInResultSet;
			}
			return stringBuilder.ToString();
		}

		protected override void UpdateCachedCommandText(int commandPosition)
		{
			ModificationCommand modificationCommand = ModificationCommands[commandPosition];
			if (modificationCommand.EntityState == EntityState.Added)
			{
				if (_bulkInsertCommands.Count > 0 && !CanBeInsertedInSameStatement(_bulkInsertCommands[0], modificationCommand))
				{
					CachedCommandText.Append(GetBulkInsertCommandText(commandPosition));
					_bulkInsertCommands.Clear();
				}
				_bulkInsertCommands.Add(modificationCommand);
				LastCachedCommandIndex = commandPosition;
			}
			else
			{
				CachedCommandText.Append(GetBulkInsertCommandText(commandPosition));
				_bulkInsertCommands.Clear();
				base.UpdateCachedCommandText(commandPosition);
			}
		}

		private static bool CanBeInsertedInSameStatement(ModificationCommand firstCommand, ModificationCommand secondCommand)
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
