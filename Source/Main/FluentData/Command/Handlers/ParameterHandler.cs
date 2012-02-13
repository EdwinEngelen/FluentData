using System;

namespace FluentData
{
	internal class ParameterHandler
	{
		internal void FixParameterType(DbCommandData data)
		{
			foreach (var parameter in data.Parameters)
			{
				if (parameter.Direction == ParameterDirection.Input
					&& parameter.DataTypes == DataTypes.Object)
				{
					if (parameter.Value == null)
						parameter.Value = DBNull.Value;
					else
					{
						parameter.DataTypes = data.DbContextData.DbProvider.GetDbTypeForClrType(parameter.Value.GetType());
						if (parameter.DataTypes == DataTypes.Object)
							throw new FluentDataException(string.Format("The parameter {0} is off a type that is not supported.", parameter.ParameterName));
					}
				}

				var dbParameter = data.InnerCommand.CreateParameter();
				dbParameter.DbType = (System.Data.DbType) parameter.DataTypes;
				dbParameter.ParameterName = data.DbContextData.DbProvider.GetParameterName(parameter.ParameterName);
				dbParameter.Direction = (System.Data.ParameterDirection) parameter.Direction;
				dbParameter.Value = parameter.Value;
				if (parameter.Size > 0)
					dbParameter.Size = parameter.Size;
				data.InnerCommand.Parameters.Add(dbParameter);
			}
		}
	}
}
