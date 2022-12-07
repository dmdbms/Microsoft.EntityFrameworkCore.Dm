// Decompiled with JetBrains decompiler
// Type: Microsoft.EntityFrameworkCore.Dm.Extensions.DmDbFunctionsExtensions
// Assembly: Microsoft.EntityFrameworkCore.Dm, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 517571CD-6A2C-4476-8E0F-892E361CCCD8
// Assembly location: E:\主同步盘\我的坚果云\桌面文件夹\Microsoft.EntityFrameworkCore.Dm.dll

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;
using System;



namespace Microsoft.EntityFrameworkCore.Dm.Extensions
{
  public static class DmDbFunctionsExtensions
  {
    public static bool FreeText(
      [CanBeNull] this DbFunctions _,
      [NotNull] string propertyReference,
      [NotNull] string freeText,
      int languageTerm)
    {
      return DmDbFunctionsExtensions.FreeTextCore(propertyReference, freeText, new int?(languageTerm));
    }

    public static bool FreeText([CanBeNull] this DbFunctions _, [NotNull] string propertyReference, [NotNull] string freeText) => DmDbFunctionsExtensions.FreeTextCore(propertyReference, freeText, new int?());

    private static bool FreeTextCore(string propertyName, string freeText, int? languageTerm) => throw new InvalidOperationException(DmStrings.FunctionOnClient((object) "FreeText"));

    public static bool Contains(
      [CanBeNull] this DbFunctions _,
      [NotNull] string propertyReference,
      [NotNull] string searchCondition,
      int languageTerm)
    {
      return DmDbFunctionsExtensions.ContainsCore(propertyReference, searchCondition, new int?(languageTerm));
    }

    public static bool Contains(
      [CanBeNull] this DbFunctions _,
      [NotNull] string propertyReference,
      [NotNull] string searchCondition)
    {
      return DmDbFunctionsExtensions.ContainsCore(propertyReference, searchCondition, new int?());
    }

    private static bool ContainsCore(
      string propertyName,
      string searchCondition,
      int? languageTerm)
    {
      throw new InvalidOperationException(DmStrings.FunctionOnClient((object) "Contains"));
    }

