namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		public IDbContext EntityFactory(IEntityFactory entityFactory)
		{
			ContextData.EntityFactory = entityFactory;
			return this;
		}
	}
}
