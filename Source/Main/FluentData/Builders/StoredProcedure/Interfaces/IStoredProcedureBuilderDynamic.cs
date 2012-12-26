using System;

namespace FluentData
{
    public interface IStoredProcedureBuilderDynamic : IExecute, IQuery, IParameterValue, IDisposable
	{
		IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties);
		IStoredProcedureBuilderDynamic Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0);
	}
}