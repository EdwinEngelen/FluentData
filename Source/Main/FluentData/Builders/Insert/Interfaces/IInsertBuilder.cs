namespace FluentData
{
	public interface IInsertBuilder
	{
		int Execute();
		T ExecuteReturnLastId<T>(string identityColumnName = null);
		IInsertBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}