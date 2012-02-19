namespace FluentData.Providers.Common.Builders
{
	internal class InsertBuilderSqlGenerator
	{
		public string GenerateSql(string parameterPrefix, BuilderData data)
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

				insertSql += column.ColumnName;
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
