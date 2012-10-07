namespace FluentData
{
	public interface IUpdateBuilderDynamic
	{
		int Execute();
		IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties);
		IUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Where(string name, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilderDynamic Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}