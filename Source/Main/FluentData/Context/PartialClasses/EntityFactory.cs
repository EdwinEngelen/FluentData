namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext EntityFactory(IEntityFactory entityFactory)
		{
			Data.EntityFactory = entityFactory;
			return this;
		}
	}
}
