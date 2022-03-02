using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Internal
{
	public static class DmLoggerExtensions
	{
		public static void DecimalTypeDefaultWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
		{
			EventDefinition<string, string> eventDefinition = DmResources.LogDefaultDecimalTypeColumn(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, property.Name, property.DeclaringEntityType.DisplayName());
			}
			if (diagnostics.NeedsEventData(eventDefinition, out var diagnosticSourceEnabled, out var simpleLogEnabled))
			{
				PropertyEventData eventData = new PropertyEventData(eventDefinition, DecimalTypeDefaultWarning, property);
				diagnostics.DispatchEventData(eventDefinition, eventData, diagnosticSourceEnabled, simpleLogEnabled);
			}
		}

		private static string DecimalTypeDefaultWarning(EventDefinitionBase definition, EventData payload)
		{
			EventDefinition<string, string> eventDefinition = (EventDefinition<string, string>)definition;
			PropertyEventData propertyEventData = (PropertyEventData)payload;
			return eventDefinition.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
		}

		public static void ByteIdentityColumnWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Model.Validation> diagnostics, [JetBrains.Annotations.NotNull] IProperty property)
		{
			EventDefinition<string, string> eventDefinition = DmResources.LogByteIdentityColumn(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, property.Name, property.DeclaringEntityType.DisplayName());
			}
			if (diagnostics.NeedsEventData(eventDefinition, out var diagnosticSourceEnabled, out var simpleLogEnabled))
			{
				PropertyEventData eventData = new PropertyEventData(eventDefinition, ByteIdentityColumnWarning, property);
				diagnostics.DispatchEventData(eventDefinition, eventData, diagnosticSourceEnabled, simpleLogEnabled);
			}
		}

		private static string ByteIdentityColumnWarning(EventDefinitionBase definition, EventData payload)
		{
			EventDefinition<string, string> eventDefinition = (EventDefinition<string, string>)definition;
			PropertyEventData propertyEventData = (PropertyEventData)payload;
			return eventDefinition.GenerateMessage(propertyEventData.Property.Name, propertyEventData.Property.DeclaringEntityType.DisplayName());
		}

		public static void ColumnFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string columnName, int ordinal, [JetBrains.Annotations.NotNull] string dataTypeName, int maxLength, int precision, int scale, bool nullable, bool identity, [JetBrains.Annotations.CanBeNull] string defaultValue, [JetBrains.Annotations.CanBeNull] string computedValue, bool? stored)
		{
			FallbackEventDefinition definition = DmResources.LogFoundColumn(diagnostics);
			if (diagnostics.ShouldLog(definition))
			{
				definition.Log(diagnostics, delegate(ILogger l)
				{
					l.LogDebug(definition.EventId, null, definition.MessageFormat, tableName, columnName, ordinal, dataTypeName, maxLength, precision, scale, nullable, identity, defaultValue, computedValue, stored);
				});
			}
		}

		public static void ForeignKeyFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string foreignKeyName, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string principalTableName, [JetBrains.Annotations.NotNull] string onDeleteAction)
		{
			EventDefinition<string, string, string, string> eventDefinition = DmResources.LogFoundForeignKey(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, foreignKeyName, tableName, principalTableName, onDeleteAction);
			}
		}

		public static void DefaultSchemaFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string schemaName)
		{
			EventDefinition<string> eventDefinition = DmResources.LogFoundDefaultSchema(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, schemaName);
			}
		}

		public static void PrimaryKeyFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string primaryKeyName, [JetBrains.Annotations.NotNull] string tableName)
		{
			EventDefinition<string, string> eventDefinition = DmResources.LogFoundPrimaryKey(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, primaryKeyName, tableName);
			}
		}

		public static void UniqueConstraintFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string uniqueConstraintName, [JetBrains.Annotations.NotNull] string tableName)
		{
			EventDefinition<string, string> eventDefinition = DmResources.LogFoundUniqueConstraint(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, uniqueConstraintName, tableName);
			}
		}

		public static void IndexFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string indexName, [JetBrains.Annotations.NotNull] string tableName, bool unique)
		{
			EventDefinition<string, string, bool> eventDefinition = DmResources.LogFoundIndex(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, indexName, tableName, unique);
			}
		}

		public static void ForeignKeyReferencesMissingPrincipalTableWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.CanBeNull] string foreignKeyName, [JetBrains.Annotations.CanBeNull] string tableName, [JetBrains.Annotations.CanBeNull] string principalTableName)
		{
			EventDefinition<string, string, string> eventDefinition = DmResources.LogPrincipalTableNotInSelectionSet(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, foreignKeyName, tableName, principalTableName);
			}
		}

		public static void ForeignKeyPrincipalColumnMissingWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string foreignKeyName, [JetBrains.Annotations.NotNull] string tableName, [JetBrains.Annotations.NotNull] string principalColumnName, [JetBrains.Annotations.NotNull] string principalTableName)
		{
			EventDefinition<string, string, string, string> eventDefinition = DmResources.LogPrincipalColumnNotFound(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, foreignKeyName, tableName, principalColumnName, principalTableName);
			}
		}

		public static void MissingSchemaWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.CanBeNull] string schemaName)
		{
			EventDefinition<string> eventDefinition = DmResources.LogMissingSchema(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, schemaName);
			}
		}

		public static void MissingTableWarning([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.CanBeNull] string tableName)
		{
			EventDefinition<string> eventDefinition = DmResources.LogMissingTable(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, tableName);
			}
		}

		public static void SequenceFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string sequenceName, [JetBrains.Annotations.NotNull] string sequenceTypeName, bool? cyclic, int? increment, long? start, long? min, long? max)
		{
			FallbackEventDefinition definition = DmResources.LogFoundSequence(diagnostics);
			if (diagnostics.ShouldLog(definition))
			{
				definition.Log(diagnostics, delegate(ILogger l)
				{
					l.LogDebug(definition.EventId, null, definition.MessageFormat, sequenceName, sequenceTypeName, cyclic, increment, start, min, max);
				});
			}
		}

		public static void TableFound([JetBrains.Annotations.NotNull] this IDiagnosticsLogger<DbLoggerCategory.Scaffolding> diagnostics, [JetBrains.Annotations.NotNull] string tableName)
		{
			EventDefinition<string> eventDefinition = DmResources.LogFoundTable(diagnostics);
			if (diagnostics.ShouldLog(eventDefinition))
			{
				eventDefinition.Log(diagnostics, tableName);
			}
		}
	}
}
