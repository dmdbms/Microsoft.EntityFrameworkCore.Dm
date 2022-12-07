using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Dm.Internal;

namespace Microsoft.EntityFrameworkCore.Dm.Extensions
{
	public static class DmDbFunctionsExtensions
	{
		public static bool FreeText([CanBeNull] this DbFunctions _, [NotNull] string propertyReference, [NotNull] string freeText, int languageTerm)
		{
			return FreeTextCore(propertyReference, freeText, languageTerm);
		}

		public static bool FreeText([CanBeNull] this DbFunctions _, [NotNull] string propertyReference, [NotNull] string freeText)
		{
			return FreeTextCore(propertyReference, freeText, null);
		}

		private static bool FreeTextCore(string propertyName, string freeText, int? languageTerm)
		{
			throw new InvalidOperationException(DmStrings.FunctionOnClient("FreeText"));
		}

		public static bool Contains([CanBeNull] this DbFunctions _, [NotNull] string propertyReference, [NotNull] string searchCondition, int languageTerm)
		{
			return ContainsCore(propertyReference, searchCondition, languageTerm);
		}

		public static bool Contains([CanBeNull] this DbFunctions _, [NotNull] string propertyReference, [NotNull] string searchCondition)
		{
			return ContainsCore(propertyReference, searchCondition, null);
		}

		private static bool ContainsCore(string propertyName, string searchCondition, int? languageTerm)
		{
			throw new InvalidOperationException(DmStrings.FunctionOnClient("Contains"));
		}

		public static int DateDiffYear([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return endDate.Year - startDate.Year;
		}

		public static int? DateDiffYear([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffYear(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffYear([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffYear(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffYear([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffYear(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMonth([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return 12 * (endDate.Year - startDate.Year) + endDate.Month - startDate.Month;
		}

		public static int? DateDiffMonth([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMonth(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMonth([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffMonth(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffMonth([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMonth(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffDay([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return (endDate.Date - startDate.Date).Days;
		}

		public static int? DateDiffDay([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffDay(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffDay([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffDay(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffDay([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffDay(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffHour([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return checked(_.DateDiffDay(startDate, endDate) * 24 + endDate.Hour - startDate.Hour);
		}

		public static int? DateDiffHour([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffHour(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffHour([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffHour(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffHour([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffHour(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffHour([CanBeNull] this DbFunctions _, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
		{
			return checked(endTimeSpan.Hours - startTimeSpan.Hours);
		}

		public static int? DateDiffHour([CanBeNull] this DbFunctions _, TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
		{
			return (startTimeSpan.HasValue && endTimeSpan.HasValue) ? new int?(_.DateDiffHour(startTimeSpan.Value, endTimeSpan.Value)) : null;
		}

		public static int DateDiffMinute([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return checked(_.DateDiffHour(startDate, endDate) * 60 + endDate.Minute - startDate.Minute);
		}

		public static int? DateDiffMinute([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMinute(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMinute([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffMinute(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffMinute([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMinute(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMinute([CanBeNull] this DbFunctions _, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
		{
			return checked(_.DateDiffHour(startTimeSpan, endTimeSpan) * 60 + endTimeSpan.Minutes - startTimeSpan.Minutes);
		}

		public static int? DateDiffMinute([CanBeNull] this DbFunctions _, TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
		{
			return (startTimeSpan.HasValue && endTimeSpan.HasValue) ? new int?(_.DateDiffMinute(startTimeSpan.Value, endTimeSpan.Value)) : null;
		}

		public static int DateDiffSecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return checked(_.DateDiffMinute(startDate, endDate) * 60 + endDate.Second - startDate.Second);
		}

		public static int? DateDiffSecond([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffSecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffSecond([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffSecond(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffSecond([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffSecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffSecond([CanBeNull] this DbFunctions _, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
		{
			return checked(_.DateDiffMinute(startTimeSpan, endTimeSpan) * 60 + endTimeSpan.Seconds - startTimeSpan.Seconds);
		}

		public static int? DateDiffSecond([CanBeNull] this DbFunctions _, TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
		{
			return (startTimeSpan.HasValue && endTimeSpan.HasValue) ? new int?(_.DateDiffSecond(startTimeSpan.Value, endTimeSpan.Value)) : null;
		}

		public static int DateDiffMillisecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return checked(_.DateDiffSecond(startDate, endDate) * 1000 + endDate.Millisecond - startDate.Millisecond);
		}

		public static int? DateDiffMillisecond([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMillisecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMillisecond([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffMillisecond(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffMillisecond([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMillisecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMillisecond([CanBeNull] this DbFunctions _, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
		{
			return checked(_.DateDiffSecond(startTimeSpan, endTimeSpan) * 1000 + endTimeSpan.Milliseconds - startTimeSpan.Milliseconds);
		}

		public static int? DateDiffMillisecond([CanBeNull] this DbFunctions _, TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
		{
			return (startTimeSpan.HasValue && endTimeSpan.HasValue) ? new int?(_.DateDiffMillisecond(startTimeSpan.Value, endTimeSpan.Value)) : null;
		}

		public static int DateDiffMicrosecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			checked
			{
				return (int)unchecked(checked(endDate.Ticks - startDate.Ticks) / 10);
			}
		}

		public static int? DateDiffMicrosecond([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMicrosecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMicrosecond([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffMicrosecond(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffMicrosecond([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffMicrosecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffMicrosecond([CanBeNull] this DbFunctions _, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
		{
			checked
			{
				return (int)unchecked(checked(endTimeSpan.Ticks - startTimeSpan.Ticks) / 10);
			}
		}

		public static int? DateDiffMicrosecond([CanBeNull] this DbFunctions _, TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
		{
			return (startTimeSpan.HasValue && endTimeSpan.HasValue) ? new int?(_.DateDiffMicrosecond(startTimeSpan.Value, endTimeSpan.Value)) : null;
		}

		public static int DateDiffNanosecond([CanBeNull] this DbFunctions _, DateTime startDate, DateTime endDate)
		{
			return checked((int)((endDate.Ticks - startDate.Ticks) * 100));
		}

		public static int? DateDiffNanosecond([CanBeNull] this DbFunctions _, DateTime? startDate, DateTime? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffNanosecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffNanosecond([CanBeNull] this DbFunctions _, DateTimeOffset startDate, DateTimeOffset endDate)
		{
			return _.DateDiffNanosecond(startDate.UtcDateTime, endDate.UtcDateTime);
		}

		public static int? DateDiffNanosecond([CanBeNull] this DbFunctions _, DateTimeOffset? startDate, DateTimeOffset? endDate)
		{
			return (startDate.HasValue && endDate.HasValue) ? new int?(_.DateDiffNanosecond(startDate.Value, endDate.Value)) : null;
		}

		public static int DateDiffNanosecond([CanBeNull] this DbFunctions _, TimeSpan startTimeSpan, TimeSpan endTimeSpan)
		{
			return checked((int)((endTimeSpan.Ticks - startTimeSpan.Ticks) * 100));
		}

		public static int? DateDiffNanosecond([CanBeNull] this DbFunctions _, TimeSpan? startTimeSpan, TimeSpan? endTimeSpan)
		{
			return (startTimeSpan.HasValue && endTimeSpan.HasValue) ? new int?(_.DateDiffNanosecond(startTimeSpan.Value, endTimeSpan.Value)) : null;
		}

		public static bool IsDate([CanBeNull] this DbFunctions _, [NotNull] string expression)
		{
			throw new InvalidOperationException(DmStrings.FunctionOnClient("IsDate"));
		}
	}
}
