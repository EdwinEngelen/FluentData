using System;
using System.Linq.Expressions;
using System.Text;

namespace FluentData
{
	internal partial class DbCommand
	{
		public IDbCommand Sql(string sql)
		{
			_data.Sql.Append(sql);
			return this;
		}

		public IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			if (mappingExpressions == null)
				Sql(sql);
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
	}
}
