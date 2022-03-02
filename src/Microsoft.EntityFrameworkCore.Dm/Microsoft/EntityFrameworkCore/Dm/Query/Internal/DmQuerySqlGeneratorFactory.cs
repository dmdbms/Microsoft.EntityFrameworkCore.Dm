using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmQuerySqlGeneratorFactory : IQuerySqlGeneratorFactory
	{
		private readonly QuerySqlGeneratorDependencies _dependencies;

		public DmQuerySqlGeneratorFactory(QuerySqlGeneratorDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public virtual QuerySqlGenerator Create()
		{
			return new DmQuerySqlGenerator(_dependencies);
		}
	}
}
