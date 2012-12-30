using System;
using System.Collections.Generic;

namespace FluentData
{
    public interface IQuery
    {
        List<TEntity> QueryMany<TEntity>(Action<TEntity, IDataReader> customMapper = null);
        List<TEntity> QueryMany<TEntity>(Action<TEntity, dynamic> customMapper);
        TList QueryMany<TEntity, TList>(Action<TEntity, IDataReader> customMapper = null) where TList : IList<TEntity>;
        TList QueryMany<TEntity, TList>(Action<TEntity, dynamic> customMapper) where TList : IList<TEntity>;
        void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, IDataReader> customMapper);
        void QueryComplexMany<TEntity>(IList<TEntity> list, Action<IList<TEntity>, dynamic> customMapper);
        TEntity QuerySingle<TEntity>(Action<TEntity, IDataReader> customMapper = null);
        TEntity QuerySingle<TEntity>(Action<TEntity, dynamic> customMapper);
        TEntity QueryComplexSingle<TEntity>(Func<IDataReader, TEntity> customMapper);
        TEntity QueryComplexSingle<TEntity>(Func<dynamic, TEntity> customMapper);
    }
}