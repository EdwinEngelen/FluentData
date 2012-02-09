using System;
using System.Collections.Generic;

namespace FluentData
{
	public interface IStoredProcedureBuilderDynamic : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilderDynamic IgnoreProperty(string name);
		IStoredProcedureBuilderDynamic AutoMap();
		IStoredProcedureBuilderDynamic Parameter(string name, object value);
		IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType);
	}
}