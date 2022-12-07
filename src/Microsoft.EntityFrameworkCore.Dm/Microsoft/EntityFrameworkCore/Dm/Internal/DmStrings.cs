// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Internal.DmStrings
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using System.Reflection;
using System.Resources;

namespace Microsoft.EntityFrameworkCore.Dm.Internal
{
  public static class DmStrings
  {
    private static readonly ResourceManager _resourceManager = new ResourceManager("Microsoft.EntityFrameworkCore.Dm.Properties.DmStrings", typeof (DmStrings).GetTypeInfo().Assembly);

    public static string IdentityBadType([CanBeNull] object property, [CanBeNull] object entityType, [CanBeNull] object propertyType) => string.Format(DmStrings.GetString(nameof (IdentityBadType), nameof (property), nameof (entityType), nameof (propertyType)), property, entityType, propertyType);

    public static string IndexTableRequired => DmStrings.GetString(nameof (IndexTableRequired));

    public static string AlterIdentityColumn => DmStrings.GetString(nameof (AlterIdentityColumn));

    public static string MultipleIdentityColumns([CanBeNull] object properties, [CanBeNull] object table) => string.Format(DmStrings.GetString(nameof (MultipleIdentityColumns), nameof (properties), nameof (table)), properties, table);

    public static string NonKeyValueGeneration([CanBeNull] object property, [CanBeNull] object entityType) => string.Format(DmStrings.GetString(nameof (NonKeyValueGeneration), nameof (property), nameof (entityType)), property, entityType);

    public static string NoUserId => DmStrings.GetString(nameof (NoUserId));

    public static string SequenceBadType([CanBeNull] object property, [CanBeNull] object entityType, [CanBeNull] object propertyType) => string.Format(DmStrings.GetString(nameof (SequenceBadType), nameof (property), nameof (entityType), nameof (propertyType)), property, entityType, propertyType);

    public static string TransientExceptionDetected => DmStrings.GetString(nameof (TransientExceptionDetected));

    public static string UnqualifiedDataType([CanBeNull] object dataType) => string.Format(DmStrings.GetString(nameof (UnqualifiedDataType), nameof (dataType)), dataType);

    public static string DuplicateColumnNameValueGenerationStrategyMismatch(
      [CanBeNull] object entityType1,
      [CanBeNull] object property1,
      [CanBeNull] object entityType2,
      [CanBeNull] object property2,
      [CanBeNull] object columnName,
      [CanBeNull] object table)
    {
      return string.Format(DmStrings.GetString(nameof (DuplicateColumnNameValueGenerationStrategyMismatch), nameof (entityType1), nameof (property1), nameof (entityType2), nameof (property2), nameof (columnName), nameof (table)), entityType1, property1, entityType2, property2, columnName, table);
    }

    public static string UnqualifiedDataTypeOnProperty([CanBeNull] object dataType, [CanBeNull] object property) => string.Format(DmStrings.GetString(nameof (UnqualifiedDataTypeOnProperty), nameof (dataType), nameof (property)), dataType, property);

    public static string InvalidTableToIncludeInScaffolding([CanBeNull] object table) => string.Format(DmStrings.GetString(nameof (InvalidTableToIncludeInScaffolding), nameof (table)), table);

    public static string FunctionOnClient([CanBeNull] object methodName) => string.Format(DmStrings.GetString(nameof (FunctionOnClient), nameof (methodName)), methodName);

    public static string InvalidColumnNameForFreeText => DmStrings.GetString(nameof (InvalidColumnNameForFreeText));

    private static string GetString(string name, params string[] formatterNames)
    {
      string str = DmStrings._resourceManager.GetString(name);
      for (int index = 0; index < formatterNames.Length; ++index)
        str = str.Replace("{" + formatterNames[index] + "}", "{" + index.ToString() + "}");
      return str;
    }
  }
}
