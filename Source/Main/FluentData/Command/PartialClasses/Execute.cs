namespace FluentData
{
	internal partial class DbCommand
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns>Numbers of records affected.</returns>
		public int Execute()
		{
			int recordsAffected = 0;

			_data.QueryExecuter.ExecuteQueryHandler(false, () =>
			{
				recordsAffected = new ExecuteHandler(_data).Execute<int>();
			});
			return recordsAffected;
		}

		public T ExecuteReturnLastId<T>()
		{
			return ExecuteReturnLastId<T>(null);
		}

		public T ExecuteReturnLastId<T>(string identityColumnName)
		{
			T lastId = _data.DbContextData.DbProvider.ExecuteReturnLastId<T>(_data, identityColumnName);

			return lastId;
		}

		public int ExecuteReturnLastId()
		{
			return ExecuteReturnLastId(null);
		}

		public int ExecuteReturnLastId(string identityColumnName)
		{
			return ExecuteReturnLastId<int>(identityColumnName);
		}
	}
}
