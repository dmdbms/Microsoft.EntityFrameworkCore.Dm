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

		public static EventDefinition<string, string> LogDefaultDecimalTypeColumn([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogDefaultDecimalTypeColumn;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogDefaultDecimalTypeColumn, () => new EventDefinition<string, string>(logger.Options, DmEventId.DecimalTypeDefaultWarning, LogLevel.Warning, "DmEventId.DecimalTypeDefaultWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.DecimalTypeDefaultWarning, _resourceManager.GetString("LogDefaultDecimalTypeColumn"))));
			}
			return (EventDefinition<string, string>)eventDefinitionBase;
		}

		public static EventDefinition<string, string> LogByteIdentityColumn([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogByteIdentityColumn;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogByteIdentityColumn, () => new EventDefinition<string, string>(logger.Options, DmEventId.ByteIdentityColumnWarning, LogLevel.Warning, "DmEventId.ByteIdentityColumnWarning", (LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.ByteIdentityColumnWarning, _resourceManager.GetString("LogByteIdentityColumn"))));
			}
			return (EventDefinition<string, string>)eventDefinitionBase;
		}

		public static EventDefinition<string> LogFoundDefaultSchema([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundDefaultSchema;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundDefaultSchema, () => new EventDefinition<string>(logger.Options, DmEventId.DefaultSchemaFound, LogLevel.Debug, "DmEventId.DefaultSchemaFound", (LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.DefaultSchemaFound, _resourceManager.GetString("LogFoundDefaultSchema"))));
			}
			return (EventDefinition<string>)eventDefinitionBase;
		}

		public static FallbackEventDefinition LogFoundColumn([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundColumn;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundColumn, () => new FallbackEventDefinition(logger.Options, DmEventId.ColumnFound, LogLevel.Debug, "DmEventId.ColumnFound", _resourceManager.GetString("LogFoundColumn")));
			}
			return (FallbackEventDefinition)eventDefinitionBase;
		}

		public static EventDefinition<string, string, string, string> LogFoundForeignKey([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundForeignKey;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundForeignKey, () => new EventDefinition<string, string, string, string>(logger.Options, DmEventId.ForeignKeyFound, LogLevel.Debug, "DmEventId.ForeignKeyFound", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, DmEventId.ForeignKeyFound, _resourceManager.GetString("LogFoundForeignKey"))));
			}
			return (EventDefinition<string, string, string, string>)eventDefinitionBase;
		}

		public static EventDefinition<string, string, bool> LogFoundIndex([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundIndex;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundIndex, () => new EventDefinition<string, string, bool>(logger.Options, DmEventId.IndexFound, LogLevel.Debug, "DmEventId.IndexFound", (LogLevel level) => LoggerMessage.Define<string, string, bool>(level, DmEventId.IndexFound, _resourceManager.GetString("LogFoundIndex"))));
			}
			return (EventDefinition<string, string, bool>)eventDefinitionBase;
		}

		public static EventDefinition<string, string> LogFoundPrimaryKey([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundPrimaryKey;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundPrimaryKey, () => new EventDefinition<string, string>(logger.Options, DmEventId.PrimaryKeyFound, LogLevel.Debug, "DmEventId.PrimaryKeyFound", (LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.PrimaryKeyFound, _resourceManager.GetString("LogFoundPrimaryKey"))));
			}
			return (EventDefinition<string, string>)eventDefinitionBase;
		}

		public static EventDefinition<string> LogFoundTable([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundTable;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundTable, () => new EventDefinition<string>(logger.Options, DmEventId.TableFound, LogLevel.Debug, "DmEventId.TableFound", (LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.TableFound, _resourceManager.GetString("LogFoundTable"))));
			}
			return (EventDefinition<string>)eventDefinitionBase;
		}

		public static EventDefinition<string, string> LogFoundUniqueConstraint([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundUniqueConstraint;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundUniqueConstraint, () => new EventDefinition<string, string>(logger.Options, DmEventId.UniqueConstraintFound, LogLevel.Debug, "DmEventId.UniqueConstraintFound", (LogLevel level) => LoggerMessage.Define<string, string>(level, DmEventId.UniqueConstraintFound, _resourceManager.GetString("LogFoundUniqueConstraint"))));
			}
			return (EventDefinition<string, string>)eventDefinitionBase;
		}

		public static EventDefinition<string> LogMissingSchema([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogMissingSchema;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogMissingSchema, () => new EventDefinition<string>(logger.Options, DmEventId.MissingSchemaWarning, LogLevel.Warning, "DmEventId.MissingSchemaWarning", (LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.MissingSchemaWarning, _resourceManager.GetString("LogMissingSchema"))));
			}
			return (EventDefinition<string>)eventDefinitionBase;
		}

		public static EventDefinition<string> LogMissingTable([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogMissingTable;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogMissingTable, () => new EventDefinition<string>(logger.Options, DmEventId.MissingTableWarning, LogLevel.Warning, "DmEventId.MissingTableWarning", (LogLevel level) => LoggerMessage.Define<string>(level, DmEventId.MissingTableWarning, _resourceManager.GetString("LogMissingTable"))));
			}
			return (EventDefinition<string>)eventDefinitionBase;
		}

		public static EventDefinition<string, string, string, string> LogPrincipalColumnNotFound([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogPrincipalColumnNotFound;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogPrincipalColumnNotFound, () => new EventDefinition<string, string, string, string>(logger.Options, DmEventId.ForeignKeyPrincipalColumnMissingWarning, LogLevel.Warning, "DmEventId.ForeignKeyPrincipalColumnMissingWarning", (LogLevel level) => LoggerMessage.Define<string, string, string, string>(level, DmEventId.ForeignKeyPrincipalColumnMissingWarning, _resourceManager.GetString("LogPrincipalColumnNotFound"))));
			}
			return (EventDefinition<string, string, string, string>)eventDefinitionBase;
		}

		public static EventDefinition<string, string, string> LogPrincipalTableNotInSelectionSet([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogPrincipalTableNotInSelectionSet;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogPrincipalTableNotInSelectionSet, () => new EventDefinition<string, string, string>(logger.Options, DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning, LogLevel.Warning, "DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning", (LogLevel level) => LoggerMessage.Define<string, string, string>(level, DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning, _resourceManager.GetString("LogPrincipalTableNotInSelectionSet"))));
			}
			return (EventDefinition<string, string, string>)eventDefinitionBase;
		}

		public static FallbackEventDefinition LogFoundSequence([JetBrains.Annotations.NotNull] IDiagnosticsLogger logger)
		{
			EventDefinitionBase eventDefinitionBase = ((DmLoggingDefinitions)logger.Definitions).LogFoundSequence;
			if (eventDefinitionBase == null)
			{
				eventDefinitionBase = LazyInitializer.EnsureInitialized(ref ((DmLoggingDefinitions)logger.Definitions).LogFoundSequence, () => new FallbackEventDefinition(logger.Options, DmEventId.SequenceFound, LogLevel.Debug, "DmEventId.SequenceFound", _resourceManager.GetString("LogFoundSequence")));
			}
			return (FallbackEventDefinition)eventDefinitionBase;
		}
	}
}
