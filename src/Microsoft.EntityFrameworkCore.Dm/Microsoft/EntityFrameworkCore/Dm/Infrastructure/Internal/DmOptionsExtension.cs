using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Dm.Infrastructure.Internal
{
	public class DmOptionsExtension : RelationalOptionsExtension
	{
		private sealed class ExtensionInfo : RelationalExtensionInfo
		{
			private string _logFragment;

			private new DmOptionsExtension Extension => (DmOptionsExtension)base.Extension;

			public override bool IsDatabaseProvider => true;

			public override string LogFragment
			{
				get
				{
					if (_logFragment == null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(base.LogFragment);
						_logFragment = stringBuilder.ToString();
					}
					return _logFragment;
				}
			}

			public ExtensionInfo(IDbContextOptionsExtension extension)
				: base(extension)
			{
			}

			public override void PopulateDebugInfo([JetBrains.Annotations.NotNull] IDictionary<string, string> debugInfo)
			{
			}
		}

		private DbContextOptionsExtensionInfo _info;

		public override DbContextOptionsExtensionInfo Info => _info ?? (_info = new ExtensionInfo(this));

		public DmOptionsExtension()
		{
		}

		protected DmOptionsExtension([JetBrains.Annotations.NotNull] DmOptionsExtension copyFrom)
			: base(copyFrom)
		{
		}

		protected override RelationalOptionsExtension Clone()
		{
			return new DmOptionsExtension(this);
		}

		public override void ApplyServices(IServiceCollection services)
		{
			Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(services, "services");
			services.AddEntityFrameworkDm();
		}
	}
}
