using System.Collections;
using System.Data;
using System;
using System.Text;

namespace FluentData
{
	internal partial class DbCommand
	{
		public IDbCommand Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0)
		{
			if (ReflectionHelper.IsList(value))
				AddListParameterToInnerCommand(name, value);
			else
				AddParameterToInnerCommand(name, value, parameterType, direction, size);

			return this;
		}

		private void AddListParameterToInnerCommand(string name, object value)
		{
			var list = (IEnumerable) value;

			var newInStatement = new StringBuilder();

			var k = -1;
			foreach (var item in list)
			{
				k++;
				if (k == 0)
					newInStatement.Append(" in(");
				else
					newInStatement.Append(",");
				
				var parameter = AddParameterToInnerCommand("p" + name + "p" + k.ToString(), item);

				newInStatement.Append(parameter.ParameterName);
			}
			newInStatement.Append(")");

			var oldInStatement = string.Format(" in({0})", Data.Context.Data.Provider.GetParameterName(name));
			Data.InnerCommand.CommandText = Data.InnerCommand.CommandText.Replace(oldInStatement, newInStatement.ToString());
		}

		private IDbDataParameter AddParameterToInnerCommand(string name, object value, DataTypes parameterType = DataTypes.Object, ParameterDirection direction = ParameterDirection.Input, int size = 0)
		{
			if (value == null)
				value = DBNull.Value;

			if (value.GetType().IsEnum)
				value = (int) value;

			var dbParameter = Data.InnerCommand.CreateParameter();
			if (parameterType == DataTypes.Object)
				dbParameter.DbType = (System.Data.DbType) Data.Context.Data.Provider.GetDbTypeForClrType(value.GetType());
			else
				dbParameter.DbType = (System.Data.DbType) parameterType;

			dbParameter.ParameterName = Data.Context.Data.Provider.GetParameterName(name);
			dbParameter.Direction = (System.Data.ParameterDirection) direction;
			dbParameter.Value = value;
			if (size > 0)
				dbParameter.Size = size;
			Data.InnerCommand.Parameters.Add(dbParameter);

			return dbParameter;
		}

		public IDbCommand ParameterOut(string name, DataTypes parameterType, int size)
		{
			if (!Data.Context.Data.Provider.SupportsOutputParameters)
				throw new FluentDataException("The selected database does not support output parameters");
			Parameter(name, null, parameterType, ParameterDirection.Output, size);
			return this;
		}

		public TParameterType ParameterValue<TParameterType>(string outputParameterName)
		{
			outputParameterName = Data.Context.Data.Provider.GetParameterName(outputParameterName);
			if (!Data.InnerCommand.Parameters.Contains(outputParameterName))
				throw new FluentDataException(string.Format("Parameter {0} not found", outputParameterName));

			var value = (Data.InnerCommand.Parameters[outputParameterName] as System.Data.IDataParameter).Value;

			if (value == null)
				return default(TParameterType);

			if (value.GetType() == typeof(TParameterType))
				return (TParameterType) value;

			return (TParameterType) Convert.ChangeType(value, typeof(TParameterType));
		}
	}
}
