using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations
{
	public class DmDropUserOperation : MigrationOperation
	{
		public virtual string UserName
		{
			get; [param: JetBrains.Annotations.NotNull]
			set;
		}
	}
}
