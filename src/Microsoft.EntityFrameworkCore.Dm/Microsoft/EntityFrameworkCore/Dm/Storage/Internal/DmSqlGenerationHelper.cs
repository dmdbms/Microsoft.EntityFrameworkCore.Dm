// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmSqlGenerationHelper
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmSqlGenerationHelper : RelationalSqlGenerationHelper
  {
    public DmSqlGenerationHelper(
      [NotNull] RelationalSqlGenerationHelperDependencies dependencies)
      : base(dependencies)
    {
    }

    public virtual string BatchTerminator => Environment.NewLine + "/";

    public virtual void GenerateParameterName(StringBuilder builder, string name) => builder.Append(":").Append(name);

    public virtual string GenerateParameterName(string name) => ":" + name;

    public virtual string EscapeIdentifier(string identifier) => Check.NotEmpty(identifier, nameof (identifier)).Replace("\"", "\"\"");

    public virtual void EscapeIdentifier(StringBuilder builder, string identifier)
    {
      Check.NotEmpty(identifier, nameof (identifier));
      int length = builder.Length;
      builder.Append(identifier);
      builder.Replace("\"", "\"\"", length, identifier.Length);
    }

    public virtual string DelimitIdentifier(string identifier) => "\"" + base.EscapeIdentifier(Check.NotEmpty(identifier, nameof (identifier))) + "\"";

    public virtual void DelimitIdentifier(StringBuilder builder, string identifier)
    {
      Check.NotEmpty(identifier, nameof (identifier));
      builder.Append('"');
      base.EscapeIdentifier(builder, identifier);
      builder.Append('"');
    }
  }
}
