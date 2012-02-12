namespace FluentData
{
	public interface IInsertBuilder
	{
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		int ExecuteReturnLastId(string identityColumnName);
		T ExecuteReturnLastId<T>(string identityColumnName);
		IInsertBuilder Column(string columnName, object value);
	}
}