    public static int DateDiffYear([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => endDate.Year - startDate.Year;

    public static int? DateDiffYear([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate) => !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffYear(startDate.Value, endDate.Value));

    public static int DateDiffYear(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffYear(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffYear(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffYear(startDate.Value, endDate.Value));
    }

    public static int DateDiffMonth([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => 12 * (endDate.Year - startDate.Year) + endDate.Month - startDate.Month;

    public static int? DateDiffMonth([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate) => !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMonth(startDate.Value, endDate.Value));

    public static int DateDiffMonth(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffMonth(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffMonth(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMonth(startDate.Value, endDate.Value));
    }

    public static int DateDiffDay([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => (endDate.Date - startDate.Date).Days;

    public static int? DateDiffDay([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate) => !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffDay(startDate.Value, endDate.Value));

    public static int DateDiffDay(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffDay(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffDay(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffDay(startDate.Value, endDate.Value));
    }

    public static int DateDiffHour([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => checked (_.DateDiffDay(startDate, endDate) * 24 + endDate.Hour - startDate.Hour);

    public static int? DateDiffHour([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate) => !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffHour(startDate.Value, endDate.Value));

    public static int DateDiffHour(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffHour(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffHour(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffHour(startDate.Value, endDate.Value));
    }

    public static int DateDiffHour(
      [CanBeNull] this DbFunctions _,
      TimeSpan startTimeSpan,
      TimeSpan endTimeSpan)
    {
      return checked (endTimeSpan.Hours - startTimeSpan.Hours);
    }

    public static int? DateDiffHour(
      [CanBeNull] this DbFunctions _,
      TimeSpan? startTimeSpan,
      TimeSpan? endTimeSpan)
    {
      return !startTimeSpan.HasValue || !endTimeSpan.HasValue ? new int?() : new int?(_.DateDiffHour(startTimeSpan.Value, endTimeSpan.Value));
    }

    public static int DateDiffMinute([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => checked (_.DateDiffHour(startDate, endDate) * 60 + endDate.Minute - startDate.Minute);

    public static int? DateDiffMinute([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate) => !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMinute(startDate.Value, endDate.Value));

    public static int DateDiffMinute(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffMinute(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffMinute(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMinute(startDate.Value, endDate.Value));
    }

    public static int DateDiffMinute(
      [CanBeNull] this DbFunctions _,
      TimeSpan startTimeSpan,
      TimeSpan endTimeSpan)
    {
      return checked (_.DateDiffHour(startTimeSpan, endTimeSpan) * 60 + endTimeSpan.Minutes - startTimeSpan.Minutes);
    }

    public static int? DateDiffMinute(
      [CanBeNull] this DbFunctions _,
      TimeSpan? startTimeSpan,
      TimeSpan? endTimeSpan)
    {
      return !startTimeSpan.HasValue || !endTimeSpan.HasValue ? new int?() : new int?(_.DateDiffMinute(startTimeSpan.Value, endTimeSpan.Value));
    }

    public static int DateDiffSecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => checked (_.DateDiffMinute(startDate, endDate) * 60 + endDate.Second - startDate.Second);

    public static int? DateDiffSecond([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate) => !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffSecond(startDate.Value, endDate.Value));

    public static int DateDiffSecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffSecond(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffSecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffSecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffSecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan startTimeSpan,
      TimeSpan endTimeSpan)
    {
      return checked (_.DateDiffMinute(startTimeSpan, endTimeSpan) * 60 + endTimeSpan.Seconds - startTimeSpan.Seconds);
    }

    public static int? DateDiffSecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan? startTimeSpan,
      TimeSpan? endTimeSpan)
    {
      return !startTimeSpan.HasValue || !endTimeSpan.HasValue ? new int?() : new int?(_.DateDiffSecond(startTimeSpan.Value, endTimeSpan.Value));
    }

    public static int DateDiffMillisecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => checked (_.DateDiffSecond(startDate, endDate) * 1000 + endDate.Millisecond - startDate.Millisecond);

    public static int? DateDiffMillisecond(
      [CanBeNull] this DbFunctions _,
      DateTime? startDate,
      DateTime? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMillisecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffMillisecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffMillisecond(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffMillisecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMillisecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffMillisecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan startTimeSpan,
      TimeSpan endTimeSpan)
    {
      return checked (_.DateDiffSecond(startTimeSpan, endTimeSpan) * 1000 + endTimeSpan.Milliseconds - startTimeSpan.Milliseconds);
    }

    public static int? DateDiffMillisecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan? startTimeSpan,
      TimeSpan? endTimeSpan)
    {
      return !startTimeSpan.HasValue || !endTimeSpan.HasValue ? new int?() : new int?(_.DateDiffMillisecond(startTimeSpan.Value, endTimeSpan.Value));
    }

    public static int DateDiffMicrosecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => checked ((int) unchecked (checked (endDate.Ticks - startDate.Ticks) / 10L));

    public static int? DateDiffMicrosecond(
      [CanBeNull] this DbFunctions _,
      DateTime? startDate,
      DateTime? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMicrosecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffMicrosecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffMicrosecond(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffMicrosecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffMicrosecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffMicrosecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan startTimeSpan,
      TimeSpan endTimeSpan)
    {
      return checked ((int) unchecked (checked (endTimeSpan.Ticks - startTimeSpan.Ticks) / 10L));
    }

    public static int? DateDiffMicrosecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan? startTimeSpan,
      TimeSpan? endTimeSpan)
    {
      return !startTimeSpan.HasValue || !endTimeSpan.HasValue ? new int?() : new int?(_.DateDiffMicrosecond(startTimeSpan.Value, endTimeSpan.Value));
    }

    public static int DateDiffNanosecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate) => checked ((int) ((endDate.Ticks - startDate.Ticks) * 100L));

    public static int? DateDiffNanosecond(
      [CanBeNull] this DbFunctions _,
      DateTime? startDate,
      DateTime? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffNanosecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffNanosecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset startDate,
      DateTimeOffset endDate)
    {
      return _.DateDiffNanosecond(startDate.UtcDateTime, endDate.UtcDateTime);
    }

    public static int? DateDiffNanosecond(
      [CanBeNull] this DbFunctions _,
      DateTimeOffset? startDate,
      DateTimeOffset? endDate)
    {
      return !startDate.HasValue || !endDate.HasValue ? new int?() : new int?(_.DateDiffNanosecond(startDate.Value, endDate.Value));
    }

    public static int DateDiffNanosecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan startTimeSpan,
      TimeSpan endTimeSpan)
    {
      return checked ((int) ((endTimeSpan.Ticks - startTimeSpan.Ticks) * 100L));
    }

    public static int? DateDiffNanosecond(
      [CanBeNull] this DbFunctions _,
      TimeSpan? startTimeSpan,
      TimeSpan? endTimeSpan)
    {
      return !startTimeSpan.HasValue || !endTimeSpan.HasValue ? new int?() : new int?(_.DateDiffNanosecond(startTimeSpan.Value, endTimeSpan.Value));
    }

    public static bool IsDate([CanBeNull] this DbFunctions _, [NotNull] string expression) => throw new InvalidOperationException(DmStrings.FunctionOnClient((object) nameof (IsDate)));
  }
}
