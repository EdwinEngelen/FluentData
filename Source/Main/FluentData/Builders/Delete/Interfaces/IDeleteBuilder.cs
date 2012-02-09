namespace FluentData
{
	public interface IDeleteBuilder
	{
		int Execute();
		IDeleteBuilder Where(string columnName, object value);
	}
}