using System.Text;
using FluentData;

namespace FluentData.Providers.SqlServer.Builders
{
	internal class DeleteBuilderSqlGenerator
	{
		public string GenerateSql(BuilderData data)
		{
			var sql = new StringBuilder();

			sql.Append("delete from " + data.ObjectName);

			sql.Append(" where");

			for (int i = 0; i < data.Columns.Count; i++)
			{
				if (i > 0)
					sql.Append(" and");

				var column = data.Columns[i];

				sql.AppendFormat(" {0} = @{1}", column.ColumnName, column.ParameterName);
			}

			sql.Append(";");

			return sql.ToString();
		}
	}
}
