namespace FluentData
{
	internal partial class DbCommand
	{
		public T ExecuteReturnLastId<T>(string identityColumnName = null)
		{
			if (!Data.Context.Data.Provider.SupportsExecuteReturnLastIdWithNoIdentityColumn && string.IsNullOrEmpty(identityColumnName))
				throw new FluentDataException("The selected database does not support this method.");

			var lastId = Data.Context.Data.Provider.ExecuteReturnLastId<T>(this, identityColumnName);

			return lastId;
		}
	}
}
