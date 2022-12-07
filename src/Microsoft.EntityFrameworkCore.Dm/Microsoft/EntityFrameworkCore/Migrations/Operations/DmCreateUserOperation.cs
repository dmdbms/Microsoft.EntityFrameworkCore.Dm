using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations
{
	public class DmCreateUserOperation : MigrationOperation
	{
		public virtual string UserName
		{
			get; [param: NotNull]
			set;
		}

		public virtual string Password
		{
			get; [param: NotNull]
			set;
		}

		public DmCreateUserOperation()
		{
		}
	}
}
