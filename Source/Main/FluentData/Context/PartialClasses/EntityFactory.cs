namespace FluentData
{
	public partial class DbContext : IDbContext
	{
		/// <summary>
		/// Set a custom object locator to override the way the entity and list instances are created.
		/// </summary>
		/// <param name="entityFactory"></param>
		public IDbContext EntityFactory(IEntityFactory entityFactory)
		{
			ContextData.EntityFactory = entityFactory;
			return this;
		}
	}
}
