using Microsoft.Extensions.Logging;

namespace Microsoft.EntityFrameworkCore.Diagnostics
{
	public static class DmEventId
	{
		private enum Id
		{
			DecimalTypeDefaultWarning = 30000,
			ByteIdentityColumnWarning = 30001,
			ColumnFound = 35000,
			ColumnNotNamedWarning = 35001,
			ColumnSkipped = 35002,
			DefaultSchemaFound = 35003,
			ForeignKeyColumnFound = 35004,
			ForeignKeyColumnMissingWarning = 35005,
			ForeignKeyColumnNotNamedWarning = 35006,
			ForeignKeyColumnsNotMappedWarning = 35007,
			ForeignKeyNotNamedWarning = 35008,
			ForeignKeyReferencesMissingPrincipalTableWarning = 35009,
			IndexColumnFound = 35010,
			IndexColumnNotNamedWarning = 35011,
			IndexColumnSkipped = 35012,
			IndexColumnsNotMappedWarning = 35013,
			IndexNotNamedWarning = 35014,
			IndexTableMissingWarning = 35015,
			MissingSchemaWarning = 35016,
			MissingTableWarning = 35017,
			SequenceFound = 35018,
			SequenceNotNamedWarning = 35019,
			TableFound = 35020,
			TableSkipped = 35021,
			TypeAliasFound = 35022,
			ForeignKeyTableMissingWarning = 35023,
			PrimaryKeyFound = 35024,
			UniqueConstraintFound = 35025,
			IndexFound = 35026,
			ForeignKeyFound = 35027,
			ForeignKeyPrincipalColumnMissingWarning = 35028,
			ReflexiveConstraintIgnored = 35029
		}

		private static readonly string _validationPrefix = LoggerCategory<DbLoggerCategory.Model.Validation>.Name + ".";

		public static readonly EventId DecimalTypeDefaultWarning = MakeValidationId(Id.DecimalTypeDefaultWarning);

		public static readonly EventId ByteIdentityColumnWarning = MakeValidationId(Id.ByteIdentityColumnWarning);

		private static readonly string _scaffoldingPrefix = LoggerCategory<DbLoggerCategory.Scaffolding>.Name + ".";

		public static readonly EventId ColumnFound = MakeScaffoldingId(Id.ColumnFound);

		public static readonly EventId DefaultSchemaFound = MakeScaffoldingId(Id.DefaultSchemaFound);

		public static readonly EventId TypeAliasFound = MakeScaffoldingId(Id.TypeAliasFound);

		public static readonly EventId MissingSchemaWarning = MakeScaffoldingId(Id.MissingSchemaWarning);

		public static readonly EventId MissingTableWarning = MakeScaffoldingId(Id.MissingTableWarning);

		public static readonly EventId ForeignKeyReferencesMissingPrincipalTableWarning = MakeScaffoldingId(Id.ForeignKeyReferencesMissingPrincipalTableWarning);

		public static readonly EventId TableFound = MakeScaffoldingId(Id.TableFound);

		public static readonly EventId SequenceFound = MakeScaffoldingId(Id.SequenceFound);

		public static readonly EventId PrimaryKeyFound = MakeScaffoldingId(Id.PrimaryKeyFound);

		public static readonly EventId UniqueConstraintFound = MakeScaffoldingId(Id.UniqueConstraintFound);

		public static readonly EventId IndexFound = MakeScaffoldingId(Id.IndexFound);

		public static readonly EventId ForeignKeyFound = MakeScaffoldingId(Id.ForeignKeyFound);

		public static readonly EventId ForeignKeyPrincipalColumnMissingWarning = MakeScaffoldingId(Id.ForeignKeyPrincipalColumnMissingWarning);

		public static readonly EventId ReflexiveConstraintIgnored = MakeScaffoldingId(Id.ReflexiveConstraintIgnored);

		private static EventId MakeValidationId(Id id)
		{
			return new EventId((int)id, _validationPrefix + id);
		}

		private static EventId MakeScaffoldingId(Id id)
		{
			return new EventId((int)id, _scaffoldingPrefix + id);
		}
	}
}
