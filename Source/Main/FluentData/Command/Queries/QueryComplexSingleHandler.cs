//using System;

//namespace FluentData
//{
//	internal class QueryComplexSingleHandler<TEntity>
//	{
//		internal TEntity ExecuteSingleComplex(DbCommandData data,
//			Func<IDataReader, TEntity> customMapperReader,
//			Func<dynamic, TEntity> customMapperDynamic)
//		{
//			var item = default(TEntity);
//			var reader = new DynamicDataReader(data.Reader);

//			if (reader. data.Reader.Read())
//			{
//				if (customMapperReader != null)
//					item = customMapperReader(reader);
//				else
//					item = customMapperDynamic(data.Reader);
//			}
//			return item;
//		}
//	}
//}
