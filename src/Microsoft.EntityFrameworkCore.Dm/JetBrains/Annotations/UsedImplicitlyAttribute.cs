using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class UsedImplicitlyAttribute : Attribute
	{
		public ImplicitUseKindFlags UseKindFlags { get; }

		public ImplicitUseTargetFlags TargetFlags { get; }

		public UsedImplicitlyAttribute()
			: this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
		{
		}

		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
			: this(useKindFlags, ImplicitUseTargetFlags.Default)
		{
		}

		public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
			: this(ImplicitUseKindFlags.Default, targetFlags)
		{
		}

		public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
		{
			UseKindFlags = useKindFlags;
			TargetFlags = targetFlags;
		}
	}
}
