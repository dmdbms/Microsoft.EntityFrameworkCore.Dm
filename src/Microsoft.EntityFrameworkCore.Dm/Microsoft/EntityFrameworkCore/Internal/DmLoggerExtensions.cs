using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace Microsoft.EntityFrameworkCore.Internal
{
public static class DmLoggerExtensions
{
    public static void DecimalTypeDefaultWarning([NotNull] this IDiagnosticsLogger<Validation> diagnostics, [NotNull] IProperty property)
    {
        //IL_004e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0055: Expected O, but got Unknown
        EventDefinition<string, string> val = DmResources.LogDefaultDecimalTypeColumn((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<Validation>(diagnostics, ((IReadOnlyPropertyBase)property).Name, ((IReadOnlyTypeBase)property.DeclaringEntityType).DisplayName());
        }
        bool flag = default(bool);
        bool flag2 = default(bool);
        if (((IDiagnosticsLogger)diagnostics).NeedsEventData((EventDefinitionBase)(object)val, out flag, out flag2))
        {
            PropertyEventData val2 = new PropertyEventData((EventDefinitionBase)(object)val, (Func<EventDefinitionBase, EventData, string>)DecimalTypeDefaultWarning, (IReadOnlyProperty)(object)property);
            ((IDiagnosticsLogger)diagnostics).DispatchEventData((EventDefinitionBase)(object)val, (EventData)(object)val2, flag, flag2);
        }
    }

    private static string DecimalTypeDefaultWarning(EventDefinitionBase definition, EventData payload)
    {
        //IL_0009: Unknown result type (might be due to invalid IL or missing references)
        //IL_000f: Expected O, but got Unknown
        EventDefinition<string, string> val = (EventDefinition<string, string>)(object)definition;
        PropertyEventData val2 = (PropertyEventData)payload;
        return val.GenerateMessage(((IReadOnlyPropertyBase)val2.Property).Name, ((IReadOnlyTypeBase)val2.Property.DeclaringEntityType).DisplayName());
    }

    public static void ByteIdentityColumnWarning([NotNull] this IDiagnosticsLogger<Validation> diagnostics, [NotNull] IProperty property)
    {
        //IL_004e: Unknown result type (might be due to invalid IL or missing references)
        //IL_0055: Expected O, but got Unknown
        EventDefinition<string, string> val = DmResources.LogByteIdentityColumn((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<Validation>(diagnostics, ((IReadOnlyPropertyBase)property).Name, ((IReadOnlyTypeBase)property.DeclaringEntityType).DisplayName());
        }
        bool flag = default(bool);
        bool flag2 = default(bool);
        if (((IDiagnosticsLogger)diagnostics).NeedsEventData((EventDefinitionBase)(object)val, out flag, out flag2))
        {
            PropertyEventData val2 = new PropertyEventData((EventDefinitionBase)(object)val, (Func<EventDefinitionBase, EventData, string>)ByteIdentityColumnWarning, (IReadOnlyProperty)(object)property);
            ((IDiagnosticsLogger)diagnostics).DispatchEventData((EventDefinitionBase)(object)val, (EventData)(object)val2, flag, flag2);
        }
    }

    private static string ByteIdentityColumnWarning(EventDefinitionBase definition, EventData payload)
    {
        //IL_0009: Unknown result type (might be due to invalid IL or missing references)
        //IL_000f: Expected O, but got Unknown
        EventDefinition<string, string> val = (EventDefinition<string, string>)(object)definition;
        PropertyEventData val2 = (PropertyEventData)payload;
        return val.GenerateMessage(val2.Property.Name, ((IReadOnlyTypeBase)val2.Property.DeclaringEntityType).DisplayName());
    }

    public static void ColumnFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string tableName, [NotNull] string columnName, int ordinal, [NotNull] string dataTypeName, int maxLength, int precision, int scale, bool nullable, bool identity, [CanBeNull] string defaultValue, [CanBeNull] string computedValue, bool? stored)
    {
        string tableName2 = tableName;
        string columnName2 = columnName;
        string dataTypeName2 = dataTypeName;
        string defaultValue2 = defaultValue;
        string computedValue2 = computedValue;
        FallbackEventDefinition definition = DmResources.LogFoundColumn((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)definition))
        {
            definition.Log<DbLoggerCategory.Scaffolding>(diagnostics, (Action<ILogger>)delegate(ILogger l)
            {
                l.LogDebug(((EventDefinitionBase)definition).EventId, null, definition.MessageFormat, tableName2, columnName2, ordinal, dataTypeName2, maxLength, precision, scale, nullable, identity, defaultValue2, computedValue2, stored);
            });
        }
    }

    public static void ForeignKeyFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string foreignKeyName, [NotNull] string tableName, [NotNull] string principalTableName, [NotNull] string onDeleteAction)
    {
        EventDefinition<string, string, string, string> val = DmResources.LogFoundForeignKey((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, foreignKeyName, tableName, principalTableName, onDeleteAction);
        }
    }

    public static void DefaultSchemaFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string schemaName)
    {
        EventDefinition<string> val = DmResources.LogFoundDefaultSchema((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, schemaName);
        }
    }

    public static void PrimaryKeyFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string primaryKeyName, [NotNull] string tableName)
    {
        EventDefinition<string, string> val = DmResources.LogFoundPrimaryKey((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, primaryKeyName, tableName);
        }
    }

    public static void UniqueConstraintFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string uniqueConstraintName, [NotNull] string tableName)
    {
        EventDefinition<string, string> val = DmResources.LogFoundUniqueConstraint((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, uniqueConstraintName, tableName);
        }
    }

    public static void IndexFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string indexName, [NotNull] string tableName, bool unique)
    {
        EventDefinition<string, string, bool> val = DmResources.LogFoundIndex((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, indexName, tableName, unique, (Exception)null);
        }
    }

    public static void ForeignKeyReferencesMissingPrincipalTableWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string foreignKeyName, [CanBeNull] string tableName, [CanBeNull] string principalTableName)
    {
        EventDefinition<string, string, string> val = DmResources.LogPrincipalTableNotInSelectionSet((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, foreignKeyName, tableName, principalTableName, (Exception)null);
        }
    }

    public static void ForeignKeyPrincipalColumnMissingWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string foreignKeyName, [NotNull] string tableName, [NotNull] string principalColumnName, [NotNull] string principalTableName)
    {
        EventDefinition<string, string, string, string> val = DmResources.LogPrincipalColumnNotFound((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, foreignKeyName, tableName, principalColumnName, principalTableName);
        }
    }

    public static void MissingSchemaWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string schemaName)
    {
        EventDefinition<string> val = DmResources.LogMissingSchema((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, schemaName);
        }
    }

    public static void MissingTableWarning([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [CanBeNull] string tableName)
    {
        EventDefinition<string> val = DmResources.LogMissingTable((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, tableName);
        }
    }

    public static void SequenceFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string sequenceName, [NotNull] string sequenceTypeName, bool? cyclic, int? increment, long? start, long? min, long? max)
    {
        string sequenceName2 = sequenceName;
        string sequenceTypeName2 = sequenceTypeName;
        FallbackEventDefinition definition = DmResources.LogFoundSequence((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)definition))
        {
            definition.Log<DbLoggerCategory.Scaffolding>(diagnostics, (Action<ILogger>)delegate(ILogger l)
            {
                l.LogDebug(((EventDefinitionBase)definition).EventId, null, definition.MessageFormat, sequenceName2, sequenceTypeName2, cyclic, increment, start, min, max);
            });
        }
    }

    public static void TableFound([NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [NotNull] string tableName)
    {
        EventDefinition<string> val = DmResources.LogFoundTable((IDiagnosticsLogger)(object)diagnostics);
        if (((IDiagnosticsLogger)diagnostics).ShouldLog((EventDefinitionBase)(object)val))
        {
            val.Log<DbLoggerCategory.Scaffolding>(diagnostics, tableName);
        }
    }
}
}