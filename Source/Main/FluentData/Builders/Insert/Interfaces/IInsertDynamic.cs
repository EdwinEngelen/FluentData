namespace FluentData
{
	public interface IInsertBuilderDynamic
	{
		int Execute();
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
		IInsertBuilderDynamic IgnoreProperty(string name);
		IInsertBuilderDynamic AutoMap();
		IInsertBuilderDynamic Column(string columnName, object value);
		IInsertBuilderDynamic Column(string propertyName);
	}
}