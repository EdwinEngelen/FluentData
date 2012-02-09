using System.Text;
using FluentData;

namespace FluentData.Providers.Oracle.Builders
{
	internal class UpdateBuilderSqlGenerator
	{
		public string GenerateSql(BuilderData data)
		{
			var sql = new StringBuilder();

			sql.Append("update " + data.ObjectName);
			// Column.
			sql.Append(" set");
			for (int i = 0; i < data.Columns.Count; i++)
			{
				if (i > 0)
					sql.Append(",");

				var column = data.Columns[i];

				if (column.IsSql)
					sql.AppendFormat(@" {0} = ({1})", column.ColumnName, column.Value);
				else
					sql.AppendFormat(@" {0} = :{1}", column.ColumnName, column.ParameterName);

			}

			sql.Append(" where");

			for (int i = 0; i < data.Wheres.Count; i++)
			{
				if (i > 0)
					sql.Append(" and");

				var column = data.Wheres[i];

				sql.AppendFormat(" {0} = :{1}", column.ColumnName, column.ParameterName);
			}

			return sql.ToString();
		}
	}
}
