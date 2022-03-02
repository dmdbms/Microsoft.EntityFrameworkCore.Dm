using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Delegate)]
	internal sealed class NotNullAttribute : Attribute
	{
	}
}
