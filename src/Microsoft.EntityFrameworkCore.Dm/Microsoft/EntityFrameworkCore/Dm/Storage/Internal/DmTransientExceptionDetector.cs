using System;
using Dm;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTransientExceptionDetector
	{
		public static bool ShouldRetryOn([JetBrains.Annotations.NotNull] Exception ex)
		{
			DmException ex2 = ex as DmException;
			if (ex2 != null)
			{
				return false;
			}
			if (ex is TimeoutException)
			{
				return true;
			}
			return false;
		}
	}
}
