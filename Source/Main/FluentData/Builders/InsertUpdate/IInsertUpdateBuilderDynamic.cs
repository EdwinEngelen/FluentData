namespace FluentData
{
	public interface IInsertUpdateBuilderDynamic
	{
		IInsertUpdateBuilderDynamic AutoMap(params string[] ignoreProperties);
		IInsertUpdateBuilderDynamic Column(string columnName, object value);
		IInsertUpdateBuilderDynamic Column(string propertyName);
	}
}
