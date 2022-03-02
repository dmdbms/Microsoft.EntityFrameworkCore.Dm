using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	internal sealed class ContractAnnotationAttribute : Attribute
	{
		public string Contract { get; }

		public bool ForceFullStates { get; }

		public ContractAnnotationAttribute([NotNull] string contract)
			: this(contract, forceFullStates: false)
		{
		}

		public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
		{
			Contract = contract;
			ForceFullStates = forceFullStates;
		}
	}
}
