using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query;

namespace Microsoft.EntityFrameworkCore.Dm.Query.Internal
{
	public class DmMethodCallTranslatorProvider : RelationalMethodCallTranslatorProvider
	{
		public DmMethodCallTranslatorProvider([JetBrains.Annotations.NotNull] RelationalMethodCallTranslatorProviderDependencies dependencies)
			: base(dependencies)
		{
			ISqlExpressionFactory sqlExpressionFactory = dependencies.SqlExpressionFactory;
			AddTranslators(new IMethodCallTranslator[9]
			{
				new DmConvertTranslator(sqlExpressionFactory),
				new DmDateTimeMethodTranslator(sqlExpressionFactory),
				new DmDateDiffFunctionsTranslator(sqlExpressionFactory),
				new DmFullTextSearchFunctionsTranslator(sqlExpressionFactory),
				new DmIsDateFunctionTranslator(sqlExpressionFactory),
				new DmMathTranslator(sqlExpressionFactory),
				new DmNewGuidTranslator(sqlExpressionFactory),
				new DmObjectToStringTranslator(sqlExpressionFactory),
				new DmStringMethodTranslator(sqlExpressionFactory)
			});
		}
	}
}
