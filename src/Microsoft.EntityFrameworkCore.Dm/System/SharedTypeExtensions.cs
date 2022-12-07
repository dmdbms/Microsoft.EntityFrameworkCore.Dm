// Decompiled with JetBrains decompiler
// Type: System.SharedTypeExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

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
    private static readonly Dictionary<Type, object> _commonTypeDictionary = new Dictionary<Type, object>()
    {
      {
        typeof (int),
        (object) 0
      },
      {
        typeof (Guid),
        (object) new Guid()
      },
      {
        typeof (DateTime),
        (object) new DateTime()
      },
      {
        typeof (DateTimeOffset),
        (object) new DateTimeOffset()
      },
      {
        typeof (long),
        (object) 0L
      },
      {
        typeof (bool),
        (object) false
      },
      {
        typeof (double),
        (object) 0.0
      },
      {
        typeof (short),
        (object) (short) 0
      },
      {
        typeof (float),
        (object) 0.0f
      },
      {
        typeof (byte),
        (object) (byte) 0
      },
      {
        typeof (char),
        (object) char.MinValue
      },
      {
        typeof (uint),
        (object) 0U
      },
      {
        typeof (ushort),
        (object) (ushort) 0
      },
      {
        typeof (ulong),
        (object) 0UL
      },
      {
        typeof (sbyte),
        (object) (sbyte) 0
      }
    };

    public static Type UnwrapNullableType(this Type type)
    {
      Type underlyingType = Nullable.GetUnderlyingType(type);
      return (object) underlyingType != null ? underlyingType : type;
    }

    public static bool IsNullableValueType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);

    public static bool IsNullableType(this Type type) => !type.IsValueType || type.IsNullableValueType();

    public static bool IsValidEntityType(this Type type) => type.GetTypeInfo().IsClass;

    public static Type MakeNullable(this Type type, bool nullable = true)
    {
      if (type.IsNullableType() == nullable)
        return type;
      if (!nullable)
        return type.UnwrapNullableType();
      return typeof (Nullable<>).MakeGenericType(type);
    }

    public static bool IsNumeric(this Type type)
    {
      type = type.UnwrapNullableType();
      return type.IsInteger() || type == typeof (Decimal) || type == typeof (float) || type == typeof (double);
    }

    public static bool IsInteger(this Type type)
    {
      type = type.UnwrapNullableType();
      return type == typeof (int) || type == typeof (long) || type == typeof (short) || type == typeof (byte) || type == typeof (uint) || type == typeof (ulong) || type == typeof (ushort) || type == typeof (sbyte) || type == typeof (char);
    }

    public static bool IsSignedInteger(this Type type) => type == typeof (int) || type == typeof (long) || type == typeof (short) || type == typeof (sbyte);

    public static bool IsAnonymousType(this Type type) => type.Name.StartsWith("<>", StringComparison.Ordinal) && type.GetCustomAttributes(typeof (CompilerGeneratedAttribute), false).Length != 0 && type.Name.Contains("AnonymousType");

    public static bool IsTupleType(this Type type)
    {
      if (type == typeof (Tuple))
        return true;
      if (type.IsGenericType)
      {
        Type genericTypeDefinition = type.GetGenericTypeDefinition();
        if (genericTypeDefinition == typeof (Tuple<>) || genericTypeDefinition == typeof (Tuple<,>) || genericTypeDefinition == typeof (Tuple<,,>) || genericTypeDefinition == typeof (Tuple<,,,>) || genericTypeDefinition == typeof (Tuple<,,,,>) || genericTypeDefinition == typeof (Tuple<,,,,,>) || genericTypeDefinition == typeof (Tuple<,,,,,,>) || genericTypeDefinition == typeof (Tuple<,,,,,,,>) || genericTypeDefinition == typeof (Tuple<,,,,,,,>))
          return true;
      }
      return false;
    }

    public static PropertyInfo GetAnyProperty(this Type type, string name)
    {
      List<PropertyInfo> list = type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.Name == name)).ToList<PropertyInfo>();
      return list.Count <= 1 ? list.SingleOrDefault<PropertyInfo>() : throw new AmbiguousMatchException();
    }

    public static bool IsInstantiable(this Type type) => SharedTypeExtensions.IsInstantiable(type.GetTypeInfo());

    private static bool IsInstantiable(TypeInfo type)
    {
      if (type.IsAbstract || type.IsInterface)
        return false;
      return !type.IsGenericType || !type.IsGenericTypeDefinition;
    }

    public static Type UnwrapEnumType(this Type type)
    {
      bool flag = type.IsNullableType();
      Type type1 = flag ? type.UnwrapNullableType() : type;
      if (!type1.GetTypeInfo().IsEnum)
        return type;
      Type underlyingType = Enum.GetUnderlyingType(type1);
      return flag ? underlyingType.MakeNullable() : underlyingType;
    }

    public static Type GetSequenceType(this Type type)
    {
      Type sequenceType = type.TryGetSequenceType();
      return !(sequenceType == (Type) null) ? sequenceType : throw new ArgumentException();
    }

    public static Type TryGetSequenceType(this Type type)
    {
      Type elementType = type.TryGetElementType(typeof (IEnumerable<>));
      return (object) elementType != null ? elementType : type.TryGetElementType(typeof (IAsyncEnumerable<>));
    }

    public static Type TryGetElementType(this Type type, Type interfaceOrBaseType)
    {
      if (type.GetTypeInfo().IsGenericTypeDefinition)
        return (Type) null;
      IEnumerable<Type> typeImplementations = type.GetGenericTypeImplementations(interfaceOrBaseType);
      Type type1 = (Type) null;
      foreach (Type type2 in typeImplementations)
      {
        if (type1 == (Type) null)
        {
          type1 = type2;
        }
        else
        {
          type1 = (Type) null;
          break;
        }
      }
      return (object) type1 != null ? ((IEnumerable<Type>) type1.GetTypeInfo().GenericTypeArguments).FirstOrDefault<Type>() : (Type) null;
    }

    public static IEnumerable<Type> GetGenericTypeImplementations(
      this Type type,
      Type interfaceOrBaseType)
    {
      TypeInfo typeInfo = type.GetTypeInfo();
      if (!typeInfo.IsGenericTypeDefinition)
      {
        IEnumerable<Type> baseTypes = interfaceOrBaseType.GetTypeInfo().IsInterface ? typeInfo.ImplementedInterfaces : type.GetBaseTypes();
        foreach (Type baseType in baseTypes)
        {
          if (baseType.GetTypeInfo().IsGenericType && baseType.GetGenericTypeDefinition() == interfaceOrBaseType)
            yield return baseType;
        }
        if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == interfaceOrBaseType)
          yield return type;
        baseTypes = (IEnumerable<Type>) null;
      }
    }

    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
      for (type = type.GetTypeInfo().BaseType; type != (Type) null; type = type.GetTypeInfo().BaseType)
        yield return type;
    }

    public static IEnumerable<Type> GetTypesInHierarchy(this Type type)
    {
      for (; type != (Type) null; type = type.GetTypeInfo().BaseType)
        yield return type;
    }

    public static ConstructorInfo GetDeclaredConstructor(
      this Type type,
      Type[] types)
    {
      if (types == null)
        types = Array.Empty<Type>();
      return type.GetTypeInfo().DeclaredConstructors.SingleOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => !c.IsStatic && ((IEnumerable<ParameterInfo>) c.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).SequenceEqual<Type>((IEnumerable<Type>) types)));
    }

    public static IEnumerable<PropertyInfo> GetPropertiesInHierarchy(
      this Type type,
      string name)
    {
label_3:
      TypeInfo typeInfo = type.GetTypeInfo();
      foreach (PropertyInfo propertyInfo in typeInfo.DeclaredProperties)
      {
        int num;
        if (propertyInfo.Name.Equals(name, StringComparison.Ordinal))
        {
          MethodInfo methodInfo = propertyInfo.GetMethod;
          if ((object) methodInfo == null)
            methodInfo = propertyInfo.SetMethod;
          num = !methodInfo.IsStatic ? 1 : 0;
        }
        else
          num = 0;
        if (num != 0)
          yield return propertyInfo;
      }
      type = typeInfo.BaseType;
      typeInfo = (TypeInfo) null;
      if (type != (Type) null)
        goto label_3;
    }

    public static IEnumerable<MemberInfo> GetMembersInHierarchy(this Type type)
    {
label_3:
      foreach (PropertyInfo propertyInfo in type.GetRuntimeProperties().Where<PropertyInfo>((Func<PropertyInfo, bool>) (pi =>
      {
        MethodInfo methodInfo = pi.GetMethod;
        if ((object) methodInfo == null)
          methodInfo = pi.SetMethod;
        return !methodInfo.IsStatic;
      })))
        yield return (MemberInfo) propertyInfo;
      foreach (FieldInfo fieldInfo in type.GetRuntimeFields().Where<FieldInfo>((Func<FieldInfo, bool>) (f => !f.IsStatic)))
        yield return (MemberInfo) fieldInfo;
      type = type.BaseType;
      if (type != (Type) null)
        goto label_3;
    }

    public static IEnumerable<MemberInfo> GetMembersInHierarchy(
      this Type type,
      string name)
    {
      return type.GetMembersInHierarchy().Where<MemberInfo>((Func<MemberInfo, bool>) (m => m.Name == name));
    }

    public static object GetDefaultValue(this Type type)
    {
      object obj;
      return !type.GetTypeInfo().IsValueType ? (object) null : (SharedTypeExtensions._commonTypeDictionary.TryGetValue(type, out obj) ? obj : Activator.CreateInstance(type));
    }

    public static IEnumerable<TypeInfo> GetConstructibleTypes(
      this Assembly assembly)
    {
      return assembly.GetLoadableDefinedTypes().Where<TypeInfo>((Func<TypeInfo, bool>) (t => !t.IsAbstract && !t.IsGenericTypeDefinition));
    }

    public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(
      this Assembly assembly)
    {
      try
      {
        return assembly.DefinedTypes;
      }
      catch (ReflectionTypeLoadException ex)
      {
        return ((IEnumerable<Type>) ex.Types).Where<Type>((Func<Type, bool>) (t => t != (Type) null)).Select<Type, TypeInfo>(new Func<Type, TypeInfo>(IntrospectionExtensions.GetTypeInfo));
      }
    }
  }
}
