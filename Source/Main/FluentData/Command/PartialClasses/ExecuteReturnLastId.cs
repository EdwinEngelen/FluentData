namespace FluentData
{
	internal partial class DbCommand
	{
		public int ExecuteReturnLastId()
		{
			return ExecuteReturnLastId<int>();
		}

		public T ExecuteReturnLastId<T>()
		{
			if (!_data.ContextData.Provider.SupportsExecuteReturnLastIdWithNoIdentityColumn)
				throw new FluentDataException("The selected database does not support this method.");

			T lastId = _data.ContextData.Provider.ExecuteReturnLastId<T>(_data, null);

			return lastId;
		}

		public int ExecuteReturnLastId(string identityColumnName)
		{
			return ExecuteReturnLastId<int>(identityColumnName);
		}

		public T ExecuteReturnLastId<T>(string identityColumnName)
		{
			if (_data.ContextData.Provider.SupportsExecuteReturnLastIdWithNoIdentityColumn)
				throw new FluentDataException("The selected database does not support this method.");

			T lastId = _data.ContextData.Provider.ExecuteReturnLastId<T>(_data, identityColumnName);

			return lastId;
		}

		
	}
}
