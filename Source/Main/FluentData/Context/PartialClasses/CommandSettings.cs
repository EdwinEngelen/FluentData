namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext CommandTimeout(int timeout)
		{
			ContextData.CommandTimeout = timeout;
			return this;
		}
	}
}
