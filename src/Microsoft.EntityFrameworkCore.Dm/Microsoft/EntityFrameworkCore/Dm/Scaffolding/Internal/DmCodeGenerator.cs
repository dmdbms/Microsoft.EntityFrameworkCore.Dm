using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
	public class DmCodeGenerator : ProviderCodeGenerator
	{
		public DmCodeGenerator([JetBrains.Annotations.NotNull] ProviderCodeGeneratorDependencies dependencies)
			: base(dependencies)
		{
		}

		public override MethodCallCodeFragment GenerateUseProvider(string connectionString, MethodCallCodeFragment providerOptions)
		{
			return new MethodCallCodeFragment("UseDm", (providerOptions != null) ? new object[2]
			{
				connectionString,
				new NestedClosureCodeFragment("x", providerOptions)
			} : new object[1] { connectionString });
		}
	}
}
