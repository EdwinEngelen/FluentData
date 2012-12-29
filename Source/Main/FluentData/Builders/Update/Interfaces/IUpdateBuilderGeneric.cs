using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IUpdateBuilder<T> : IExecute
	{
		BuilderData Data { get; }
		IUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IUpdateBuilder<T> Where(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilder<T> Where(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilder<T> Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IUpdateBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);
	}
}