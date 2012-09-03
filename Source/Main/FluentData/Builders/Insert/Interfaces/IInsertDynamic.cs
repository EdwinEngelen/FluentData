namespace FluentData
{
	public interface IInsertBuilderDynamic
	{
		int Execute();
		int ExecuteReturnLastId(string identityColumnName = null);
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		IInsertBuilderDynamic AutoMap(params string[] ignoreProperties);
		IInsertBuilderDynamic Column(string columnName, object value);
		IInsertBuilderDynamic Column(string propertyName);
	}
}