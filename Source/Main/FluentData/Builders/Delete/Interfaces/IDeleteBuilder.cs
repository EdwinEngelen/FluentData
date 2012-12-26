namespace FluentData
{
	public interface IDeleteBuilder : IExecute
	{
		IDeleteBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}