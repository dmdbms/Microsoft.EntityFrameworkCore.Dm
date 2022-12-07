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

			private DmOptionsExtension Extension => (DmOptionsExtension)(object)((RelationalExtensionInfo)this).Extension;

			public override bool IsDatabaseProvider => true;

			public override string LogFragment
			{
				get
				{
					if (_logFragment == null)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(((RelationalExtensionInfo)this).LogFragment);
						_logFragment = stringBuilder.ToString();
					}
					return _logFragment;
				}
			}

			public ExtensionInfo(IDbContextOptionsExtension extension)
				: base(extension)
			{
			}

			public override void PopulateDebugInfo([NotNull] IDictionary<string, string> debugInfo)
			{
			}
		}

		private DbContextOptionsExtensionInfo _info;

		public override DbContextOptionsExtensionInfo Info => _info ?? (_info = (DbContextOptionsExtensionInfo)(object)new ExtensionInfo((IDbContextOptionsExtension)(object)this));

		public DmOptionsExtension()
			: base()
		{
		}

		protected DmOptionsExtension([NotNull] DmOptionsExtension copyFrom)
			: base((RelationalOptionsExtension)(object)copyFrom)
		{
		}

		protected override RelationalOptionsExtension Clone()
		{
			return (RelationalOptionsExtension)(object)new DmOptionsExtension(this);
		}

		public override void ApplyServices(IServiceCollection services)
		{
			Check.NotNull(services, "services");
			services.AddEntityFrameworkDm();
		}
	}
}
