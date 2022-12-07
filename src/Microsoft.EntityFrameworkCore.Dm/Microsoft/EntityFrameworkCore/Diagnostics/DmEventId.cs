// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Diagnostics.DmEventId
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using Microsoft.Extensions.Logging;



namespace Microsoft.EntityFrameworkCore.Diagnostics
{
  public static class DmEventId
  {
    private static readonly string _validationPrefix = LoggerCategory<DbLoggerCategory.Model.Validation>.Name + ".";
    public static readonly EventId DecimalTypeDefaultWarning = DmEventId.MakeValidationId(DmEventId.Id.DecimalTypeDefaultWarning);
    public static readonly EventId ByteIdentityColumnWarning = DmEventId.MakeValidationId(DmEventId.Id.ByteIdentityColumnWarning);
    private static readonly string _scaffoldingPrefix = LoggerCategory<DbLoggerCategory.Scaffolding>.Name + ".";
    public static readonly EventId ColumnFound = DmEventId.MakeScaffoldingId(DmEventId.Id.ColumnFound);
    public static readonly EventId DefaultSchemaFound = DmEventId.MakeScaffoldingId(DmEventId.Id.DefaultSchemaFound);
    public static readonly EventId TypeAliasFound = DmEventId.MakeScaffoldingId(DmEventId.Id.TypeAliasFound);
    public static readonly EventId MissingSchemaWarning = DmEventId.MakeScaffoldingId(DmEventId.Id.MissingSchemaWarning);
    public static readonly EventId MissingTableWarning = DmEventId.MakeScaffoldingId(DmEventId.Id.MissingTableWarning);
    public static readonly EventId ForeignKeyReferencesMissingPrincipalTableWarning = DmEventId.MakeScaffoldingId(DmEventId.Id.ForeignKeyReferencesMissingPrincipalTableWarning);
    public static readonly EventId TableFound = DmEventId.MakeScaffoldingId(DmEventId.Id.TableFound);
    public static readonly EventId SequenceFound = DmEventId.MakeScaffoldingId(DmEventId.Id.SequenceFound);
    public static readonly EventId PrimaryKeyFound = DmEventId.MakeScaffoldingId(DmEventId.Id.PrimaryKeyFound);
    public static readonly EventId UniqueConstraintFound = DmEventId.MakeScaffoldingId(DmEventId.Id.UniqueConstraintFound);
    public static readonly EventId IndexFound = DmEventId.MakeScaffoldingId(DmEventId.Id.IndexFound);
    public static readonly EventId ForeignKeyFound = DmEventId.MakeScaffoldingId(DmEventId.Id.ForeignKeyFound);
    public static readonly EventId ForeignKeyPrincipalColumnMissingWarning = DmEventId.MakeScaffoldingId(DmEventId.Id.ForeignKeyPrincipalColumnMissingWarning);
    public static readonly EventId ReflexiveConstraintIgnored = DmEventId.MakeScaffoldingId(DmEventId.Id.ReflexiveConstraintIgnored);

    private static EventId MakeValidationId(DmEventId.Id id) => new EventId((int) id, DmEventId._validationPrefix + id.ToString());

    private static EventId MakeScaffoldingId(DmEventId.Id id) => new EventId((int) id, DmEventId._scaffoldingPrefix + id.ToString());

    private enum Id
    {
      DecimalTypeDefaultWarning = 30000, // 0x00007530
      ByteIdentityColumnWarning = 30001, // 0x00007531
      ColumnFound = 35000, // 0x000088B8
      ColumnNotNamedWarning = 35001, // 0x000088B9
      ColumnSkipped = 35002, // 0x000088BA
      DefaultSchemaFound = 35003, // 0x000088BB
      ForeignKeyColumnFound = 35004, // 0x000088BC
      ForeignKeyColumnMissingWarning = 35005, // 0x000088BD
      ForeignKeyColumnNotNamedWarning = 35006, // 0x000088BE
      ForeignKeyColumnsNotMappedWarning = 35007, // 0x000088BF
      ForeignKeyNotNamedWarning = 35008, // 0x000088C0
      ForeignKeyReferencesMissingPrincipalTableWarning = 35009, // 0x000088C1
      IndexColumnFound = 35010, // 0x000088C2
      IndexColumnNotNamedWarning = 35011, // 0x000088C3
      IndexColumnSkipped = 35012, // 0x000088C4
      IndexColumnsNotMappedWarning = 35013, // 0x000088C5
      IndexNotNamedWarning = 35014, // 0x000088C6
      IndexTableMissingWarning = 35015, // 0x000088C7
      MissingSchemaWarning = 35016, // 0x000088C8
      MissingTableWarning = 35017, // 0x000088C9
      SequenceFound = 35018, // 0x000088CA
      SequenceNotNamedWarning = 35019, // 0x000088CB
      TableFound = 35020, // 0x000088CC
      TableSkipped = 35021, // 0x000088CD
      TypeAliasFound = 35022, // 0x000088CE
      ForeignKeyTableMissingWarning = 35023, // 0x000088CF
      PrimaryKeyFound = 35024, // 0x000088D0
      UniqueConstraintFound = 35025, // 0x000088D1
      IndexFound = 35026, // 0x000088D2
      ForeignKeyFound = 35027, // 0x000088D3
      ForeignKeyPrincipalColumnMissingWarning = 35028, // 0x000088D4
      ReflexiveConstraintIgnored = 35029, // 0x000088D5
    }
  }
}
