namespace FluentData
{
	public interface IInsertUpdateBuilderDynamic
	{
		IInsertUpdateBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IInsertUpdateBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}
