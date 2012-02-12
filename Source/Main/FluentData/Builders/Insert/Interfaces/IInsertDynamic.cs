namespace FluentData
{
	public interface IInsertBuilderDynamic
	{
		int Execute();
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		int ExecuteReturnLastId(string identityColumnName);
		T ExecuteReturnLastId<T>(string identityColumnName);
		IInsertBuilderDynamic IgnoreProperty(string name);
		IInsertBuilderDynamic AutoMap();
		IInsertBuilderDynamic Column(string columnName, object value);
		IInsertBuilderDynamic Column(string propertyName);
	}
}