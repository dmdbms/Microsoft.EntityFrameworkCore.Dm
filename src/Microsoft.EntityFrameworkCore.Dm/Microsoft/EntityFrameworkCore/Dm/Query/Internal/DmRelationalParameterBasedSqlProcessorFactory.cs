using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	internal class DmRelationalParameterBasedSqlProcessorFactory : IRelationalParameterBasedSqlProcessorFactory
	{
		private readonly RelationalParameterBasedSqlProcessorDependencies _dependencies;

		public DmRelationalParameterBasedSqlProcessorFactory(RelationalParameterBasedSqlProcessorDependencies dependencies)
		{
			_dependencies = dependencies;
		}

		public virtual RelationalParameterBasedSqlProcessor Create(bool useRelationalNulls)
		{
			return new DmRelationalParameterBasedSqlProcessor(_dependencies, useRelationalNulls);
		}
	}
}
