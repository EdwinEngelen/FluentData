namespace FluentData.Providers.Common.Builders
{
	internal class DeleteBuilderSqlGenerator
	{
		public string GenerateSql(string parameterPrefix, BuilderData data)
		{
			var whereSql = "";
			foreach (var column in data.Columns)
			{
				if (whereSql.Length > 0)
					whereSql += " and ";

				whereSql += string.Format("{0} = {1}{2}",
												column.ColumnName,
												parameterPrefix,
												column.ParameterName);
			}

			var sql = string.Format("delete from {0} where {1}", data.ObjectName, whereSql);
			return sql;
		}
	}
}
