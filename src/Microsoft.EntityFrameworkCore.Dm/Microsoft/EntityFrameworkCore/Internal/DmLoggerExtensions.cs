// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Internal.DmLoggerExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System;



namespace Microsoft.EntityFrameworkCore.Internal
{
  public static class DmLoggerExtensions
  {
    public static void DecimalTypeDefaultWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics,
      [NotNull] IProperty property)
    {
      EventDefinition<string, string> eventDefinition = DmResources.LogDefaultDecimalTypeColumn((IDiagnosticsLogger) diagnostics);
      if (((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        eventDefinition.Log<DbLoggerCategory.Model.Validation>(diagnostics, ((IReadOnlyPropertyBase) property).Name, ((IReadOnlyTypeBase) property.DeclaringEntityType).DisplayName());
      bool flag1;
      bool flag2;
      if (!((IDiagnosticsLogger) diagnostics).NeedsEventData((EventDefinitionBase) eventDefinition,  out flag1,  out flag2))
        return;
      PropertyEventData propertyEventData = new PropertyEventData((EventDefinitionBase) eventDefinition, new Func<EventDefinitionBase, EventData, string>(DmLoggerExtensions.DecimalTypeDefaultWarning), (IReadOnlyProperty) property);
      ((IDiagnosticsLogger) diagnostics).DispatchEventData((EventDefinitionBase) eventDefinition, (EventData) propertyEventData, flag1, flag2);
    }

    private static string DecimalTypeDefaultWarning(
      EventDefinitionBase definition,
      EventData payload)
    {
      EventDefinition<string, string> eventDefinition = (EventDefinition<string, string>) definition;
      PropertyEventData propertyEventData = (PropertyEventData) payload;
      return eventDefinition.GenerateMessage(((IReadOnlyPropertyBase) propertyEventData.Property).Name, ((IReadOnlyTypeBase) propertyEventData.Property.DeclaringEntityType).DisplayName());
    }

    public static void ByteIdentityColumnWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics,
      [NotNull] IProperty property)
    {
      EventDefinition<string, string> eventDefinition = DmResources.LogByteIdentityColumn((IDiagnosticsLogger) diagnostics);
      if (((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        eventDefinition.Log<DbLoggerCategory.Model.Validation>(diagnostics, ((IReadOnlyPropertyBase) property).Name, ((IReadOnlyTypeBase) property.DeclaringEntityType).DisplayName());
      bool flag1;
      bool flag2;
      if (!((IDiagnosticsLogger) diagnostics).NeedsEventData((EventDefinitionBase) eventDefinition, out flag1, out flag2))
        return;
      PropertyEventData propertyEventData = new PropertyEventData((EventDefinitionBase) eventDefinition, new Func<EventDefinitionBase, EventData, string>(DmLoggerExtensions.ByteIdentityColumnWarning), (IReadOnlyProperty) property);
      ((IDiagnosticsLogger) diagnostics).DispatchEventData((EventDefinitionBase) eventDefinition, (EventData) propertyEventData, flag1, flag2);
    }

    private static string ByteIdentityColumnWarning(
      EventDefinitionBase definition,
      EventData payload)
    {
      EventDefinition<string, string> eventDefinition = (EventDefinition<string, string>) definition;
      PropertyEventData propertyEventData = (PropertyEventData) payload;
      return eventDefinition.GenerateMessage(((IReadOnlyPropertyBase) propertyEventData.Property).Name, ((IReadOnlyTypeBase) propertyEventData.Property.DeclaringEntityType).DisplayName());
    }

    public static void ColumnFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string tableName,
      [NotNull] string columnName,
      int ordinal,
      [NotNull] string dataTypeName,
      int maxLength,
      int precision,
      int scale,
      bool nullable,
      bool identity,
      [CanBeNull] string defaultValue,
      [CanBeNull] string computedValue,
      bool? stored)
    {
      FallbackEventDefinition definition = DmResources.LogFoundColumn((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) definition))
        return;
      definition.Log<DbLoggerCategory.Scaffolding>(diagnostics, (Action<ILogger>) (l => l.LogDebug(((EventDefinitionBase) definition).EventId, (Exception) null, definition.MessageFormat, (object) tableName, (object) columnName, (object) ordinal, (object) dataTypeName, (object) maxLength, (object) precision, (object) scale, (object) nullable, (object) identity, (object) defaultValue, (object) computedValue, (object) stored)));
    }

    public static void ForeignKeyFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string foreignKeyName,
      [NotNull] string tableName,
      [NotNull] string principalTableName,
      [NotNull] string onDeleteAction)
    {
      EventDefinition<string, string, string, string> eventDefinition = DmResources.LogFoundForeignKey((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, foreignKeyName, tableName, principalTableName, onDeleteAction);
    }

    public static void DefaultSchemaFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string schemaName)
    {
      EventDefinition<string> eventDefinition = DmResources.LogFoundDefaultSchema((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, schemaName);
    }

    public static void PrimaryKeyFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string primaryKeyName,
      [NotNull] string tableName)
    {
      EventDefinition<string, string> eventDefinition = DmResources.LogFoundPrimaryKey((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, primaryKeyName, tableName);
    }

    public static void UniqueConstraintFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string uniqueConstraintName,
      [NotNull] string tableName)
    {
      EventDefinition<string, string> eventDefinition = DmResources.LogFoundUniqueConstraint((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, uniqueConstraintName, tableName);
    }

    public static void IndexFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string indexName,
      [NotNull] string tableName,
      bool unique)
    {
      EventDefinition<string, string, bool> eventDefinition = DmResources.LogFoundIndex((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, indexName, tableName, unique, (Exception) null);
    }

    public static void ForeignKeyReferencesMissingPrincipalTableWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [CanBeNull] string foreignKeyName,
      [CanBeNull] string tableName,
      [CanBeNull] string principalTableName)
    {
      EventDefinition<string, string, string> eventDefinition = DmResources.LogPrincipalTableNotInSelectionSet((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, foreignKeyName, tableName, principalTableName, (Exception) null);
    }

    public static void ForeignKeyPrincipalColumnMissingWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string foreignKeyName,
      [NotNull] string tableName,
      [NotNull] string principalColumnName,
      [NotNull] string principalTableName)
    {
      EventDefinition<string, string, string, string> eventDefinition = DmResources.LogPrincipalColumnNotFound((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, foreignKeyName, tableName, principalColumnName, principalTableName);
    }

    public static void MissingSchemaWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [CanBeNull] string schemaName)
    {
      EventDefinition<string> eventDefinition = DmResources.LogMissingSchema((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, schemaName);
    }

    public static void MissingTableWarning(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [CanBeNull] string tableName)
    {
      EventDefinition<string> eventDefinition = DmResources.LogMissingTable((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, tableName);
    }

    public static void SequenceFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string sequenceName,
      [NotNull] string sequenceTypeName,
      bool? cyclic,
      int? increment,
      long? start,
      long? min,
      long? max)
    {
      FallbackEventDefinition definition = DmResources.LogFoundSequence((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) definition))
        return;
      definition.Log<DbLoggerCategory.Scaffolding>(diagnostics, (Action<ILogger>) (l => l.LogDebug(((EventDefinitionBase) definition).EventId, (Exception) null, definition.MessageFormat, (object) sequenceName, (object) sequenceTypeName, (object) cyclic, (object) increment, (object) start, (object) min, (object) max)));
    }

    public static void TableFound(
      [NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics,
      [NotNull] string tableName)
    {
      EventDefinition<string> eventDefinition = DmResources.LogFoundTable((IDiagnosticsLogger) diagnostics);
      if (!((IDiagnosticsLogger) diagnostics).ShouldLog((EventDefinitionBase) eventDefinition))
        return;
      eventDefinition.Log<DbLoggerCategory.Scaffolding>(diagnostics, tableName);
    }
  }
}
