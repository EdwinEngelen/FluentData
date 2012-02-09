namespace FluentData
{
	public interface IInsertUpdateBuilderDynamic
	{
		IInsertUpdateBuilderDynamic IgnoreProperty(string name);
		IInsertUpdateBuilderDynamic AutoMap();
		IInsertUpdateBuilderDynamic Column(string columnName, object value);
		IInsertUpdateBuilderDynamic Column(string propertyName);
	}
}
