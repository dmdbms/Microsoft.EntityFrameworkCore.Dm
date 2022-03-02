using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System
{
	[DebuggerStepThrough]
	internal static class SharedTypeExtensions
	{
		private static readonly Dictionary<Type, object> _commonTypeDictionary = new Dictionary<Type, object>
		{
			{
				typeof(int),
				0
			},
			{
				typeof(Guid),
				default(Guid)
			},
			{
				typeof(DateTime),
				default(DateTime)
			},
			{
				typeof(DateTimeOffset),
				default(DateTimeOffset)
			},
			{
				typeof(long),
				0L
			},
			{
				typeof(bool),
				false
			},
			{
				typeof(double),
				0.0
			},
			{
				typeof(short),
				(short)0
			},
			{
				typeof(float),
				0f
			},
			{
				typeof(byte),
				(byte)0
			},
			{
				typeof(char),
				'\0'
			},
			{
				typeof(uint),
				0u
			},
			{
				typeof(ushort),
				(ushort)0
			},
			{
				typeof(ulong),
				0uL
			},
			{
				typeof(sbyte),
				(sbyte)0
			}
		};

		public static Type UnwrapNullableType(this Type type)
		{
			return Nullable.GetUnderlyingType(type) ?? type;
		}

		public static bool IsNullableValueType(this Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static bool IsNullableType(this Type type)
		{
			return !type.IsValueType || type.IsNullableValueType();
		}

		public static bool IsValidEntityType(this Type type)
		{
			return type.GetTypeInfo().IsClass;
		}

		public static Type MakeNullable(this Type type, bool nullable = true)
		{
			return (type.IsNullableType() == nullable) ? type : (nullable ? typeof(Nullable<>).MakeGenericType(type) : type.UnwrapNullableType());
		}

		public static bool IsNumeric(this Type type)
		{
			type = type.UnwrapNullableType();
			return type.IsInteger() || type == typeof(decimal) || type == typeof(float) || type == typeof(double);
		}

		public static bool IsInteger(this Type type)
		{
			type = type.UnwrapNullableType();
			return type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(byte) || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) || type == typeof(sbyte) || type == typeof(char);
		}

		public static bool IsSignedInteger(this Type type)
		{
			return type == typeof(int) || type == typeof(long) || type == typeof(short) || type == typeof(sbyte);
		}

		public static bool IsAnonymousType(this Type type)
		{
			return type.Name.StartsWith("<>", StringComparison.Ordinal) && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Length != 0 && type.Name.Contains("AnonymousType");
		}

		public static bool IsTupleType(this Type type)
		{
			if (type == typeof(Tuple))
			{
				return true;
			}
			if (type.IsGenericType)
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(Tuple<>) || genericTypeDefinition == typeof(Tuple<, >) || genericTypeDefinition == typeof(Tuple<, , >) || genericTypeDefinition == typeof(Tuple<, , , >) || genericTypeDefinition == typeof(Tuple<, , , , >) || genericTypeDefinition == typeof(Tuple<, , , , , >) || genericTypeDefinition == typeof(Tuple<, , , , , , >) || genericTypeDefinition == typeof(Tuple<, , , , , , , >) || genericTypeDefinition == typeof(Tuple<, , , , , , , >))
				{
					return true;
				}
			}
			return false;
		}

		public static PropertyInfo GetAnyProperty(this Type type, string name)
		{
			List<PropertyInfo> list = (from p in type.GetRuntimeProperties()
				where p.Name == name
				select p).ToList();
			if (list.Count > 1)
			{
				throw new AmbiguousMatchException();
			}
			return list.SingleOrDefault();
		}

		public static bool IsInstantiable(this Type type)
		{
			return IsInstantiable(type.GetTypeInfo());
		}

		private static bool IsInstantiable(TypeInfo type)
		{
			return !type.IsAbstract && !type.IsInterface && (!type.IsGenericType || !type.IsGenericTypeDefinition);
		}

		public static Type UnwrapEnumType(this Type type)
		{
			bool flag = type.IsNullableType();
			Type type2 = (flag ? type.UnwrapNullableType() : type);
			if (!type2.GetTypeInfo().IsEnum)
			{
				return type;
			}
			Type underlyingType = Enum.GetUnderlyingType(type2);
			return flag ? underlyingType.MakeNullable() : underlyingType;
		}

		public static Type GetSequenceType(this Type type)
		{
			Type type2 = type.TryGetSequenceType();
			if (type2 == null)
			{
				throw new ArgumentException();
			}
			return type2;
		}

		public static Type TryGetSequenceType(this Type type)
		{
			return type.TryGetElementType(typeof(IEnumerable<>)) ?? type.TryGetElementType(typeof(IAsyncEnumerable<>));
		}

		public static Type TryGetElementType(this Type type, Type interfaceOrBaseType)
		{
			if (type.GetTypeInfo().IsGenericTypeDefinition)
			{
				return null;
			}
			IEnumerable<Type> genericTypeImplementations = type.GetGenericTypeImplementations(interfaceOrBaseType);
			Type type2 = null;
			foreach (Type item in genericTypeImplementations)
			{
				if (type2 == null)
				{
					type2 = item;
					continue;
				}
				type2 = null;
				break;
			}
			return type2?.GetTypeInfo().GenericTypeArguments.FirstOrDefault();
		}

		public static IEnumerable<Type> GetGenericTypeImplementations(this Type type, Type interfaceOrBaseType)
		{
			TypeInfo typeInfo = type.GetTypeInfo();
			if (typeInfo.IsGenericTypeDefinition)
			{
				yield break;
			}
			IEnumerable<Type> enumerable;
			if (!interfaceOrBaseType.GetTypeInfo().IsInterface)
			{
				enumerable = type.GetBaseTypes();
			}
			else
			{
				IEnumerable<Type> implementedInterfaces = typeInfo.ImplementedInterfaces;
				enumerable = implementedInterfaces;
			}
			IEnumerable<Type> baseTypes = enumerable;
			foreach (Type baseType in baseTypes)
			{
				if (baseType.GetTypeInfo().IsGenericType && baseType.GetGenericTypeDefinition() == interfaceOrBaseType)
				{
					yield return baseType;
				}
			}
			if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == interfaceOrBaseType)
			{
				yield return type;
			}
		}

		public static IEnumerable<Type> GetBaseTypes(this Type type)
		{
			type = type.GetTypeInfo().BaseType;
			while (type != null)
			{
				yield return type;
				type = type.GetTypeInfo().BaseType;
			}
		}

		public static IEnumerable<Type> GetTypesInHierarchy(this Type type)
		{
			while (type != null)
			{
				yield return type;
				type = type.GetTypeInfo().BaseType;
			}
		}

		public static ConstructorInfo GetDeclaredConstructor(this Type type, Type[] types)
		{
			if (types == null)
			{
				types = Array.Empty<Type>();
			}
			return type.GetTypeInfo().DeclaredConstructors.SingleOrDefault((ConstructorInfo c) => !c.IsStatic && (from p in c.GetParameters()
				select p.ParameterType).SequenceEqual(types));
		}

		public static IEnumerable<PropertyInfo> GetPropertiesInHierarchy(this Type type, string name)
		{
			do
			{
				TypeInfo typeInfo = type.GetTypeInfo();
				foreach (PropertyInfo propertyInfo in typeInfo.DeclaredProperties)
				{
					if (propertyInfo.Name.Equals(name, StringComparison.Ordinal) && !(propertyInfo.GetMethod ?? propertyInfo.SetMethod)!.IsStatic)
					{
						yield return propertyInfo;
					}
				}
				type = typeInfo.BaseType;
			}
			while (type != null);
		}

		public static IEnumerable<MemberInfo> GetMembersInHierarchy(this Type type)
		{
			do
			{
				foreach (PropertyInfo item in from pi in type.GetRuntimeProperties()
					where !(pi.GetMethod ?? pi.SetMethod)!.IsStatic
					select pi)
				{
					yield return item;
				}
				foreach (FieldInfo item2 in from f in type.GetRuntimeFields()
					where !f.IsStatic
					select f)
				{
					yield return item2;
				}
				type = type.BaseType;
			}
			while (type != null);
		}

		public static IEnumerable<MemberInfo> GetMembersInHierarchy(this Type type, string name)
		{
			return from m in type.GetMembersInHierarchy()
				where m.Name == name
				select m;
		}

		public static object GetDefaultValue(this Type type)
		{
			if (!type.GetTypeInfo().IsValueType)
			{
				return null;
			}
			object value;
			return _commonTypeDictionary.TryGetValue(type, out value) ? value : Activator.CreateInstance(type);
		}

		public static IEnumerable<TypeInfo> GetConstructibleTypes(this Assembly assembly)
		{
			return from t in assembly.GetLoadableDefinedTypes()
				where !t.IsAbstract && !t.IsGenericTypeDefinition
				select t;
		}

		public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly)
		{
			try
			{
				return assembly.DefinedTypes;
			}
			catch (ReflectionTypeLoadException ex)
			{
				return ex.Types.Where((Type t) => t != null).Select(new Func<Type, TypeInfo>(IntrospectionExtensions.GetTypeInfo));
			}
		}
	}
}
