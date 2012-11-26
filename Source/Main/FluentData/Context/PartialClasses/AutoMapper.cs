namespace FluentData
{
	public partial class DbContext
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
