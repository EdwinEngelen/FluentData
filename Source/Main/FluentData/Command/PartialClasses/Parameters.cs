using System.Linq;

namespace FluentData
{
	internal partial class DbCommand
	{
		public IDbCommand Parameters(params object[] parameters)
		{
			for (int i = 0; i < parameters.Count(); i++)
				Parameter(i.ToString(), parameters[i]);
			return this;
		}

		public IDbCommand Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction)
		{
			var parameter = new Parameter();
			parameter.DataTypes = parameterType;
			parameter.ParameterName = name;
			parameter.Direction = direction;
			parameter.Value = value;
			_data.Parameters.Add(parameter);
			return this;
		}

		public IDbCommand Parameter(string name, object value)
		{
			Parameter(name, value, DataTypes.Object, ParameterDirection.Input);
			return this;
		}

		public IDbCommand ParameterOut(string name, DataTypes parameterType)
		{
			if (!_data.DbContextData.DbProvider.SupportsOutputParameters)
				throw new FluentDbException("The selected database does not support output parameters");
			Parameter(name, null, parameterType, ParameterDirection.Output);
			return this;
		}

		public TParameterType ParameterValue<TParameterType>(string outputParameterName)
		{
			outputParameterName = _data.DbContextData.DbProvider.GetParameterName(outputParameterName);
			var parameter = _data.Parameters.SingleOrDefault(x => x.ParameterName == outputParameterName);
			var value = parameter.Value;
			if (value == null)
				return default(TParameterType);

			return (TParameterType) value;
		}
	}
}
