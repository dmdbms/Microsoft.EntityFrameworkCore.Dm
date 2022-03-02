using System.Linq;

namespace System.Reflection
{
	internal static class MemberInfoExtensions
	{
		public static Type GetMemberType(this MemberInfo memberInfo)
		{
			return (memberInfo as PropertyInfo)?.PropertyType ?? ((FieldInfo)memberInfo)?.FieldType;
		}

		public static bool IsSameAs(this MemberInfo propertyInfo, MemberInfo otherPropertyInfo)
		{
			if (propertyInfo == null)
			{
				return otherPropertyInfo == null;
			}
			if (otherPropertyInfo == null)
			{
				return false;
			}
			return object.Equals(propertyInfo, otherPropertyInfo) || (propertyInfo.Name == otherPropertyInfo.Name && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType || propertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(otherPropertyInfo.DeclaringType) || otherPropertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(propertyInfo.DeclaringType) || propertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains<Type>(otherPropertyInfo.DeclaringType) || otherPropertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains<Type>(propertyInfo.DeclaringType)));
		}
	}
}
