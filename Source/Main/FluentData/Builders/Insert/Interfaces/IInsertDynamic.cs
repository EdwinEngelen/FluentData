namespace FluentData
{
	public interface IInsertBuilderDynamic : IExecute, IExecuteReturnLastId
	{
		IInsertBuilderDynamic AutoMap(params string[] ignoreProperties);
		IInsertBuilderDynamic Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IInsertBuilderDynamic Column(string propertyName, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}