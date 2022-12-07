// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Diagnostics.Internal.DmLoggingDefinitions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.EntityFrameworkCore.Diagnostics;



namespace Microsoft.EntityFrameworkCore.Dm.Diagnostics.Internal
{
  public class DmLoggingDefinitions : RelationalLoggingDefinitions
  {
    public EventDefinitionBase LogDefaultDecimalTypeColumn;
    public EventDefinitionBase LogByteIdentityColumn;
    public EventDefinitionBase LogFoundDefaultSchema;
    public EventDefinitionBase LogFoundTypeAlias;
    public EventDefinitionBase LogFoundColumn;
    public EventDefinitionBase LogFoundForeignKey;
    public EventDefinitionBase LogPrincipalTableNotInSelectionSet;
    public EventDefinitionBase LogMissingSchema;
    public EventDefinitionBase LogMissingTable;
    public EventDefinitionBase LogFoundSequence;
    public EventDefinitionBase LogFoundTable;
    public EventDefinitionBase LogFoundIndex;
    public EventDefinitionBase LogFoundPrimaryKey;
    public EventDefinitionBase LogFoundUniqueConstraint;
    public EventDefinitionBase LogPrincipalColumnNotFound;
    public EventDefinitionBase LogReflexiveConstraintIgnored;
  }
}
