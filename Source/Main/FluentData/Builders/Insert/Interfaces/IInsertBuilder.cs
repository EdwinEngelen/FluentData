namespace FluentData
{
	public interface IInsertBuilder : IExecute, IExecuteReturnLastId
	{
		IInsertBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}