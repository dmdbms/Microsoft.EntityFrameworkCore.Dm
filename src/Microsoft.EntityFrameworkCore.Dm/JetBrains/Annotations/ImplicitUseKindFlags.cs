// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ImplicitUseKindFlags
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using System;

namespace JetBrains.Annotations
{
  [Flags]
  internal enum ImplicitUseKindFlags
  {
    Default = 7,
    Access = 1,
    Assign = 2,
    InstantiatedWithFixedConstructorSignature = 4,
    InstantiatedNoFixedConstructorSignature = 8,
  }
}
