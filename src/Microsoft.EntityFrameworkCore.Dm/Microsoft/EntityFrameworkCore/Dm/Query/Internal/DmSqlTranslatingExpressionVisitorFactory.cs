using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmSqlTranslatingExpressionVisitorFactory : IRelationalSqlTranslatingExpressionVisitorFactory
	{
		private readonly RelationalSqlTranslatingExpressionVisitorDependencies _dependencies;

		public DmSqlTranslatingExpressionVisitorFactory([JetBrains.Annotations.NotNull] RelationalSqlTranslatingExpressionVisitorDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public virtual RelationalSqlTranslatingExpressionVisitor Create(QueryCompilationContext queryCompilationContext, QueryableMethodTranslatingExpressionVisitor queryableMethodTranslatingExpressionVisitor)
		{
			return new DmSqlTranslatingExpressionVisitor(_dependencies, queryCompilationContext, queryableMethodTranslatingExpressionVisitor);
		}
	}
}
