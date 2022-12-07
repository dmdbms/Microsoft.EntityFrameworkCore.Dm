using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Dm.Update.Internal
{
	public interface IDmUpdateSqlGenerator : IUpdateSqlGenerator
	{
		ResultSetMapping AppendBulkInsertOperation([NotNull] StringBuilder commandStringBuilder, [NotNull] IReadOnlyList<IReadOnlyModificationCommand> modificationCommands, int commandPosition);
	}
}
