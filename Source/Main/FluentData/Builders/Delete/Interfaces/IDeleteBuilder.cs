namespace FluentData
{
	public interface IDeleteBuilder
	{
		int Execute();
		IDeleteBuilder Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}