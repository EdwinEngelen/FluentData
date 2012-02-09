using System.Collections;
using System.Text;

namespace FluentData.Providers.Common
{
	internal class FixSqlInStatement
	{
		public void FixPotentialInSql(IDbProvider provider, StringBuilder sql, ParameterCollection parameters)
		{
			int i = -1;
			while (true)
			{
				i++;
				if (i == parameters.Count)
					break;

				var parameter = parameters[i];

				if (parameter.Direction == ParameterDirection.Output
					|| parameter.DataTypes != DataTypes.Object)
					continue;

				if (ReflectionHelper.IsList(parameter.Value))
				{
					var oldListParameterName = parameter.ParameterName;
					var list = (IEnumerable) parameter.Value;
					
					var newInStatement = new StringBuilder();

					int k = -1;
					foreach (var item in list)
					{
						k++;
						if (k == 0)
						{
							parameter.ParameterName = "p" + parameter.ParameterName + "p0";
							newInStatement.Append(" in(" + provider.GetParameterName(parameter.ParameterName));
							parameter.Value = item;
						}
						else
						{
							var newParameter = new Parameter();
							newParameter.ParameterName = "p" + oldListParameterName + "p" + k.ToString();
							newParameter.Value = item;
							newParameter.DataTypes = DataTypes.Object;
							newParameter.Direction = parameter.Direction;

							parameters.Insert(k, newParameter);

							newInStatement.Append("," + newParameter.GetParameterName(provider));
						}
					}
					newInStatement.Append(")");

					var oldInStatement = string.Format(" in({0})", provider.GetParameterName(oldListParameterName));
					sql.Replace(oldInStatement, newInStatement.ToString());
				}
			}
		}
	}
}
