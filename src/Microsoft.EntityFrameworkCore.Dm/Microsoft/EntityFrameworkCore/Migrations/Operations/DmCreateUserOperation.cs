using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations
{
	public class DmCreateUserOperation : MigrationOperation
	{
		public virtual string UserName
		{
			get; [param: JetBrains.Annotations.NotNull]
			set;
		}

		public virtual string Password
		{
			get; [param: JetBrains.Annotations.NotNull]
			set;
		}
	}
}
