using System;

namespace FluentData
{
	public interface IStoredProcedureBuilder : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilder Parameter(string name, object value);
		IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType);
	}
}