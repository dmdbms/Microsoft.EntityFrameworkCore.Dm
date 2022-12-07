// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Internal.DmSqlStrings
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll



namespace Microsoft.EntityFrameworkCore.Internal
{
  public static class DmSqlStrings
  {
    public const string CurrentSchema = "(SELECT NAME FROM SYSOBJECTS WHERE ID = (SELECT CURRENT_SCHID()))";
    public const string ObjExistsBegin = "DECLARE CNT INT; BEGIN ";
    public const string ObjExistsEnd = " END; ";
    public const string DmSchID = " SCHID = (SELECT ID FROM SYS.SYSOBJECTS WHERE TYPE$ = 'SCH' AND NAME = ";
    public const string DmSchIDTail = ") ";
    public const string SelectCntIntoCnt = " SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM ";
    public const string ObjIntoCnt = " SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END INTO CNT FROM SYSOBJECTS WHERE NAME = ";
    public const string ObjIntoCntTail = " ";
    public const string IfCnt = "IF CNT > 0 THEN ";
    public const string EndIF = "END IF; ";
  }
}
