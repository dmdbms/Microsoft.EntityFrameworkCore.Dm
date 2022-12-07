using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmMemberTranslatorProvider : RelationalMemberTranslatorProvider
	{
		public DmMemberTranslatorProvider([NotNull] RelationalMemberTranslatorProviderDependencies dependencies)
			: base(dependencies)
		{
			ISqlExpressionFactory sqlExpressionFactory = dependencies.SqlExpressionFactory;
			base.AddTranslators((IEnumerable<IMemberTranslator>)(object)new IMemberTranslator[2]
			{
				new DmDateTimeMemberTranslator(sqlExpressionFactory),
				new DmStringMemberTranslator(sqlExpressionFactory)
			});
		}
	}
}
