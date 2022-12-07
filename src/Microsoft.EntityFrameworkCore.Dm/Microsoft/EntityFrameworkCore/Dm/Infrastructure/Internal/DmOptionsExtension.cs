// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal.DmOptionsExtension
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;



namespace Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal
{
  public class DmOptionsExtension : RelationalOptionsExtension
  {
    private DbContextOptionsExtensionInfo _info;

    public DmOptionsExtension()
    {
    }

    protected DmOptionsExtension([NotNull] DmOptionsExtension copyFrom)
      : base((RelationalOptionsExtension) copyFrom)
    {
    }

    public override DbContextOptionsExtensionInfo Info => this._info ?? (this._info = (DbContextOptionsExtensionInfo) new DmOptionsExtension.ExtensionInfo((IDbContextOptionsExtension) this));

    protected override RelationalOptionsExtension Clone() => (RelationalOptionsExtension) new DmOptionsExtension(this);

    public override void ApplyServices(IServiceCollection services)
    {
      Check.NotNull<IServiceCollection>(services, nameof (services));
      services.AddEntityFrameworkDm();
    }

    private sealed class ExtensionInfo : RelationalOptionsExtension.RelationalExtensionInfo
    {
      private string _logFragment;

      public ExtensionInfo(IDbContextOptionsExtension extension)
        : base(extension)
      {
      }

      public override bool IsDatabaseProvider => true;

      public override string LogFragment
      {
        get
        {
          if (this._logFragment == null)
          {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(base.LogFragment);
            this._logFragment = stringBuilder.ToString();
          }
          return this._logFragment;
        }
      }

      public override void PopulateDebugInfo([NotNull] IDictionary<string, string> debugInfo)
      {
      }
    }
  }
}
