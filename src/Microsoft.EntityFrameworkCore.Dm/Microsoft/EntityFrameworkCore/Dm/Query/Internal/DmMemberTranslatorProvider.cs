using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmMemberTranslatorProvider : RelationalMemberTranslatorProvider
	{
		public DmMemberTranslatorProvider([JetBrains.Annotations.NotNull] RelationalMemberTranslatorProviderDependencies dependencies)
			: base(dependencies)
		{
			ISqlExpressionFactory sqlExpressionFactory = dependencies.SqlExpressionFactory;
			AddTranslators(new IMemberTranslator[2]
			{
				new DmDateTimeMemberTranslator(sqlExpressionFactory),
				new DmStringMemberTranslator(sqlExpressionFactory)
			});
		}
	}
}
