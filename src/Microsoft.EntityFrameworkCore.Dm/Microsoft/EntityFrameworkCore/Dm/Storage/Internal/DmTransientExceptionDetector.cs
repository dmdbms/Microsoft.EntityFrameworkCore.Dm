// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Storage.Internal.DmTransientExceptionDetector
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Dm;
using JetBrains.Annotations;
using System;



namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
  public class DmTransientExceptionDetector
  {
    public static bool ShouldRetryOn([NotNull] Exception ex)
    {
      switch (ex)
      {
        case DmException _:
          return false;
        case TimeoutException _:
          return true;
        default:
          return false;
      }
    }
  }
}
