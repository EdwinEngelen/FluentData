using System;
using System.Linq.Expressions;
using System.Text;

namespace FluentData
{
	internal partial class DbCommand
	{
		public IDbCommand Sql(string sql)
		{
			if (_data.Sql == null)
				_data.Sql = new StringBuilder();
			_data.Sql.Append(sql);
			return this;
		}

		public IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			if (_data.Sql == null)
				_data.Sql = new StringBuilder();

			if (mappingExpressions == null)
			{
				_data.Sql.Append(sql);
			}
			else
			{
				var propertyNames = ReflectionHelper.GetPropertyNamesFromExpressions(mappingExpressions);
				for (int i = 0; i < propertyNames.Count; i++)
				{
					propertyNames[i] = propertyNames[i].Replace('.', '_');
				}

				_data.Sql.AppendFormat(sql, propertyNames.ToArray());
			}
			return this;
		}

		public string GetSql()
		{
			return _data.Sql.ToString();
		}
	}
}
