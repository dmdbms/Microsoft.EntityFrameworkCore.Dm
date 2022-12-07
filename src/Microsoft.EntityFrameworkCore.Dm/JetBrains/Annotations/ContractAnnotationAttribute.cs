// Decompiled with JetBrains decompiler
// Type: JetBrains.Annotations.ContractAnnotationAttribute
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using System;



namespace JetBrains.Annotations
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  internal sealed class ContractAnnotationAttribute : Attribute
  {
    public string Contract { get; }

    public bool ForceFullStates { get; }

    public ContractAnnotationAttribute([NotNull] string contract)
      : this(contract, false)
    {
    }

    public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
    {
      this.Contract = contract;
      this.ForceFullStates = forceFullStates;
    }
  }
}
