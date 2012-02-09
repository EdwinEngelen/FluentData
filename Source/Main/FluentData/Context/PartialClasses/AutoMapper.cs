namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext ThrowExceptionIfAutoMapFails
		{
			get
			{
				ContextData.ThrowExceptionIfAutoMapFails = true;
				return this;
			}
		}
	}
}
