using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace System.Reflection
{
	[DebuggerStepThrough]
	internal static class PropertyInfoExtensions
	{
		public static bool IsStatic(this PropertyInfo property)
		{
			return (property.GetMethod ?? property.SetMethod)!.IsStatic;
		}

		public static bool IsCandidateProperty(this PropertyInfo propertyInfo, bool needsWrite = true, bool publicOnly = true)
		{
			return !propertyInfo.IsStatic() && propertyInfo.CanRead && (!needsWrite || propertyInfo.FindSetterProperty() != null) && propertyInfo.GetMethod != null && (!publicOnly || propertyInfo.GetMethod!.IsPublic) && propertyInfo.GetIndexParameters().Length == 0;
		}

		public static PropertyInfo FindGetterProperty([JetBrains.Annotations.NotNull] this PropertyInfo propertyInfo)
		{
			return propertyInfo.DeclaringType.GetPropertiesInHierarchy(propertyInfo.Name).FirstOrDefault((PropertyInfo p) => p.GetMethod != null);
		}

		public static PropertyInfo FindSetterProperty([JetBrains.Annotations.NotNull] this PropertyInfo propertyInfo)
		{
			return propertyInfo.DeclaringType.GetPropertiesInHierarchy(propertyInfo.Name).FirstOrDefault((PropertyInfo p) => p.SetMethod != null);
		}
	}
}
