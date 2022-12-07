// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Utilities.SharedTypeExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Utilities
{
  [DebuggerStepThrough]
  internal static class SharedTypeExtensions
  {
    private static readonly Dictionary<Type, string> _builtInTypeNames = new Dictionary<Type, string>()
    {
      {
        typeof (bool),
        "bool"
      },
      {
        typeof (byte),
        "byte"
      },
      {
        typeof (char),
        "char"
      },
      {
        typeof (Decimal),
        "decimal"
      },
      {
        typeof (double),
        "double"
      },
      {
        typeof (float),
        "float"
      },
      {
        typeof (int),
        "int"
      },
      {
        typeof (long),
        "long"
      },
      {
        typeof (object),
        "object"
      },
      {
        typeof (sbyte),
        "sbyte"
      },
      {
        typeof (short),
        "short"
      },
      {
        typeof (string),
        "string"
      },
      {
        typeof (uint),
        "uint"
      },
      {
        typeof (ulong),
        "ulong"
      },
      {
        typeof (ushort),
        "ushort"
      },
      {
        typeof (void),
        "void"
      }
    };
    private static readonly 
    #nullable disable
    Dictionary<Type, object> _commonTypeDictionary = new Dictionary<Type, object>()
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
    private static readonly MethodInfo _generateDefaultValueConstantMethod = typeof (SharedTypeExtensions).GetTypeInfo().GetDeclaredMethod("GenerateDefaultValueConstant");

    public static 
    
    Type UnwrapNullableType(this Type type)
    {
      Type underlyingType = Nullable.GetUnderlyingType(type);
      return (object) underlyingType != null ? underlyingType : type;
    }

    public static bool IsNullableValueType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);

    public static bool IsNullableType(this Type type) => !type.IsValueType || type.IsNullableValueType();

    public static bool IsValidEntityType(this Type type) => type.IsClass;

    public static bool IsPropertyBagType(this Type type) => !type.IsGenericTypeDefinition && type.GetGenericTypeImplementations(typeof (IDictionary<,>)).Any<Type>((Func<Type, bool>) (t => t.GetGenericArguments()[0] == typeof (string) && t.GetGenericArguments()[1] == typeof (object)));

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

    public static MethodInfo GetRequiredMethod(
      this Type type,
      string name,
      params Type[] parameters)
    {
      MethodInfo method = type.GetTypeInfo().GetMethod(name, parameters);
      if (method == (MethodInfo) null && parameters.Length == 0)
        method = type.GetMethod(name);
      return !(method == (MethodInfo) null) ? method : throw new InvalidOperationException();
    }

    public static PropertyInfo GetRequiredProperty(this Type type, string name)
    {
      PropertyInfo property = type.GetTypeInfo().GetProperty(name);
      return !(property == (PropertyInfo) null) ? property : throw new InvalidOperationException();
    }

    public static FieldInfo GetRequiredDeclaredField(this Type type, string name)
    {
      FieldInfo declaredField = type.GetTypeInfo().GetDeclaredField(name);
      return !(declaredField == (FieldInfo) null) ? declaredField : throw new InvalidOperationException();
    }

    public static MethodInfo GetRequiredDeclaredMethod(this Type type, string name)
    {
      MethodInfo declaredMethod = type.GetTypeInfo().GetDeclaredMethod(name);
      return !(declaredMethod == (MethodInfo) null) ? declaredMethod : throw new InvalidOperationException();
    }

    public static PropertyInfo GetRequiredDeclaredProperty(this Type type, string name)
    {
      PropertyInfo declaredProperty = type.GetTypeInfo().GetDeclaredProperty(name);
      return !(declaredProperty == (PropertyInfo) null) ? declaredProperty : throw new InvalidOperationException();
    }

    public static MethodInfo GetRequiredRuntimeMethod(
      this Type type,
      string name,
      params Type[] parameters)
    {
      MethodInfo runtimeMethod = type.GetTypeInfo().GetRuntimeMethod(name, parameters);
      return !(runtimeMethod == (MethodInfo) null) ? runtimeMethod : throw new InvalidOperationException();
    }

    public static PropertyInfo GetRequiredRuntimeProperty(this Type type, string name)
    {
      PropertyInfo runtimeProperty = type.GetTypeInfo().GetRuntimeProperty(name);
      return !(runtimeProperty == (PropertyInfo) null) ? runtimeProperty : throw new InvalidOperationException();
    }

    public static bool IsInstantiable(this Type type)
    {
      if (type.IsAbstract || type.IsInterface)
        return false;
      return !type.IsGenericType || !type.IsGenericTypeDefinition;
    }

    public static Type UnwrapEnumType(this Type type)
    {
      bool flag = type.IsNullableType();
      Type enumType = flag ? type.UnwrapNullableType() : type;
      if (!enumType.IsEnum)
        return type;
      Type underlyingType = Enum.GetUnderlyingType(enumType);
      return flag ? underlyingType.MakeNullable() : underlyingType;
    }

    public static Type GetSequenceType(this Type type)
    {
      Type sequenceType = type.TryGetSequenceType();
      return !(sequenceType == (Type) null) ? sequenceType : throw new ArgumentException();
    }

    public static Type? TryGetSequenceType(this Type type)
    {
      Type elementType = type.TryGetElementType(typeof (IEnumerable<>));
      return (object) elementType != null ? elementType : type.TryGetElementType(typeof (IAsyncEnumerable<>));
    }

    public static Type? TryGetElementType(this Type type, Type interfaceOrBaseType)
    {
      if (type.IsGenericTypeDefinition)
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
      return (object) type1 != null ? ((IEnumerable<Type>) type1.GenericTypeArguments).FirstOrDefault<Type>() : (Type) null;
    }

    public static bool IsCompatibleWith(this 
    #nullable disable
    Type propertyType, Type fieldType)
    {
      if (propertyType.IsAssignableFrom(fieldType) || fieldType.IsAssignableFrom(propertyType))
        return true;
      Type sequenceType1 = propertyType.TryGetSequenceType();
      Type sequenceType2 = fieldType.TryGetSequenceType();
      return sequenceType1 != (Type) null && sequenceType2 != (Type) null && sequenceType1.IsCompatibleWith(sequenceType2);
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
          if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == interfaceOrBaseType)
            yield return baseType;
        }
        if (type.IsGenericType && type.GetGenericTypeDefinition() == interfaceOrBaseType)
          yield return type;
        baseTypes = (IEnumerable<Type>) null;
      }
    }

    public static IEnumerable<Type> GetBaseTypes(this Type type)
    {
      for (type = type.BaseType; type != (Type) null; type = type.BaseType)
        yield return type;
    }

    public static IEnumerable<Type> GetTypesInHierarchy(this Type type)
    {
      for (; type != (Type) null; type = type.BaseType)
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
      return !type.IsValueType ? (object) null : (SharedTypeExtensions._commonTypeDictionary.TryGetValue(type, out obj) ? obj : Activator.CreateInstance(type));
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

    public static bool IsQueryableType(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof (IQueryable<>) || ((IEnumerable<Type>) type.GetInterfaces()).Any<Type>((Func<Type, bool>) (i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IQueryable<>)));

    public static string DisplayName([NotNull] this Type type, bool fullName = true)
    {
      StringBuilder builder = new StringBuilder();
      SharedTypeExtensions.ProcessType(builder, type, fullName);
      return builder.ToString();
    }

    private static void ProcessType(StringBuilder builder, Type type, bool fullName)
    {
      if (type.IsGenericType)
      {
        Type[] genericArguments = type.GetGenericArguments();
        SharedTypeExtensions.ProcessGenericType(builder, type, genericArguments, genericArguments.Length, fullName);
      }
      else if (type.IsArray)
      {
        SharedTypeExtensions.ProcessArrayType(builder, type, fullName);
      }
      else
      {
        string str;
        if (SharedTypeExtensions._builtInTypeNames.TryGetValue(type, out str))
        {
          builder.Append(str);
        }
        else
        {
          if (type.IsGenericParameter)
            return;
          builder.Append(fullName ? type.FullName : type.Name);
        }
      }
    }

    private static void ProcessArrayType(StringBuilder builder, Type type, bool fullName)
    {
      Type type1 = type;
      while (type1.IsArray)
        type1 = type1.GetElementType();
      SharedTypeExtensions.ProcessType(builder, type1, fullName);
      for (; type.IsArray; type = type.GetElementType())
      {
        builder.Append('[');
        builder.Append(',', type.GetArrayRank() - 1);
        builder.Append(']');
      }
    }

    private static void ProcessGenericType(
      StringBuilder builder,
      Type type,
      Type[] genericArguments,
      int length,
      bool fullName)
    {
      int length1 = type.IsNested ? type.DeclaringType.GetGenericArguments().Length : 0;
      if (fullName)
      {
        if (type.IsNested)
        {
          SharedTypeExtensions.ProcessGenericType(builder, type.DeclaringType, genericArguments, length1, fullName);
          builder.Append('+');
        }
        else
        {
          builder.Append(type.Namespace);
          builder.Append('.');
        }
      }
      int count = type.Name.IndexOf('`');
      if (count <= 0)
      {
        builder.Append(type.Name);
      }
      else
      {
        builder.Append(type.Name, 0, count);
        builder.Append('<');
        for (int index = length1; index < length; ++index)
        {
          SharedTypeExtensions.ProcessType(builder, genericArguments[index], fullName);
          if (index + 1 != length)
          {
            builder.Append(',');
            if (!genericArguments[index + 1].IsGenericParameter)
              builder.Append(' ');
          }
        }
        builder.Append('>');
      }
    }

    public static IEnumerable<string> GetNamespaces([NotNull] this Type type)
    {
      if (!SharedTypeExtensions._builtInTypeNames.ContainsKey(type))
      {
        yield return type.Namespace;
        if (type.IsGenericType)
        {
          Type[] typeArray = type.GenericTypeArguments;
          for (int index = 0; index < typeArray.Length; ++index)
          {
            Type typeArgument = typeArray[index];
            foreach (string ns in typeArgument.GetNamespaces())
              yield return ns;
            typeArgument = (Type) null;
          }
          typeArray = (Type[]) null;
        }
      }
    }

    public static ConstantExpression GetDefaultValueConstant(this Type type) => (ConstantExpression) SharedTypeExtensions._generateDefaultValueConstantMethod.MakeGenericMethod(type).Invoke((object) null, Array.Empty<object>());

    private static ConstantExpression GenerateDefaultValueConstant<TDefault>() => Expression.Constant((object) default (TDefault), typeof (TDefault));
  }
}
