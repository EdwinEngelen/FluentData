using System.Data;
using System.Data.Common;

namespace FluentData.Providers.Oracle
{
	internal class OracleQueryExecuter
	{
		internal T ExecuteReturnLastId<T>(OracleProvider provider, DbCommandData data, string identityColumnName = null)
		{
			var lastInsertedParameterName = provider.GetParameterName(GlobalConstants.LastInsertedIdParameterName);
			bool found = false;

			foreach (DbParameter parameter in data.InnerCommand.Parameters)
			{
				if (parameter.ParameterName == lastInsertedParameterName)
					found = true;
			}

			if (!found)
			{
				data.DbCommand.ParameterOut(GlobalConstants.LastInsertedIdParameterName, data.DbContextData.DbProvider.GetDbTypeForClrType(typeof(T)));
				data.Sql.Append(string.Format(" returning {0} into :LastInsertedId", identityColumnName));
			}

			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				data.InnerCommand.ExecuteNonQuery();

				var parameter = (IDbDataParameter) data.InnerCommand.Parameters[":" + GlobalConstants.LastInsertedIdParameterName];
				lastId = (T) parameter.Value;
			});

			return lastId;
		}
	}
}
