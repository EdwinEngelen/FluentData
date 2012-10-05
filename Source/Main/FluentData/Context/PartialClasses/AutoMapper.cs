namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext IgnoreIfAutoMapFails
		{
			get
			{
				Data.IgnoreIfAutoMapFails = true;
				return this;
			}
		}
	}
}
