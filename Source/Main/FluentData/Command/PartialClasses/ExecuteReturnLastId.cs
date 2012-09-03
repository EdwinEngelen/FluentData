namespace FluentData
{
	internal partial class DbCommand
	{
		public int ExecuteReturnLastId(string identityColumnName = null)
		{
			return ExecuteReturnLastId<int>(identityColumnName);
		}

		public T ExecuteReturnLastId<T>(string identityColumnName = null)
		{
			if (!_data.ContextData.Provider.SupportsExecuteReturnLastIdWithNoIdentityColumn && string.IsNullOrEmpty(identityColumnName))
				throw new FluentDataException("The selected database does not support this method.");

			var lastId = _data.ContextData.Provider.ExecuteReturnLastId<T>(_data, identityColumnName);

			return lastId;
		}

		
	}
}
