using System;
using Dm;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Dm.Storage.Internal
{
	public class DmTransientExceptionDetector
	{
		public static bool ShouldRetryOn([NotNull] Exception ex)
		{
			DmException val = (DmException)(object)((ex is DmException) ? ex : null);
			if (val != null)
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
