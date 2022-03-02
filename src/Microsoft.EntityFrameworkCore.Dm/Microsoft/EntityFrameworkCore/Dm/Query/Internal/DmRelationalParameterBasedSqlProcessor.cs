using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	internal class DmRelationalParameterBasedSqlProcessor : RelationalParameterBasedSqlProcessor
	{
		public DmRelationalParameterBasedSqlProcessor(RelationalParameterBasedSqlProcessorDependencies dependencies, bool useRelationalNulls)
			: base(dependencies, useRelationalNulls)
		{
		}

		protected override SelectExpression ProcessSqlNullability(SelectExpression selectExpression, IReadOnlyDictionary<string, object?> parametersValues, out bool canCache)
		{
			return new DmSqlNullabilityProcessor(Dependencies, UseRelationalNulls).Process(selectExpression, parametersValues, out canCache);
		}
	}
}
