using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IStoredProcedureBuilderDynamic : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties);
		IStoredProcedureBuilderDynamic Parameter(string name, object value);
		IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0);
	}
}