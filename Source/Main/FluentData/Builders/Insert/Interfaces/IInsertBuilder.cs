namespace FluentData
{
	public interface IInsertBuilder
	{
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		/// <summary>
		/// Only needed for Oracle.
		/// </summary>
		/// <param name="identityColumnName">Name of the column with the auto/sequence number.</param>
		int ExecuteReturnLastId(string identityColumnName);
		/// <summary>
		/// Only needed for Oracle.
		/// </summary>
		/// <param name="identityColumnName">Name of the column with the auto/sequence number.</param>
		T ExecuteReturnLastId<T>(string identityColumnName);
		IInsertBuilder Column(string columnName, object value);
	}
}