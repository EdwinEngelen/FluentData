//using System;
//using System.Dynamic;

//namespace FluentData
//{
//	internal class QuerySingleHandler<TEntity>
//	{
//		internal TEntity ExecuteSingle(DbCommandData data,
//												Action<TEntity, IDataReader> customMapperReader,
//												Action<TEntity, dynamic> customMapperDynamic)
//		{
//			if (typeof (TEntity) == typeof (object))
//			{
//				var autoMapper = new DynamicTypAutoMapper(data);

//				ExpandoObject item = null;

//				if (data.Reader.Read())
//					item = autoMapper.AutoMap();

//				return (dynamic) item;
//			}
//			else
//			{
//				var item = default(TEntity);

//				if (ReflectionHelper.IsCustomEntity<TEntity>())
//				{
//					AutoMapper autoMapper = null;

//					if (customMapperReader == null && customMapperDynamic == null)
//						autoMapper = new AutoMapper(data, typeof (TEntity));

//					if (data.Reader.Read())
//					{
//						item = (TEntity) data.Context.Data.EntityFactory.Create(typeof (TEntity));

//						if (customMapperReader != null)
//							customMapperReader(item, data.Reader);
//						else if (customMapperDynamic != null)
//							customMapperDynamic(item, new DynamicDataReader(data.Reader));
//						else
//							autoMapper.AutoMap(item);
//					}
//				}
//				else
//				{
//					if (data.Reader.Read())
//					{
//						if (data.Reader.IsDBNull(0))
//							return item;

//						if (data.Reader.GetFieldType(0) == typeof (TEntity))
//							item = (TEntity) data.Reader.GetValue(0);
//						else
//							item = (TEntity) Convert.ChangeType(data.Reader.GetValue(0), typeof (TEntity));
//					}
//				}

//				return item;
//			}
//		}
//	}
//}
