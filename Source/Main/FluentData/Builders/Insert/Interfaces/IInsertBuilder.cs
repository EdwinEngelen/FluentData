namespace FluentData
{
	public interface IInsertBuilder
	{
		int Execute();
		int ExecuteReturnLastId(string identityColumnName = null);
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		IInsertBuilder Column(string columnName, object value);
	}
}