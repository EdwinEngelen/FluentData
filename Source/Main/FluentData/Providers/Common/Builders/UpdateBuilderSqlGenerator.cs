namespace FluentData.Providers.Common.Builders
{
	internal class UpdateBuilderSqlGenerator
	{
		public string GenerateSql(string parameterPrefix, BuilderData data)
		{
			var setSql = "";
			foreach (var column in data.Columns)
			{
				if (setSql.Length > 0)
					setSql += ", ";

				setSql += string.Format("{0} = {1}{2}",
									column.ColumnName,
									parameterPrefix,
									column.ParameterName);
			}

			var whereSql = "";
			foreach (var column in data.Where)
			{
				if (whereSql.Length > 0)
					whereSql += " and ";

				whereSql += string.Format("{0} = {1}{2}",
									column.ColumnName,
									parameterPrefix,
									column.ParameterName);
			}

			var sql = string.Format("update {0} set {1} where {2}",
										data.ObjectName,
										setSql,
										whereSql);
			return sql;
		}
	}
}
