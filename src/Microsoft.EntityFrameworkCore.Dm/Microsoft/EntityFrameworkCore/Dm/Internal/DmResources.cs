using System;
using System.Reflection;
using System.Resources;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Diagnostics.Internal;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Dm.Internal
{
	public static class DmResources
	{
		private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Dm.Properties.DmStrings", typeof(DmResources).GetTypeInfo().Assembly);

		public static EventDefinition<string, string> LogDefaultDecimalTypeColumn([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogDefaultDecimalTypeColumn;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogDefaultDecimalTypeColumn, () => (EventDefinitionBase)(object)new EventDefinition<string, string>(logger.Options, DmEventId.DecimalTypeDefaultWarning, LogLevel.Warning, "DmEventId.DecimalTypeDefaultWarning", (Func<LogLevel, Action<ILogger, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.DecimalTypeDefaultWarning, _resourceManager.GetString("LogDefaultDecimalTypeColumn")))));
			}
			return (EventDefinition<string, string>)(object)val;
		}

		public static EventDefinition<string, string> LogByteIdentityColumn([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogByteIdentityColumn;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogByteIdentityColumn, () => (EventDefinitionBase)(object)new EventDefinition<string, string>(logger.Options, DmEventId.ByteIdentityColumnWarning, LogLevel.Warning, "DmEventId.ByteIdentityColumnWarning", (Func<LogLevel, Action<ILogger, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.ByteIdentityColumnWarning, _resourceManager.GetString("LogByteIdentityColumn")))));
			}
			return (EventDefinition<string, string>)(object)val;
		}

		public static EventDefinition<string> LogFoundDefaultSchema([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundDefaultSchema;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundDefaultSchema, () => (EventDefinitionBase)(object)new EventDefinition<string>(logger.Options, DmEventId.DefaultSchemaFound, LogLevel.Debug, "DmEventId.DefaultSchemaFound", (Func<LogLevel, Action<ILogger, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.DefaultSchemaFound, _resourceManager.GetString("LogFoundDefaultSchema")))));
			}
			return (EventDefinition<string>)(object)val;
		}

		public static FallbackEventDefinition LogFoundColumn([NotNull] IDiagnosticsLogger logger)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundColumn;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundColumn, () => (EventDefinitionBase)new FallbackEventDefinition(logger.Options, DmEventId.ColumnFound, LogLevel.Debug, "DmEventId.ColumnFound", _resourceManager.GetString("LogFoundColumn")));
			}
			return (FallbackEventDefinition)val;
		}

		public static EventDefinition<string, string, string, string> LogFoundForeignKey([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundForeignKey;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundForeignKey, () => (EventDefinitionBase)(object)new EventDefinition<string, string, string, string>(logger.Options, DmEventId.ForeignKeyFound, LogLevel.Debug, "DmEventId.ForeignKeyFound", (Func<LogLevel, Action<ILogger, string, string, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, DmEventId.ForeignKeyFound, _resourceManager.GetString("LogFoundForeignKey")))));
			}
			return (EventDefinition<string, string, string, string>)(object)val;
		}

		public static EventDefinition<string, string, bool> LogFoundIndex([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundIndex;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundIndex, () => (EventDefinitionBase)(object)new EventDefinition<string, string, bool>(logger.Options, DmEventId.IndexFound, LogLevel.Debug, "DmEventId.IndexFound", (Func<LogLevel, Action<ILogger, string, string, bool, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string, bool>(level, DmEventId.IndexFound, _resourceManager.GetString("LogFoundIndex")))));
			}
			return (EventDefinition<string, string, bool>)(object)val;
		}

		public static EventDefinition<string, string> LogFoundPrimaryKey([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundPrimaryKey;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundPrimaryKey, () => (EventDefinitionBase)(object)new EventDefinition<string, string>(logger.Options, DmEventId.PrimaryKeyFound, LogLevel.Debug, "DmEventId.PrimaryKeyFound", (Func<LogLevel, Action<ILogger, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.PrimaryKeyFound, _resourceManager.GetString("LogFoundPrimaryKey")))));
			}
			return (EventDefinition<string, string>)(object)val;
		}

		public static EventDefinition<string> LogFoundTable([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundTable;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundTable, () => (EventDefinitionBase)(object)new EventDefinition<string>(logger.Options, DmEventId.TableFound, LogLevel.Debug, "DmEventId.TableFound", (Func<LogLevel, Action<ILogger, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.TableFound, _resourceManager.GetString("LogFoundTable")))));
			}
			return (EventDefinition<string>)(object)val;
		}

		public static EventDefinition<string, string> LogFoundUniqueConstraint([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundUniqueConstraint;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundUniqueConstraint, () => (EventDefinitionBase)(object)new EventDefinition<string, string>(logger.Options, DmEventId.UniqueConstraintFound, LogLevel.Debug, "DmEventId.UniqueConstraintFound", (Func<LogLevel, Action<ILogger, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.UniqueConstraintFound, _resourceManager.GetString("LogFoundUniqueConstraint")))));
			}
			return (EventDefinition<string, string>)(object)val;
		}

		public static EventDefinition<string> LogMissingSchema([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogMissingSchema;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogMissingSchema, () => (EventDefinitionBase)(object)new EventDefinition<string>(logger.Options, DmEventId.MissingSchemaWarning, LogLevel.Warning, "DmEventId.MissingSchemaWarning", (Func<LogLevel, Action<ILogger, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.MissingSchemaWarning, _resourceManager.GetString("LogMissingSchema")))));
			}
			return (EventDefinition<string>)(object)val;
		}

		public static EventDefinition<string> LogMissingTable([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogMissingTable;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogMissingTable, () => (EventDefinitionBase)(object)new EventDefinition<string>(logger.Options, DmEventId.MissingTableWarning, LogLevel.Warning, "DmEventId.MissingTableWarning", (Func<LogLevel, Action<ILogger, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.MissingTableWarning, _resourceManager.GetString("LogMissingTable")))));
			}
			return (EventDefinition<string>)(object)val;
		}

		public static EventDefinition<string, string, string, string> LogPrincipalColumnNotFound([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogPrincipalColumnNotFound;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogPrincipalColumnNotFound, () => (EventDefinitionBase)(object)new EventDefinition<string, string, string, string>(logger.Options, DmEventId.ForeignKeyPrincipalColumnMissingWarning, LogLevel.Warning, "DmEventId.ForeignKeyPrincipalColumnMissingWarning", (Func<LogLevel, Action<ILogger, string, string, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, DmEventId.ForeignKeyPrincipalColumnMissingWarning, _resourceManager.GetString("LogPrincipalColumnNotFound")))));
			}
			return (EventDefinition<string, string, string, string>)(object)val;
		}

		public static EventDefinition<string, string, string> LogPrincipalTableNotInSelectionSet([NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogPrincipalTableNotInSelectionSet;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogPrincipalTableNotInSelectionSet, () => (EventDefinitionBase)(object)new EventDefinition<string, string, string>(logger.Options, DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning, LogLevel.Warning, "DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning", (Func<LogLevel, Action<ILogger, string, string, string, Exception>>)((LogLevel level) => LoggerMessage.Define<string, string, string>(level, DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning, _resourceManager.GetString("LogPrincipalTableNotInSelectionSet")))));
			}
			return (EventDefinition<string, string, string>)(object)val;
		}

		public static FallbackEventDefinition LogFoundSequence([NotNull] IDiagnosticsLogger logger)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			EventDefinitionBase val = ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundSequence;
			if (val == null)
			{
				val = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)(object)logger.Definitions).LogFoundSequence, () => (EventDefinitionBase)new FallbackEventDefinition(logger.Options, DmEventId.SequenceFound, LogLevel.Debug, "DmEventId.SequenceFound", _resourceManager.GetString("LogFoundSequence")));
			}
			return (FallbackEventDefinition)val;
		}
	}
}
