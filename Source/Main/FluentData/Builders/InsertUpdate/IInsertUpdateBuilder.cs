namespace FluentData
{
	public interface IInsertUpdateBuilder
	{
		BuilderData Data { get; }
		IInsertUpdateBuilder Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}
