// Decompiled with JetBrains decompiler
// Type: System.Reflection.MemberInfoExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using System.Linq;



namespace System.Reflection
{
  internal static class MemberInfoExtensions
  {
    public static Type GetMemberType(this MemberInfo memberInfo)
    {
      Type propertyType = (memberInfo as PropertyInfo)?.PropertyType;
      if ((object) propertyType != null)
        return propertyType;
      return ((FieldInfo) memberInfo)?.FieldType;
    }

    public static bool IsSameAs(this MemberInfo propertyInfo, MemberInfo otherPropertyInfo)
    {
      if (propertyInfo == (MemberInfo) null)
        return otherPropertyInfo == (MemberInfo) null;
      return !(otherPropertyInfo == (MemberInfo) null) && (object.Equals((object) propertyInfo, (object) otherPropertyInfo) || propertyInfo.Name == otherPropertyInfo.Name && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType || propertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(otherPropertyInfo.DeclaringType) || otherPropertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(propertyInfo.DeclaringType) || propertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains<Type>(otherPropertyInfo.DeclaringType) || otherPropertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains<Type>(propertyInfo.DeclaringType)));
    }
  }
}
