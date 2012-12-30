using System;
using System.Linq.Expressions;

namespace FluentData
{
	public interface IInsertBuilder<T> : IExecute, IExecuteReturnLastId
	{
		BuilderData Data { get; }
		T Item { get; }
		IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IInsertBuilder<T> Column(string columnName, object value, DataTypes parameterType = DataTypes.Object, int size = 0);
		IInsertBuilder<T> Column(Expression<Func<T, object>> expression, DataTypes parameterType = DataTypes.Object, int size = 0);
		IInsertBuilder<T> Fill(Action<IInsertUpdateBuilder<T>> fillMethod);
	}
}