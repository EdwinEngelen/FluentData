namespace FluentData
{
	public interface ISelectBuilder : IQuery
	{
		BuilderData Data { get; set; }
		ISelectBuilder Select(string sql);
		ISelectBuilder From(string sql);
		ISelectBuilder Where(string sql);
		ISelectBuilder AndWhere(string sql);
		ISelectBuilder OrWhere(string sql);
		ISelectBuilder GroupBy(string sql);
		ISelectBuilder OrderBy(string sql);
		ISelectBuilder Having(string sql);
		ISelectBuilder Paging(int currentPage, int itemsPerPage);
		ISelectBuilder Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0);
		ISelectBuilder Parameters(params object[] parameters);
	}
}
