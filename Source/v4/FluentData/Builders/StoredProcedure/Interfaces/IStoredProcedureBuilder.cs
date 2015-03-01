using System;

namespace FluentData
{
    public interface IStoredProcedureBuilder : IExecute, IQuery, IParameterValue, IDisposable
	{
		BuilderData Data { get; }
		IStoredProcedureBuilder Parameter(string name, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0);
		IStoredProcedureBuilder UseMultiResult(bool useMultipleResultsets);
	}
}