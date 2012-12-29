using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IDeleteBuilder<T> : IExecute
	{
		BuilderData Data { get; }
		IDeleteBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);
		IDeleteBuilder<T> Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}