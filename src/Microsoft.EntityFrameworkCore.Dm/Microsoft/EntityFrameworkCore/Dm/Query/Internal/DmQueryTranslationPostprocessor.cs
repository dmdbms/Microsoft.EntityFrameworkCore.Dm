using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmQueryTranslationPostprocessor : RelationalQueryTranslationPostprocessor
	{
		public DmQueryTranslationPostprocessor(QueryTranslationPostprocessorDependencies dependencies, RelationalQueryTranslationPostprocessorDependencies relationalDependencies, QueryCompilationContext queryCompilationContext)
			: base(dependencies, relationalDependencies, queryCompilationContext)
		{
		}

		public override Expression Process(Expression query)
		{
			query = base.Process(query);
			query = new SearchConditionConvertingExpressionVisitor(RelationalDependencies.SqlExpressionFactory).Visit(query);
			return query;
		}
	}
}
