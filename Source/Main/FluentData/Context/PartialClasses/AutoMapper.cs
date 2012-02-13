namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext IgnoreIfAutoMapFails
		{
			get
			{
				ContextData.IgnoreIfAutoMapFails = true;
				return this;
			}
		}
	}
}
