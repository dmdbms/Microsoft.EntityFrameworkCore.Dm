// Decompiled with JetBrains decompiler
// Type: System.Reflection.PropertyInfoExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using System.Diagnostics;
using System.Linq;



namespace System.Reflection
{
  [DebuggerStepThrough]
  internal static class PropertyInfoExtensions
  {
    public static bool IsStatic(this PropertyInfo property)
    {
      MethodInfo methodInfo = property.GetMethod;
      if ((object) methodInfo == null)
        methodInfo = property.SetMethod;
      return methodInfo.IsStatic;
    }

    public static bool IsCandidateProperty(
      this PropertyInfo propertyInfo,
      bool needsWrite = true,
      bool publicOnly = true)
    {
      return !propertyInfo.IsStatic() && propertyInfo.CanRead && (!needsWrite || propertyInfo.FindSetterProperty() != (PropertyInfo) null) && propertyInfo.GetMethod != (MethodInfo) null && (!publicOnly || propertyInfo.GetMethod.IsPublic) && propertyInfo.GetIndexParameters().Length == 0;
    }

    public static PropertyInfo FindGetterProperty([NotNull] this PropertyInfo propertyInfo) => propertyInfo.DeclaringType.GetPropertiesInHierarchy(propertyInfo.Name).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.GetMethod != (MethodInfo) null));

    public static PropertyInfo FindSetterProperty([NotNull] this PropertyInfo propertyInfo) => propertyInfo.DeclaringType.GetPropertiesInHierarchy(propertyInfo.Name).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.SetMethod != (MethodInfo) null));
  }
}
