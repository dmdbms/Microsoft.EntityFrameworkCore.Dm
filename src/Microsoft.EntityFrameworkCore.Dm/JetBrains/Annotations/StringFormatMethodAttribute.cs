using System;

namespace JetBrains.Annotations
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
	internal sealed class StringFormatMethodAttribute : Attribute
	{
		[NotNull]
		public string FormatParameterName { get; }

		public StringFormatMethodAttribute([NotNull] string formatParameterName)
		{
			FormatParameterName = formatParameterName;
		}
	}
}
