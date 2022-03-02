using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmQueryTranslationPostprocessorFactory : IQueryTranslationPostprocessorFactory
	{
		private readonly QueryTranslationPostprocessorDependencies _dependencies;

		private readonly RelationalQueryTranslationPostprocessorDependencies _relationalDependencies;

		public DmQueryTranslationPostprocessorFactory(QueryTranslationPostprocessorDependencies dependencies, RelationalQueryTranslationPostprocessorDependencies relationalDependencies)
		{
			_dependencies = dependencies;
			_relationalDependencies = relationalDependencies;
		}

		public virtual QueryTranslationPostprocessor Create(QueryCompilationContext queryCompilationContext)
		{
			return new DmQueryTranslationPostprocessor(_dependencies, _relationalDependencies, queryCompilationContext);
		}
	}
}
