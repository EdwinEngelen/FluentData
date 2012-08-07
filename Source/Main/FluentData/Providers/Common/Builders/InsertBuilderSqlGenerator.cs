namespace FluentData.Providers.Common.Builders
{
	internal class InsertBuilderSqlGenerator
	{
		public string GenerateSql(IDbProvider provider, string parameterPrefix, BuilderData data)
		{
			var insertSql = "";
			var valuesSql = "";
			foreach (var column in data.Columns)
			{
				if (insertSql.Length > 0)
				{
					insertSql += ",";
					valuesSql += ",";
				}

				insertSql += provider.EscapeColumnName(column.ColumnName);
				valuesSql += parameterPrefix + column.ParameterName;
			}

			var sql = string.Format("insert into {0}({1}) values({2})",
										data.ObjectName,
										insertSql,
										valuesSql);
			return sql;
		}
	}
}
