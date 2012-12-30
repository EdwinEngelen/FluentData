using System;
using System.Data;

namespace FluentData
{
	internal class QueryDataTableHandler<TEntity> : IQueryTypeHandler<TEntity>
	{
		private readonly DbCommandData _data;

		public bool IterateDataReader { get { return false; } }

		public QueryDataTableHandler(DbCommandData data)
		{
			_data = data;
		}

		public object HandleType(Action<TEntity, IDataReader> customMapperReader, Action<TEntity, dynamic> customMapperDynamic)
		{
			var dataTable = new DataTable();
			dataTable.Load(_data.Reader.InnerReader, LoadOption.OverwriteChanges);
			
			return dataTable;
		}
	}
}
