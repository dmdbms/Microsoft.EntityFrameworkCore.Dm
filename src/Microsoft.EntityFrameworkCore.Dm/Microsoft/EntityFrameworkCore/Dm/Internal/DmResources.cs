// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Internal.DmResources
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Diagnostics.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Resources;
using System.Threading;



namespace Microsoft.EntityFrameworkCore.Dm.Internal
{
  public static class DmResources
  {
    private static readonly 
    #nullable disable
    ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Dm.Properties.DmStrings", typeof (DmResources).GetTypeInfo().Assembly);

    public static EventDefinition<string, string> LogDefaultDecimalTypeColumn(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string>) (((DmLoggingDefinitions) logger.Definitions).LogDefaultDecimalTypeColumn ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogDefaultDecimalTypeColumn, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string>(logger.Options, DmEventId.DecimalTypeDefaultWarning, LogLevel.Warning, "DmEventId.DecimalTypeDefaultWarning", (Func<LogLevel, Action<ILogger, string, string, Exception>>) (level => LoggerMessage.Define<string, string>(level, DmEventId.DecimalTypeDefaultWarning, DmResources._resourceManager.GetString(nameof (LogDefaultDecimalTypeColumn))))))));
    }

    public static EventDefinition<string, string> LogByteIdentityColumn(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string>) (((DmLoggingDefinitions) logger.Definitions).LogByteIdentityColumn ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogByteIdentityColumn, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string>(logger.Options, DmEventId.ByteIdentityColumnWarning, LogLevel.Warning, "DmEventId.ByteIdentityColumnWarning", (Func<LogLevel, Action<ILogger, string, string, Exception>>) (level => LoggerMessage.Define<string, string>(level, DmEventId.ByteIdentityColumnWarning, DmResources._resourceManager.GetString(nameof (LogByteIdentityColumn))))))));
    }

    public static EventDefinition<string> LogFoundDefaultSchema(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string>) (((DmLoggingDefinitions) logger.Definitions).LogFoundDefaultSchema ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundDefaultSchema, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string>(logger.Options, DmEventId.DefaultSchemaFound, LogLevel.Debug, "DmEventId.DefaultSchemaFound", (Func<LogLevel, Action<ILogger, string, Exception>>) (level => LoggerMessage.Define<string>(level, DmEventId.DefaultSchemaFound, DmResources._resourceManager.GetString(nameof (LogFoundDefaultSchema))))))));
    }

    public static FallbackEventDefinition LogFoundColumn(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (FallbackEventDefinition) (((DmLoggingDefinitions) logger.Definitions).LogFoundColumn ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundColumn, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new FallbackEventDefinition(logger.Options, DmEventId.ColumnFound, LogLevel.Debug, "DmEventId.ColumnFound", DmResources._resourceManager.GetString(nameof (LogFoundColumn))))));
    }

    public static EventDefinition<string, string, string, string> LogFoundForeignKey(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string, string, string>) (((DmLoggingDefinitions) logger.Definitions).LogFoundForeignKey ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundForeignKey, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string, string, string>(logger.Options, DmEventId.ForeignKeyFound, LogLevel.Debug, "DmEventId.ForeignKeyFound", (Func<LogLevel, Action<ILogger, string, string, string, string, Exception>>) (level => LoggerMessage.Define<string, string, string, string>(level, DmEventId.ForeignKeyFound, DmResources._resourceManager.GetString(nameof (LogFoundForeignKey))))))));
    }

    public static EventDefinition<string, string, bool> LogFoundIndex(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string, bool>) (((DmLoggingDefinitions) logger.Definitions).LogFoundIndex ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundIndex, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string, bool>(logger.Options, DmEventId.IndexFound, LogLevel.Debug, "DmEventId.IndexFound", (Func<LogLevel, Action<ILogger, string, string, bool, Exception>>) (level => LoggerMessage.Define<string, string, bool>(level, DmEventId.IndexFound, DmResources._resourceManager.GetString(nameof (LogFoundIndex))))))));
    }

    public static EventDefinition<string, string> LogFoundPrimaryKey(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string>) (((DmLoggingDefinitions) logger.Definitions).LogFoundPrimaryKey ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundPrimaryKey, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string>(logger.Options, DmEventId.PrimaryKeyFound, LogLevel.Debug, "DmEventId.PrimaryKeyFound", (Func<LogLevel, Action<ILogger, string, string, Exception>>) (level => LoggerMessage.Define<string, string>(level, DmEventId.PrimaryKeyFound, DmResources._resourceManager.GetString(nameof (LogFoundPrimaryKey))))))));
    }

    public static EventDefinition<string> LogFoundTable([NotNull] IDiagnosticsLogger logger) => (EventDefinition<string>) (((DmLoggingDefinitions) logger.Definitions).LogFoundTable ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundTable, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string>(logger.Options, DmEventId.TableFound, LogLevel.Debug, "DmEventId.TableFound", (Func<LogLevel, Action<ILogger, string, Exception>>) (level => LoggerMessage.Define<string>(level, DmEventId.TableFound, DmResources._resourceManager.GetString(nameof (LogFoundTable))))))));

    public static EventDefinition<string, string> LogFoundUniqueConstraint(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string>) (((DmLoggingDefinitions) logger.Definitions).LogFoundUniqueConstraint ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundUniqueConstraint, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string>(logger.Options, DmEventId.UniqueConstraintFound, LogLevel.Debug, "DmEventId.UniqueConstraintFound", (Func<LogLevel, Action<ILogger, string, string, Exception>>) (level => LoggerMessage.Define<string, string>(level, DmEventId.UniqueConstraintFound, DmResources._resourceManager.GetString(nameof (LogFoundUniqueConstraint))))))));
    }

    public static EventDefinition<string> LogMissingSchema([NotNull] IDiagnosticsLogger logger) => (EventDefinition<string>) (((DmLoggingDefinitions) logger.Definitions).LogMissingSchema ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogMissingSchema, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string>(logger.Options, DmEventId.MissingSchemaWarning, LogLevel.Warning, "DmEventId.MissingSchemaWarning", (Func<LogLevel, Action<ILogger, string, Exception>>) (level => LoggerMessage.Define<string>(level, DmEventId.MissingSchemaWarning, DmResources._resourceManager.GetString(nameof (LogMissingSchema))))))));

    public static EventDefinition<string> LogMissingTable([NotNull] IDiagnosticsLogger logger) => (EventDefinition<string>) (((DmLoggingDefinitions) logger.Definitions).LogMissingTable ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogMissingTable, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string>(logger.Options, DmEventId.MissingTableWarning, LogLevel.Warning, "DmEventId.MissingTableWarning", (Func<LogLevel, Action<ILogger, string, Exception>>) (level => LoggerMessage.Define<string>(level, DmEventId.MissingTableWarning, DmResources._resourceManager.GetString(nameof (LogMissingTable))))))));

    public static EventDefinition<string, string, string, string> LogPrincipalColumnNotFound(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string, string, string>) (((DmLoggingDefinitions) logger.Definitions).LogPrincipalColumnNotFound ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogPrincipalColumnNotFound, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string, string, string>(logger.Options, DmEventId.ForeignKeyPrincipalColumnMissingWarning, LogLevel.Warning, "DmEventId.ForeignKeyPrincipalColumnMissingWarning", (Func<LogLevel, Action<ILogger, string, string, string, string, Exception>>) (level => LoggerMessage.Define<string, string, string, string>(level, DmEventId.ForeignKeyPrincipalColumnMissingWarning, DmResources._resourceManager.GetString(nameof (LogPrincipalColumnNotFound))))))));
    }

    public static EventDefinition<string, string, string> LogPrincipalTableNotInSelectionSet(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (EventDefinition<string, string, string>) (((DmLoggingDefinitions) logger.Definitions).LogPrincipalTableNotInSelectionSet ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogPrincipalTableNotInSelectionSet, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new EventDefinition<string, string, string>(logger.Options, DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning, LogLevel.Warning, "DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning", (Func<LogLevel, Action<ILogger, string, string, string, Exception>>) (level => LoggerMessage.Define<string, string, string>(level, DmEventId.ForeignKeyReferencesMissingPrincipalTableWarning, DmResources._resourceManager.GetString(nameof (LogPrincipalTableNotInSelectionSet))))))));
    }

    public static FallbackEventDefinition LogFoundSequence(
      [NotNull] IDiagnosticsLogger logger)
    {
      return (FallbackEventDefinition) (((DmLoggingDefinitions) logger.Definitions).LogFoundSequence ?? LazyInitializer.EnsureInitialized<EventDefinitionBase>(ref ((DmLoggingDefinitions) logger.Definitions).LogFoundSequence, (Func<EventDefinitionBase>) (() => (EventDefinitionBase) new FallbackEventDefinition(logger.Options, DmEventId.SequenceFound, LogLevel.Debug, "DmEventId.SequenceFound", DmResources._resourceManager.GetString(nameof (LogFoundSequence))))));
    }
  }
}
