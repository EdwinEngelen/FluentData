namespace FluentData
{
	public interface IInsertUpdateBuilder
	{
		IInsertUpdateBuilder Column(string columnName, object value);
	}
}
