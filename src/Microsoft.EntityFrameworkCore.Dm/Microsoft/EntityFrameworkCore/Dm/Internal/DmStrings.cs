using System.Reflection;
using System.Resources;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Dm.Internal
{
	public static class DmStrings
	{
		private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Dm.Properties.DmStrings", typeof(DmStrings).GetTypeInfo().Assembly);

		public static string IndexTableRequired => GetString("IndexTableRequired");

		public static string AlterIdentityColumn => GetString("AlterIdentityColumn");

		public static string NoUserId => GetString("NoUserId");

		public static string TransientExceptionDetected => GetString("TransientExceptionDetected");

		public static string InvalidColumnNameForFreeText => GetString("InvalidColumnNameForFreeText");

		public static string IdentityBadType([CanBeNull] object property, [CanBeNull] object entityType, [CanBeNull] object propertyType)
		{
			return string.Format(GetString("IdentityBadType", "property", "entityType", "propertyType"), property, entityType, propertyType);
		}

		public static string MultipleIdentityColumns([CanBeNull] object properties, [CanBeNull] object table)
		{
			return string.Format(GetString("MultipleIdentityColumns", "properties", "table"), properties, table);
		}

		public static string NonKeyValueGeneration([CanBeNull] object property, [CanBeNull] object entityType)
		{
			return string.Format(GetString("NonKeyValueGeneration", "property", "entityType"), property, entityType);
		}

		public static string SequenceBadType([CanBeNull] object property, [CanBeNull] object entityType, [CanBeNull] object propertyType)
		{
			return string.Format(GetString("SequenceBadType", "property", "entityType", "propertyType"), property, entityType, propertyType);
		}

		public static string UnqualifiedDataType([CanBeNull] object dataType)
		{
			return string.Format(GetString("UnqualifiedDataType", "dataType"), dataType);
		}

		public static string DuplicateColumnNameValueGenerationStrategyMismatch([CanBeNull] object entityType1, [CanBeNull] object property1, [CanBeNull] object entityType2, [CanBeNull] object property2, [CanBeNull] object columnName, [CanBeNull] object table)
		{
			return string.Format(GetString("DuplicateColumnNameValueGenerationStrategyMismatch", "entityType1", "property1", "entityType2", "property2", "columnName", "table"), entityType1, property1, entityType2, property2, columnName, table);
		}

		public static string UnqualifiedDataTypeOnProperty([CanBeNull] object dataType, [CanBeNull] object property)
		{
			return string.Format(GetString("UnqualifiedDataTypeOnProperty", "dataType", "property"), dataType, property);
		}

		public static string InvalidTableToIncludeInScaffolding([CanBeNull] object table)
		{
			return string.Format(GetString("InvalidTableToIncludeInScaffolding", "table"), table);
		}

		public static string FunctionOnClient([CanBeNull] object methodName)
		{
			return string.Format(GetString("FunctionOnClient", "methodName"), methodName);
		}

		private static string GetString(string name, params string[] formatterNames)
		{
			string text = _resourceManager.GetString(name);
			for (int i = 0; i < formatterNames.Length; i++)
			{
				text = text.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
			}
			return text;
		}
	}
}
