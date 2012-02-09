namespace FluentData
{
	public interface IUpdateBuilderDynamic
	{
		int Execute();
		IUpdateBuilderDynamic IgnoreProperty(string name);
		IUpdateBuilderDynamic AutoMap();
		IUpdateBuilderDynamic Column(string columnName, object value);
		IUpdateBuilderDynamic Column(string propertyName);
		IUpdateBuilderDynamic Where(string name);
		IUpdateBuilderDynamic Where(string columnName, object value);
	}
}