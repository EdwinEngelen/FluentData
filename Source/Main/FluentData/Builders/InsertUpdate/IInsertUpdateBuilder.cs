namespace FluentData
{
	public interface IInsertUpdateBuilder
	{
		IInsertUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}
