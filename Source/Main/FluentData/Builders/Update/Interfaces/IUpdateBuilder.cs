namespace FluentData
{
	public interface IUpdateBuilder
	{
		int Execute();
		IUpdateBuilder Column(string columnName, object value);
		IUpdateBuilder Where(string columnName, object value);
	}
}