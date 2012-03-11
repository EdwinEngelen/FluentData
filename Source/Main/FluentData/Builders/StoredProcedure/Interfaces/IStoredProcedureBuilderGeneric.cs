using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IStoredProcedureBuilder<T> : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression);
		IStoredProcedureBuilder<T> Parameter(string name, object value);
		IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0);
	}
}