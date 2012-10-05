namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext CommandTimeout(int timeout)
		{
			Data.CommandTimeout = timeout;
			return this;
		}
	}
}
