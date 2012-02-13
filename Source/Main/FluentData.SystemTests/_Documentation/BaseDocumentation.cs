namespace FluentData._Documentation
{
	public class BaseDocumentation
	{
		public IDbContext Context()
		{
			return new DbContext().ConnectionStringName("SqlServer", DbProviderTypes.SqlServer);
		}
	}
}
