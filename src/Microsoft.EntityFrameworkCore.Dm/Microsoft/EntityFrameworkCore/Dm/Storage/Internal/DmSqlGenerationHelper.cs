using System;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmSqlGenerationHelper : RelationalSqlGenerationHelper
	{
		public override string BatchTerminator => Environment.NewLine + "/";

		public DmSqlGenerationHelper([NotNull] RelationalSqlGenerationHelperDependencies dependencies)
			: base(dependencies)
		{
		}

		public override void GenerateParameterName(StringBuilder builder, string name)
		{
			builder.Append(":").Append(name);
		}

		public override string GenerateParameterName(string name)
		{
			return ":" + name;
		}

		public override string EscapeIdentifier(string identifier)
		{
			return Check.NotEmpty(identifier, "identifier").Replace("\"", "\"\"");
		}

		public override void EscapeIdentifier(StringBuilder builder, string identifier)
		{
			Check.NotEmpty(identifier, "identifier");
			int length = builder.Length;
			builder.Append(identifier);
			builder.Replace("\"", "\"\"", length, identifier.Length);
		}

		public override string DelimitIdentifier(string identifier)
		{
			return "\"" + ((RelationalSqlGenerationHelper)this).EscapeIdentifier(Check.NotEmpty(identifier, "identifier")) + "\"";
		}

		public override void DelimitIdentifier(StringBuilder builder, string identifier)
		{
			Check.NotEmpty(identifier, "identifier");
			builder.Append('"');
			((RelationalSqlGenerationHelper)this).EscapeIdentifier(builder, identifier);
			builder.Append('"');
		}
	}
}
