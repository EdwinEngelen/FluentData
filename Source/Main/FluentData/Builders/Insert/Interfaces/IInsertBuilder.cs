namespace FluentData
{
	public interface IInsertBuilder
	{
		int Execute();
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		IInsertBuilder Column(string columnName, object value);
	}
}