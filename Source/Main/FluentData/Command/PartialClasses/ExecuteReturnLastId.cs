using System;

namespace FluentData
{
	internal partial class DbCommand
	{
		public T ExecuteReturnLastId<T>(string identityColumnName = null)
		{
			if (Data.Context.Data.Provider.RequiresIdentityColumn && string.IsNullOrEmpty(identityColumnName))
				throw new FluentDataException("The identity column must be given");

			var value = Data.Context.Data.Provider.ExecuteReturnLastId<T>(this, identityColumnName);
			T lastId;

			if (value.GetType() == typeof(T))
				lastId = (T)value;
			else
				lastId = (T)Convert.ChangeType(value, typeof(T));

			return lastId;
		}
	}
}
