using System.Text;
using FluentData;

namespace FluentData.Providers.Oracle.Builders
{
	internal class InsertBuilderSqlGenerator
	{
		public string GenerateSql(BuilderData data)
		{
			var sql = new StringBuilder();
			var valuesSql = new StringBuilder();

			sql.AppendFormat("insert into {0}(", data.ObjectName);
			valuesSql.AppendFormat(" values(");

			for (int i = 0; i < data.Columns.Count; i++)
			{
				var column = data.Columns[i];

				if (i > 0)
				{
					sql.Append(",");
					valuesSql.Append(",");
				}

				sql.Append(column.ColumnName);

				if (column.IsSql)
					valuesSql.Append("(" + column.Value + ")");
				else
					valuesSql.Append(":" + column.ParameterName);
			}

			sql.Append(")");
			valuesSql.Append(")");

			sql.Append(valuesSql);

			return sql.ToString();
		}
	}
}
