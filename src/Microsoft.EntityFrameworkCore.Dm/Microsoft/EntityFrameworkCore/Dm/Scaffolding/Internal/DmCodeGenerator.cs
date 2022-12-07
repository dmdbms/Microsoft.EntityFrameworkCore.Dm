using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace Microsoft.EntityFrameworkCore.Dm.Scaffolding.Internal
{
	public class DmCodeGenerator : ProviderCodeGenerator
	{
		public DmCodeGenerator([NotNull] ProviderCodeGeneratorDependencies dependencies)
			: base(dependencies)
		{
		}

		public override MethodCallCodeFragment GenerateUseProvider(string connectionString, MethodCallCodeFragment providerOptions)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			return new MethodCallCodeFragment("UseDm", (providerOptions != null) ? new object[2]
			{
				connectionString,
				(object)new NestedClosureCodeFragment("x", providerOptions)
			} : new object[1] { connectionString });
		}
	}
}
