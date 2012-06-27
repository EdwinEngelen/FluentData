
// FluentData version 2.2.2.0.
// Copyright ©  2012 - Lars-Erik Kindblad (http://www.kindblad.com).
// See http://fluentdata.codeplex.com for more information and licensing terms.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace FluentData
{
	internal class ActionsHandler
	{
		private readonly BuilderData _data;

		internal ActionsHandler(BuilderData data)
		{
			_data = data;
		}

		internal void ColumnValueAction(string columnName, object value, bool propertyNameIsParameterName)
		{
			ColumnAction(columnName, value, typeof(object), propertyNameIsParameterName);
		}

		private void ColumnAction(string columnName, object value, Type type, bool propertyNameIsParameterName)
		{
			var parameterName = "";
			if (propertyNameIsParameterName)
				parameterName = columnName;
			else
				parameterName = "c" + _data.Columns.Count.ToString();

			if (value != null)
			{
				if (value.GetType().IsEnum)
					value = (int) value;
			}

			_data.Columns.Add(new TableColumn(columnName, value, parameterName));

			var parameterType = DataTypes.Object;
			if (type != (typeof(object)))
			{
				parameterType = _data.Provider.GetDbTypeForClrType(type);
			}

			ParameterAction(parameterName, value, parameterType, ParameterDirection.Input, false);
		}

		internal void ColumnValueAction<T>(Expression<Func<T, object>> expression, bool propertyNameIsParameterName)
		{
			var parser = new PropertyExpressionParser<T>(_data.Item, expression);

			ColumnAction(parser.Name, parser.Value, parser.Type, propertyNameIsParameterName);
		}

		internal void ColumnValueDynamic(ExpandoObject item, string propertyName)
		{
			var propertyValue = (item as IDictionary<string, object>) [propertyName];

			ColumnAction(propertyName, propertyValue, typeof(object), true);
		}

		internal void AutoMapColumnsAction<T>(bool propertyNameIsParameterName, params Expression<Func<T, object>>[] ignorePropertyExpressions)
		{
			var properties = ReflectionHelper.GetProperties(_data.Item.GetType());
			var ignorePropertyNames = new HashSet<string>();
			if (ignorePropertyExpressions != null)
			{
				foreach (var ignorePropertyExpression in ignorePropertyExpressions)
				{
					var ignorePropertyName = new PropertyExpressionParser<T>(_data.Item, ignorePropertyExpression).Name;
					ignorePropertyNames.Add(ignorePropertyName);
				}
			}

			foreach (var property in properties)
			{

				var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Value.Name, StringComparison.CurrentCultureIgnoreCase));
				if (ignoreProperty != null)
					continue;

				var propertyType = ReflectionHelper.GetPropertyType(property.Value);

				if (ReflectionHelper.IsBasicClrType(propertyType))
				{
					var propertyValue = ReflectionHelper.GetPropertyValue(_data.Item, property.Value);
					ColumnAction(property.Value.Name, propertyValue, propertyType, propertyNameIsParameterName);
				}
			}
		}

		internal void AutoMapDynamicTypeColumnsAction(bool propertyNameIsParameterName, params string[] ignorePropertyExpressions)
		{
			var properties = (IDictionary<string, object>) _data.Item;
			var ignorePropertyNames = new HashSet<string>();
			if (ignorePropertyExpressions != null)
			{
				foreach (var ignorePropertyExpression in ignorePropertyExpressions)
					ignorePropertyNames.Add(ignorePropertyExpression);
			}

			foreach (var property in properties)
			{
				var ignoreProperty = ignorePropertyNames.SingleOrDefault(x => x.Equals(property.Key, StringComparison.CurrentCultureIgnoreCase));

				if (ignoreProperty == null
					&& ReflectionHelper.IsBasicClrType(property.Value.GetType()))
				{
					ColumnAction(property.Key, property.Value, typeof(object), propertyNameIsParameterName);
				}
			}
		}

		internal void ParameterAction(string name, object value, DataTypes dataType, ParameterDirection direction, bool isId, int size = 0)
		{
			var parameter = new Parameter();
			parameter.ParameterName = name;
			parameter.Value = value;
			parameter.DataTypes = dataType;
			parameter.Direction = direction;
			parameter.IsId = isId;
			parameter.Size = size;

			_data.Parameters.Add(parameter);
			_data.Command.Parameter(parameter.ParameterName, parameter.Value, parameter.DataTypes, parameter.Direction, parameter.Size);
		}

		internal void ParameterOutputAction(string name, DataTypes dataTypes, int size)
		{
			ParameterAction(name, null, dataTypes, ParameterDirection.Output, false, size);
		}

		internal void ParametersAction(object[] parameters)
		{
			var count = parameters.Count();

			for (int i = 0; i < count; i++)
				ParameterAction(i.ToString(), parameters[i], DataTypes.Object, ParameterDirection.Input, false);
		}

		internal void WhereAction(string columnName, object value)
		{
			var parameterName = "id" + _data.Where.Count().ToString();
			ParameterAction(parameterName, value, DataTypes.Object, ParameterDirection.Input, true);

			_data.Where.Add(new TableColumn(columnName, value, parameterName));
		}

		internal void WhereAction<T>(Expression<Func<T, object>> expression)
		{
			var parser = new PropertyExpressionParser<T>(_data.Item, expression);
			WhereAction(parser.Name, parser.Value);
		}
	}

	public class BuilderData
	{
		public int PagingCurrentPage { get; set; }
		public int PagingItemsPerPage { get; set; }
		public List<TableColumn> Columns { get; set; }
		public List<Parameter> Parameters { get; set; }
		public object Item { get; set; }
		public string ObjectName { get; set; }
		public DbCommandData CommandData { get; set; }
		public IDbProvider Provider { get; set; }
		public IDbCommand Command { get; set; }
		public List<TableColumn> Where { get; set; }
		public string Having { get; set; }
		public string GroupBy { get; set; }
		public string OrderBy { get; set; }
		public string From { get; set; }
		public string Select { get; set; }
		public string WhereSql { get; set; }

		public BuilderData(IDbProvider provider, IDbCommand command, string objectName)
		{
			Provider = provider;
			ObjectName = objectName;
			Command = command;
			Parameters = new List<Parameter>();
			Columns = new List<TableColumn>();
			Where = new List<TableColumn>();
			Having = "";
			GroupBy = "";
			OrderBy = "";
			From = "";
			Select = "";
			WhereSql = "";
			PagingCurrentPage = 1;
			PagingItemsPerPage = 0;
		}

		internal int GetFromItems()
		{
			return (GetToItems() - PagingItemsPerPage + 1);
		}

		internal int GetToItems()
		{
			return (PagingCurrentPage*PagingItemsPerPage);
		}
	}

	internal abstract class BaseDeleteBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.Command.Sql(Data.Provider.GetSqlForDeleteBuilder(Data));
				return Data.Command;
			}
		}

		public BaseDeleteBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}
	}

	internal class DeleteBuilder : BaseDeleteBuilder, IDeleteBuilder
	{
		public DeleteBuilder(IDbProvider provider, IDbCommand command, string tableName)
			: base(provider, command, tableName)
		{
		}

		public IDeleteBuilder Where(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}
	}

	internal class DeleteBuilder<T> : BaseDeleteBuilder, IDeleteBuilder<T>
	{
		public DeleteBuilder(IDbProvider provider, IDbCommand command, string tableName, T item)
			: base(provider, command, tableName)
		{
			Data.Item = item;
		}
		public IDeleteBuilder<T> Where(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}

		public IDeleteBuilder<T> Where(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}
	}

	public interface IDeleteBuilder
	{
		int Execute();
		IDeleteBuilder Where(string columnName, object value);
	}

	public interface IDeleteBuilder<T>
	{
		int Execute();
		IDeleteBuilder<T> Where(Expression<Func<T, object>> expression);
		IDeleteBuilder<T> Where(string columnName, object value);
	}

	public interface IInsertUpdateBuilder
	{
		IInsertUpdateBuilder Column(string columnName, object value);
	}

	public interface IInsertUpdateBuilderDynamic
	{
		IInsertUpdateBuilderDynamic AutoMap(params string[] ignoreProperties);
		IInsertUpdateBuilderDynamic Column(string columnName, object value);
		IInsertUpdateBuilderDynamic Column(string propertyName);
	}

	public interface IInsertUpdateBuilder<T>
	{
		IInsertUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IInsertUpdateBuilder<T> Column(string columnName, object value);
		IInsertUpdateBuilder<T> Column(Expression<Func<T, object>> expression);
	}

	internal abstract class BaseInsertBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.Command.Sql(Data.Provider.GetSqlForInsertBuilder(Data));
				return Data.Command;
			}
		}

		public BaseInsertBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}

		public int ExecuteReturnLastId()
		{
			return Command.ExecuteReturnLastId();
		}

		public T ExecuteReturnLastId<T>()
		{
			return Command.ExecuteReturnLastId<T>();
		}

		public int ExecuteReturnLastId(string identityColumnName)
		{
			return Command.ExecuteReturnLastId(identityColumnName);
		}

		public T ExecuteReturnLastId<T>(string identityColumnName)
		{
			return Command.ExecuteReturnLastId<T>(identityColumnName);
		}
	}

	internal class InsertBuilder : BaseInsertBuilder, IInsertBuilder, IInsertUpdateBuilder
	{
		internal InsertBuilder(IDbProvider provider, IDbCommand command, string name)
			: base(provider, command, name)
		{
		}

		public IInsertBuilder Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilder IInsertUpdateBuilder.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}
	}

	internal class InsertBuilderDynamic : BaseInsertBuilder, IInsertBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		internal InsertBuilderDynamic(IDbProvider provider, IDbCommand command, string name, ExpandoObject item)
			: base(provider, command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
		}

		public IInsertBuilderDynamic Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		public IInsertBuilderDynamic Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}

		public IInsertBuilderDynamic AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}
	}

	internal class InsertBuilder<T> : BaseInsertBuilder, IInsertBuilder<T>, IInsertUpdateBuilder<T>
	{
		internal InsertBuilder(IDbProvider provider, IDbCommand command, string name, T item)
			: base(provider, command, name)
		{
			Data.Item = item;
		}

		public IInsertBuilder<T> Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		public IInsertBuilder<T> Column(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}

		public IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}
	}

	public interface IInsertBuilder
	{
		int Execute();
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		int ExecuteReturnLastId(string identityColumnName);
		T ExecuteReturnLastId<T>(string identityColumnName);
		IInsertBuilder Column(string columnName, object value);
	}

	public interface IInsertBuilder<T>
	{
		int Execute();
		int ExecuteReturnLastId();
		TReturn ExecuteReturnLastId<TReturn>();
		int ExecuteReturnLastId(string identityColumnName);
		TReturn ExecuteReturnLastId<TReturn>(string identityColumnName);
		IInsertBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IInsertBuilder<T> Column(string columnName, object value);
		IInsertBuilder<T> Column(Expression<Func<T, object>> expression);
	}

	public interface IInsertBuilderDynamic
	{
		int Execute();
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		int ExecuteReturnLastId(string identityColumnName);
		T ExecuteReturnLastId<T>(string identityColumnName);
		IInsertBuilderDynamic AutoMap(params string[] ignoreProperties);
		IInsertBuilderDynamic Column(string columnName, object value);
		IInsertBuilderDynamic Column(string propertyName);
	}

	public interface ISelectBuilder<TEntity>
	{
		ISelectBuilder<TEntity> Select(string sql);
		ISelectBuilder<TEntity> Select(string sql, Expression<Func<TEntity, object>> mapToProperty);
		ISelectBuilder<TEntity> From(string sql);
		ISelectBuilder<TEntity> Where(string sql);
		ISelectBuilder<TEntity> GroupBy(string sql);
		ISelectBuilder<TEntity> OrderBy(string sql);
		ISelectBuilder<TEntity> Having(string sql);
		ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage);

		ISelectBuilder<TEntity> Parameter(string name, object value);
		ISelectBuilder<TEntity> Parameters(params object[] parameters);

		TList Query<TList>() where TList : IList<TEntity>;
		TList Query<TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList Query<TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query();
		List<TEntity> Query(Action<dynamic, TEntity> customMapper);
		List<TEntity> Query(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList QueryNoAutoMap<TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap(Func<dynamic, TEntity> customMapper);
		List<TEntity> QueryNoAutoMap(Func<IDataReader, TEntity> customMapper);
		TEntity QuerySingle();
		TEntity QuerySingle(Action<IDataReader, TEntity> customMapper);
		TEntity QuerySingle(Action<dynamic, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap(Func<IDataReader, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap(Func<dynamic, TEntity> customMapper);
		TValue QueryValue<TValue>();
	}

	internal class SelectBuilder<TEntity> : ISelectBuilder<TEntity>
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				if (Data.PagingItemsPerPage > 0
					&& string.IsNullOrEmpty(Data.OrderBy))
					throw new FluentDataException("Order by must defined when using Paging.");

				Data.Command.Sql(Data.Provider.GetSqlForSelectBuilder(Data));
				return Data.Command;
			}
		}

		public SelectBuilder(IDbProvider provider, IDbCommand command)
		{
			Data =  new BuilderData(provider, command, "");
			Actions = new ActionsHandler(Data);
		}

		public ISelectBuilder<TEntity> Select(string sql)
		{
			Data.Select += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Select(string sql, Expression<Func<TEntity, object>> mapToProperty)
		{
			var alias = Data.Provider.GetSelectBuilderAlias(sql, ReflectionHelper.GetPropertyNameFromExpression(mapToProperty).Replace(".", "_"));
			if (Data.Select.Length > 0)
				Data.Select += ",";

			Data.Select += alias;
			return this;
		}

		public ISelectBuilder<TEntity> From(string sql)
		{
			Data.From += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Where(string sql)
		{
			Data.WhereSql += sql;
			return this;
		}

		public ISelectBuilder<TEntity> OrderBy(string sql)
		{
			Data.OrderBy += sql;
			return this;
		}

		public ISelectBuilder<TEntity> GroupBy(string sql)
		{
			Data.GroupBy += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Having(string sql)
		{
			Data.Having += sql;
			return this;
		}

		public ISelectBuilder<TEntity> Paging(int currentPage, int itemsPerPage)
		{
			Data.PagingCurrentPage = currentPage;
			Data.PagingItemsPerPage = itemsPerPage;
			return this;
		}

		public ISelectBuilder<TEntity> Parameter(string name, object value)
		{
			Data.Command.Parameter(name, value);
			return this;
		}

		public ISelectBuilder<TEntity> Parameters(params object[] parameters)
		{
			Data.Command.Parameters(parameters);
			return this;
		}

		public TList Query<TList>() where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>();
		}

		public TList Query<TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public TList Query<TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public List<TEntity> Query()
		{
			return Command.Query<TEntity>();
		}

		public List<TEntity> Query(Action<dynamic, TEntity> customMapper)
		{
			return Command.Query(customMapper);
		}

		public List<TEntity> Query(Action<IDataReader, TEntity> customMapper)
		{
			return Command.Query(customMapper);
		}

		public TList QueryComplex<TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryComplex<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryComplex(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return Command.QueryComplex(customMapper);
		}

		public TList QueryNoAutoMap<TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public TList QueryNoAutoMap<TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryNoAutoMap(Func<dynamic, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap(customMapper);
		}

		public List<TEntity> QueryNoAutoMap(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap(customMapper);
		}

		public TEntity QuerySingle()
		{
			return Command.QuerySingle<TEntity>();
		}

		public TEntity QuerySingle(Action<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingle(customMapper);
		}

		public TEntity QuerySingle(Action<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingle(customMapper);
		}

		public TEntity QuerySingleNoAutoMap(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public TEntity QuerySingleNoAutoMap(Func<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public TValue QueryValue<TValue>()
		{
			return Command.QueryValue<TValue>();

		}
	}

	internal abstract class BaseStoredProcedureBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				Data.Command.CommandType(DbCommandTypes.StoredProcedure);
				Data.Command.Sql(Data.Provider.GetSqlForStoredProcedureBuilder(Data));
				return Data.Command;
			}
		}

		public BaseStoredProcedureBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
			Actions = new ActionsHandler(Data);
		}


		public void Dispose()
		{
			Command.Dispose();
		}

		public TParameterType ParameterValue<TParameterType>(string outputParameterName)
		{
			return Command.ParameterValue<TParameterType>(outputParameterName);
		}

		public int Execute()
		{
			return Command.Execute();
		}

		public List<dynamic> Query()
		{
			return Command.Query();
		}

		public TList Query<TEntity, TList>() where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>();
		}

		public TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.Query<TEntity, TList>(customMapper);
		}

		public List<TEntity> Query<TEntity>()
		{
			return Command.Query<TEntity>();
		}

		public List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Command.Query<TEntity>(customMapper);
		}

		public List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Command.Query<TEntity>(customMapper);
		}

		public TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryComplex<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return Command.QueryComplex<TEntity>(customMapper);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			return Command.QueryNoAutoMap<TEntity, TList>(customMapper);
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap<TEntity>(customMapper);
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QueryNoAutoMap<TEntity>(customMapper);
		}

		public dynamic QuerySingle()
		{
			return Command.QuerySingle();
		}

		public TEntity QuerySingle<TEntity>()
		{
			return Command.QuerySingle<TEntity>();
		}

		public TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QuerySingle<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingle<TEntity>(customMapper);
		}

		public TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			return Command.QuerySingleNoAutoMap(customMapper);
		}

		public T QueryValue<T>()
		{
			return Command.QueryValue<T>();
		}

		public List<T> QueryValues<T>()
		{
			return Command.QueryValues<T>();
		}
	}

	public interface IBaseStoredProcedureBuilder
	{
		TValue ParameterValue<TValue>(string name);
		int Execute();
		List<dynamic> Query();
		TList Query<TEntity, TList>() where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>();
		List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper);
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TEntity, TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper);
		List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper);
		TEntity QuerySingle<TEntity>(Action<dynamic, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper);
		TValue QueryValue<TValue>();
	}

	public interface IStoredProcedureBuilder : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilder Parameter(string name, object value);
		IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0);
	}

	public interface IStoredProcedureBuilderDynamic : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties);
		IStoredProcedureBuilderDynamic Parameter(string name, object value);
		IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0);
	}

	public interface IStoredProcedureBuilder<T> : IBaseStoredProcedureBuilder, IDisposable
	{
		IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression);
		IStoredProcedureBuilder<T> Parameter(string name, object value);
		IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0);
	}

	internal class StoredProcedureBuilder : BaseStoredProcedureBuilder, IStoredProcedureBuilder
	{
		internal StoredProcedureBuilder(IDbProvider dbProvider, IDbCommand command, string name)
			: base(dbProvider, command, name)
		{
		}

		public IStoredProcedureBuilder Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value, true);
			return this;
		}

		public IStoredProcedureBuilder ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}
	}	

	internal class StoredProcedureBuilderDynamic : BaseStoredProcedureBuilder, IStoredProcedureBuilderDynamic
	{
		internal StoredProcedureBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
			: base(dbProvider, command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
		}

		public IStoredProcedureBuilderDynamic Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value, true);
			return this;
		}

		public IStoredProcedureBuilderDynamic AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(true, ignoreProperties);
			return this;
		}

		public IStoredProcedureBuilderDynamic ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}
	}

	internal class StoredProcedureBuilder<T> : BaseStoredProcedureBuilder, IStoredProcedureBuilder<T>
	{
		internal StoredProcedureBuilder(IDbProvider dbProvider, IDbCommand command, string name, T item)
			: base(dbProvider, command, name)
		{
			Data.Item = item;
		}

		public IStoredProcedureBuilder<T> Parameter(string name, object value)
		{
			Actions.ColumnValueAction(name, value, true);
			return this;
		}

		public IStoredProcedureBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(true, ignoreProperties);
			return this;
		}

		public IStoredProcedureBuilder<T> Parameter(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression, true);

			return this;
		}

		public IStoredProcedureBuilder<T> ParameterOut(string name, DataTypes parameterType, int size = 0)
		{
			Actions.ParameterOutputAction(name, parameterType, size);
			return this;
		}
	}

	public class TableColumn
	{
		public string ColumnName { get; set; }
		public string ParameterName { get; set; }
		public object Value { get; set; }

		public TableColumn(string columnName, object value, string parameterName)
		{
			ColumnName = columnName;
			Value = value;
			ParameterName = parameterName;
		}
	}

	internal abstract class BaseUpdateBuilder
	{
		protected BuilderData Data { get; set; }
		protected ActionsHandler Actions { get; set; }

		private IDbCommand Command
		{
			get
			{
				if (Data.Columns.Count == 0
					|| Data.Where.Count == 0)
					throw new FluentDataException("Columns or where filter have not yet been added.");

				Data.Command.Sql(Data.Provider.GetSqlForUpdateBuilder(Data));
				return Data.Command;
			}
		}

		public BaseUpdateBuilder(IDbProvider provider, IDbCommand command, string name)
		{
			Data =  new BuilderData(provider, command, name);
			Actions = new ActionsHandler(Data);
		}

		public int Execute()
		{
			return Command.Execute();
		}
	}

	public interface IUpdateBuilder
	{
		int Execute();
		IUpdateBuilder Column(string columnName, object value);
		IUpdateBuilder Where(string columnName, object value);
	}

	public interface IUpdateBuilderDynamic
	{
		int Execute();
		IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties);
		IUpdateBuilderDynamic Column(string columnName, object value);
		IUpdateBuilderDynamic Column(string propertyName);
		IUpdateBuilderDynamic Where(string name);
		IUpdateBuilderDynamic Where(string columnName, object value);
	}

	public interface IUpdateBuilder<T>
	{
		int Execute();
		IUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties);
		IUpdateBuilder<T> Where(Expression<Func<T, object>> expression);
		IUpdateBuilder<T> Where(string columnName, object value);
		IUpdateBuilder<T> Column(string columnName, object value);
		IUpdateBuilder<T> Column(Expression<Func<T, object>> expression);
	}

	internal class UpdateBuilder : BaseUpdateBuilder, IUpdateBuilder, IInsertUpdateBuilder
	{
		internal UpdateBuilder(IDbProvider dbProvider, IDbCommand command, string name)
			: base(dbProvider, command, name)
		{
		}

		public virtual IUpdateBuilder Where(string columnName, object value)
		{
			Actions.WhereAction(columnName, value);
			return this;
		}

		public IUpdateBuilder Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilder IInsertUpdateBuilder.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}
	}

	internal class UpdateBuilderDynamic : BaseUpdateBuilder, IUpdateBuilderDynamic, IInsertUpdateBuilderDynamic
	{
		internal UpdateBuilderDynamic(IDbProvider dbProvider, IDbCommand command, string name, ExpandoObject item)
			: base(dbProvider, command, name)
		{
			Data.Item = (IDictionary<string, object>) item;
		}

		public virtual IUpdateBuilderDynamic Where(string columnName, object value)
		{
			Actions.WhereAction(columnName, value);
			return this;
		}

		public IUpdateBuilderDynamic Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		public IUpdateBuilderDynamic Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}

		public IUpdateBuilderDynamic Where(string name)
		{
			var propertyValue = ReflectionHelper.GetPropertyValueDynamic(Data.Item, name);
			Where(name, propertyValue);
			return this;
		}

		public IUpdateBuilderDynamic AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.AutoMap(params string[] ignoreProperties)
		{
			Actions.AutoMapDynamicTypeColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilderDynamic IInsertUpdateBuilderDynamic.Column(string propertyName)
		{
			Actions.ColumnValueDynamic((ExpandoObject) Data.Item, propertyName);
			return this;
		}
	}

	internal class UpdateBuilder<T> : BaseUpdateBuilder, IUpdateBuilder<T>, IInsertUpdateBuilder<T>
	{
		internal UpdateBuilder(IDbProvider provider, IDbCommand command, string name, T item)
			: base(provider, command, name)
		{
			Data.Item = item;
		}

		public IUpdateBuilder<T> Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		public IUpdateBuilder<T> AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(false, ignoreProperties);
			return this;
		}

		public IUpdateBuilder<T> Column(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}

		public virtual IUpdateBuilder<T> Where(string columnName, object value)
		{
			Actions.WhereAction(columnName, value);
			return this;
		}

		public IUpdateBuilder<T> Where(Expression<Func<T, object>> expression)
		{
			Actions.WhereAction(expression);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.AutoMap(params Expression<Func<T, object>>[] ignoreProperties)
		{
			Actions.AutoMapColumnsAction(false, ignoreProperties);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(string columnName, object value)
		{
			Actions.ColumnValueAction(columnName, value, false);
			return this;
		}

		IInsertUpdateBuilder<T> IInsertUpdateBuilder<T>.Column(Expression<Func<T, object>> expression)
		{
			Actions.ColumnValueAction(expression, false);
			return this;
		}
	}

	public enum DataTypes
	{
		// Summary:
		//     A variable-length stream of non-Unicode characters ranging between 1 and
		//     8,000 characters.
		AnsiString = 0,
		//
		// Summary:
		//     A variable-length stream of binary data ranging between 1 and 8,000 bytes.
		Binary = 1,
		//
		// Summary:
		//     An 8-bit unsigned integer ranging in value from 0 to 255.
		Byte = 2,
		//
		// Summary:
		//     A simple type representing Boolean values of true or false.
		Boolean = 3,
		//
		// Summary:
		//     A currency value ranging from -2 63 (or -922,337,203,685,477.5808) to 2 63
		//     -1 (or +922,337,203,685,477.5807) with an accuracy to a ten-thousandth of
		//     a currency unit.
		Currency = 4,
		//
		// Summary:
		//     A type representing a date value.
		Date = 5,
		//
		// Summary:
		//     A type representing a date and time value.
		DateTime = 6,
		//
		// Summary:
		//     A simple type representing values ranging from 1.0 x 10 -28 to approximately
		//     7.9 x 10 28 with 28-29 significant digits.
		Decimal = 7,
		//
		// Summary:
		//     A floating point type representing values ranging from approximately 5.0
		//     x 10 -324 to 1.7 x 10 308 with a precision of 15-16 digits.
		Double = 8,
		//
		// Summary:
		//     A globally unique identifier (or GUID).
		Guid = 9,
		//
		// Summary:
		//     An integral type representing signed 16-bit integers with values between
		//     -32768 and 32767.
		Int16 = 10,
		//
		// Summary:
		//     An integral type representing signed 32-bit integers with values between
		//     -2147483648 and 2147483647.
		Int32 = 11,
		//
		// Summary:
		//     An integral type representing signed 64-bit integers with values between
		//     -9223372036854775808 and 9223372036854775807.
		Int64 = 12,
		//
		// Summary:
		//     A general type representing any reference or value type not explicitly represented
		//     by another DataTypes value.
		Object = 13,
		//
		// Summary:
		//     An integral type representing signed 8-bit integers with values between -128
		//     and 127.
		SByte = 14,
		//
		// Summary:
		//     A floating point type representing values ranging from approximately 1.5
		//     x 10 -45 to 3.4 x 10 38 with a precision of 7 digits.
		Single = 15,
		//
		// Summary:
		//     A type representing Unicode character strings.
		String = 16,
		//
		// Summary:
		//     A type representing a SQL Server DateTime value. If you want to use a SQL
		//     Server time value, use System.Data.SqlDbType.Time.
		Time = 17,
		//
		// Summary:
		//     An integral type representing unsigned 16-bit integers with values between
		//     0 and 65535.
		UInt16 = 18,
		//
		// Summary:
		//     An integral type representing unsigned 32-bit integers with values between
		//     0 and 4294967295.
		UInt32 = 19,
		//
		// Summary:
		//     An integral type representing unsigned 64-bit integers with values between
		//     0 and 18446744073709551615.
		UInt64 = 20,
		//
		// Summary:
		//     A variable-length numeric value.
		VarNumeric = 21,
		//
		// Summary:
		//     A fixed-length stream of non-Unicode characters.
		AnsiStringFixedLength = 22,
		//
		// Summary:
		//     A fixed-length string of Unicode characters.
		StringFixedLength = 23,
		//
		// Summary:
		//     A parsed representation of an XML document or fragment.
		Xml = 25,
		//
		// Summary:
		//     Date and time data. Date value range is from January 1,1 AD through December
		//     31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an
		//     accuracy of 100 nanoseconds.
		DateTime2 = 26,
		//
		// Summary:
		//     Date and time data with time zone awareness. Date value range is from January
		//     1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through
		//     23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range
		//     is -14:00 through +14:00.
		DateTimeOffset = 27,
	}

	internal partial class DbCommand : IDisposable, IDbCommand
	{
		private readonly DbCommandData _data;

		public DbCommand(
			DbContext dbContext,
			System.Data.IDbCommand dbCommand,
			DbContextData dbContextData)
		{
			_data = new DbCommandData();
			_data.Context = dbContext;
			_data.ContextData = dbContextData;
			_data.InnerCommand = dbCommand;
			_data.Command = this;
			_data.ExecuteQueryHandler = new ExecuteQueryHandler(_data, this);
		}

		internal IDbCommand UseMultipleResultset
		{
			get
			{
				if (!_data.ContextData.Provider.SupportsMultipleResultset)
					throw new FluentDataException("The selected database does not support multiple resultset");

				_data.UseMultipleResultsets = true;
				return this;
			}
		}

		public IDbCommand CommandType(DbCommandTypes dbCommandType)
		{
			_data.CommandType = dbCommandType;
			_data.InnerCommand.CommandType = (System.Data.CommandType) dbCommandType;
			return this;
		}

		internal void ClosePrivateConnection()
		{
			if (!_data.ContextData.UseTransaction
				&& !_data.ContextData.UseSharedConnection)
			{
				_data.InnerCommand.Connection.Close();

				if (_data.ContextData.OnConnectionClosed != null)
					_data.ContextData.OnConnectionClosed(new OnConnectionClosedEventArgs(_data.InnerCommand.Connection));
			}
		}

		public void Dispose()
		{
			if (_data.Reader != null)
				_data.Reader.Close();

			ClosePrivateConnection();
		}
	}

	public class DbCommandData
	{
		public IDbCommand Command { get; set; }
		public DbContext Context { get; set; }
		public DbContextData ContextData { get; set; }
		public System.Data.IDbCommand InnerCommand { get; set; }
		public StringBuilder Sql { get; set; }
		public bool UseMultipleResultsets { get; set; }
		public IDataReader Reader { get; set; }
		public ParameterCollection Parameters { get; set; }
		internal ExecuteQueryHandler ExecuteQueryHandler;
		public DbCommandTypes CommandType { get; set; }

		public DbCommandData()
		{
			Parameters = new ParameterCollection();
			CommandType = DbCommandTypes.Text;
		}
	}

	public enum DbCommandTypes
	{
		// Summary:
		//     An SQL text command. (Default.)
		Text = 1,
		//
		// Summary:
		//     The name of a stored procedure.
		StoredProcedure = 4,
		//
		// Summary:
		//     The name of a table.
		TableDirect = 512,
	}

	public interface IDbCommand : IDisposable
	{
		IDbCommand ParameterOut(string name, DataTypes parameterType, int size = 0);
		IDbCommand Parameter(string name, object value);
		IDbCommand Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction, int size = 0);
		IDbCommand Parameters(params object[] parameters);
		TParameterType ParameterValue<TParameterType>(string outputParameterName);
		int Execute();
		int ExecuteReturnLastId();
		T ExecuteReturnLastId<T>();
		int ExecuteReturnLastId(string identityColumnName);
		T ExecuteReturnLastId<T>(string identityColumnName);
		List<dynamic> Query();
		TList Query<TEntity, TList>() where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> Query<TEntity>();
		List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper);
		List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper);
		TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper);
		TList QueryNoAutoMap<TEntity, TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>;
		TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>;
		List<TEntity> QueryNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper);
		List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		dynamic QuerySingle();
		TEntity QuerySingle<TEntity>();
		TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper);
		TEntity QuerySingle<TEntity>(Action<dynamic, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper);
		TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper);
		T QueryValue<T>();
		List<T> QueryValues<T>();
		IDbCommand Sql(string sql);
		IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpression);
		IDbCommand CommandType(DbCommandTypes dbCommandType);
	}

	internal class AutoMapper<T>
	{
		private readonly DbCommandData _dbCommandData;

		internal AutoMapper(DbCommandData dbCommandData)
		{
			_dbCommandData = dbCommandData;
		}

		public void AutoMap(object item)
		{
			var properties = ReflectionHelper.GetProperties(item.GetType());
			var fields = DataReaderHelper.GetDataReaderFields(_dbCommandData.Reader);

			foreach (var field in fields)
			{
				if (field.IsSystem)
					continue;

				var value = _dbCommandData.Reader.GetValue(field.Index);
				var wasMapped = false;

				PropertyInfo property = null;
					
				if (properties.TryGetValue(field.LowerName, out property))
				{
					SetPropertyValue(field, property, item, value);
					wasMapped = true;
				}
				else
				{
					if (field.LowerName.IndexOf('_') != -1)
						wasMapped = HandleComplexField(item, field, value);
				}

				if (!wasMapped && !_dbCommandData.ContextData.IgnoreIfAutoMapFails)
					throw new FluentDataException("Could not map: " + field.Name);
			}
		}

		private bool HandleComplexField(object item, DataReaderField field, object value)
		{
			string propertyName = null;

			for (var level = 0; level <= field.NestedLevels; level++)
			{
				if (string.IsNullOrEmpty(propertyName))
					propertyName = field.GetNestedName(level);
				else
					propertyName += "_" + field.GetNestedName(level);

				PropertyInfo property = null;
				var properties = ReflectionHelper.GetProperties(item.GetType());
				if (properties.TryGetValue(propertyName, out property))
				{
					if (level == field.NestedLevels)
					{
						SetPropertyValue(field, property, item, value);
						return true;
					}
					else
					{
						item = GetOrCreateInstance(item, property);
						if (item == null)
							return false;
						propertyName = null;	
					}
				}
			}

			return false;
		}

		private object GetOrCreateInstance(object item, PropertyInfo property)
		{
			object instance = ReflectionHelper.GetPropertyValue(item, property);

			if (instance == null)
			{
				instance = _dbCommandData.ContextData.EntityFactory.Create(property.PropertyType);

				property.SetValue(item, instance, null);
			}

			return instance;
		}

		private void SetPropertyValue(DataReaderField field, PropertyInfo property, object item, object value)
		{
			try
			{
				if (value == DBNull.Value)
				{
					if (ReflectionHelper.IsNullable(property))
						value = null;
					else
						value = ReflectionHelper.GetDefault(property.PropertyType);
				}
				else
				{
					var propertyType = ReflectionHelper.GetPropertyType(property);

					if (propertyType != field.Type)
					{
						if (propertyType.IsEnum)
						{
							if (field.Type == typeof(string))
								value = Enum.Parse(propertyType, (string) value, true);
							else
								value = Enum.ToObject(propertyType, value);
						}
						else if (!ReflectionHelper.IsBasicClrType(propertyType))
							return;
						else if (propertyType == typeof(string))
							value = value.ToString();
						else
							value = Convert.ChangeType(value, property.PropertyType);
					}
				}

				property.SetValue(item, value, null);
			}
			catch (Exception exception)
			{
				throw new FluentDataException("Could not map: " + property.Name, exception);
			}
		}	
	}

	internal class DataReaderField
	{
		public int Index { get; private set; }
		public string LowerName { get; private set; }
		public string Name { get; private set; }
		public Type Type { get; private set; }
		private readonly string[] _nestedPropertyNames;
		private readonly int _nestedLevels;

		public DataReaderField(int index, string name, Type type)
		{
			Index = index;
			Name = name;
			LowerName = name.ToLower();
			Type = type;
			_nestedPropertyNames = LowerName.Split('_');
			_nestedLevels = _nestedPropertyNames.Count() - 1;
		}

		public string GetNestedName(int level)
		{
			return _nestedPropertyNames[level];
		}

		public int NestedLevels
		{
			get
			{
				return _nestedLevels;
			}
		}

		public bool IsSystem
		{
			get
			{
				return Name.IndexOf("fluentdata_") > -1;
			}
		}
	}

	internal class DynamicTypAutoMapper
	{
		private readonly DbCommandData _dbCommandData;

		public DynamicTypAutoMapper(DbCommandData dbCommandData)
		{
			_dbCommandData = dbCommandData;
		}

		public ExpandoObject AutoMap()
		{
			var item = new ExpandoObject();

			var fields = DataReaderHelper.GetDataReaderFields(_dbCommandData.Reader);

			var itemDictionary = (IDictionary<string, object>) item;

			foreach (var column in fields)
			{
				var value = DataReaderHelper.GetDataReaderValue(_dbCommandData.Reader, column.Index, true);

				itemDictionary.Add(column.Name, value); 
			}

			return item;
		}
	}

	public class Parameter
	{
		public string ParameterName { get; set; }
		public DataTypes DataTypes { get; set; }
		public object Value { get; set; }
		public ParameterDirection Direction { get; set; }
		public bool IsId { get; set; }
		public int Size { get; set; }

		public string GetParameterName(IDbProvider provider)
		{
			return provider.GetParameterName(ParameterName);
		}
	}

	public class ParameterCollection : List<Parameter>
	{
		public Parameter this[string name]
		{
			get
			{
				return this.SingleOrDefault(x => x.ParameterName == name);
			}
		}
	}

	public enum ParameterDirection
	{
		// The parameter is an input parameter.
		Input = 1,

		// The parameter is an output parameter.
		Output = 2,

		// The parameter is capable of both input and output.
		InputOutput = 3,

		// The parameter represents a return value from an operation such as a stored
		// procedure, built-in function, or user-defined function.
		ReturnValue = 6,
	}

	internal class ParameterHandler
	{
		internal void FixParameterType(DbCommandData data)
		{
			foreach (var parameter in data.Parameters)
			{
				if (parameter.Direction == ParameterDirection.Input
					&& parameter.DataTypes == DataTypes.Object)
				{
					if (parameter.Value == null)
					{
						parameter.DataTypes = DataTypes.String;
					}
					else
					{
						parameter.DataTypes = data.ContextData.Provider.GetDbTypeForClrType(parameter.Value.GetType());
						if (parameter.DataTypes == DataTypes.Object)
							throw new FluentDataException(string.Format("The parameter {0} is of a type that is not supported.", parameter.ParameterName));
					}
				}

				if (parameter.Value == null)
					parameter.Value = DBNull.Value;

				var dbParameter = data.InnerCommand.CreateParameter();
				dbParameter.DbType = (System.Data.DbType) parameter.DataTypes;
				dbParameter.ParameterName = data.ContextData.Provider.GetParameterName(parameter.ParameterName);
				dbParameter.Direction = (System.Data.ParameterDirection) parameter.Direction;
				dbParameter.Value = parameter.Value;
				if (parameter.Size > 0)
					dbParameter.Size = parameter.Size;
				data.InnerCommand.Parameters.Add(dbParameter);
			}
		}
	}

	internal partial class DbCommand
	{
		public TList QueryNoAutoMap<TEntity, TList>(Func<dynamic, TEntity> customMapper) where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new QueryNoAutoMapHandler<TEntity>().QueryNoAutoMap<TList>(_data, null, customMapper);
			});

			return items;
		}
		
		public List<TEntity> QueryNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			return QueryNoAutoMap<TEntity, List<TEntity>>(customMapper);
		}

		public TList QueryNoAutoMap<TEntity, TList>(Func<IDataReader, TEntity> customMapper) where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new QueryNoAutoMapHandler<TEntity>().QueryNoAutoMap<TList>(_data, customMapper, null);
			});

			return items;
		}

		public List<TEntity> QueryNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			return QueryNoAutoMap<TEntity, List<TEntity>>(customMapper);
		}
	}

	internal partial class DbCommand
	{
		public TEntity QuerySingleNoAutoMap<TEntity>(Func<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QuerySingleNoAutoMapHandler<TEntity>().ExecuteSingleNoAutoMap(_data, customMapper, null);
			});

			return item;
		}

		public TEntity QuerySingleNoAutoMap<TEntity>(Func<dynamic, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new QuerySingleNoAutoMapHandler<TEntity>().ExecuteSingleNoAutoMap(_data, null, customMapper);
			});

			return item;
		}
	}

	internal partial class DbCommand
	{
		public int ExecuteReturnLastId()
		{
			return ExecuteReturnLastId<int>();
		}

		public T ExecuteReturnLastId<T>()
		{
			if (!_data.ContextData.Provider.SupportsExecuteReturnLastIdWithNoIdentityColumn)
				throw new FluentDataException("The selected database does not support this method.");

			T lastId = _data.ContextData.Provider.ExecuteReturnLastId<T>(_data, null);

			return lastId;
		}

		public int ExecuteReturnLastId(string identityColumnName)
		{
			return ExecuteReturnLastId<int>(identityColumnName);
		}

		public T ExecuteReturnLastId<T>(string identityColumnName)
		{
			if (_data.ContextData.Provider.SupportsExecuteReturnLastIdWithNoIdentityColumn)
				throw new FluentDataException("The selected database does not support this method.");

			T lastId = _data.ContextData.Provider.ExecuteReturnLastId<T>(_data, identityColumnName);

			return lastId;
		}

		
	}

	internal partial class DbCommand
	{
		public IDbCommand Sql(string sql)
		{
			if (_data.Sql == null)
				_data.Sql = new StringBuilder();
			_data.Sql.Append(sql);
			return this;
		}

		public IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			if (_data.Sql == null)
				_data.Sql = new StringBuilder();

			if (mappingExpressions == null)
			{
				_data.Sql.Append(sql);
			}
			else
			{
				var propertyNames = ReflectionHelper.GetPropertyNamesFromExpressions(mappingExpressions);
				for (int i = 0; i < propertyNames.Count; i++)
				{
					propertyNames[i] = propertyNames[i].Replace('.', '_');
				}

				_data.Sql.AppendFormat(sql, propertyNames.ToArray());
			}
			return this;
		}
	}

	internal partial class DbCommand
	{
		/// <returns>Numbers of records affected.</returns>
		public int Execute()
		{
			int recordsAffected = 0;

			_data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				recordsAffected = new ExecuteHandler().Execute<int>(_data);
			});
			return recordsAffected;
		}
	}

	internal partial class DbCommand
	{
		public IDbCommand Parameters(params object[] parameters)
		{
			for (int i = 0; i < parameters.Count(); i++)
				Parameter(i.ToString(), parameters[i]);
			return this;
		}

		public IDbCommand Parameter(string name, object value, DataTypes parameterType, ParameterDirection direction, int size = 0)
		{
			var parameter = new Parameter();
			parameter.DataTypes = parameterType;
			parameter.ParameterName = name;
			parameter.Direction = direction;
			parameter.Value = value;
			parameter.Size = size;
			_data.Parameters.Add(parameter);
			return this;
		}

		public IDbCommand Parameter(string name, object value)
		{
			Parameter(name, value, DataTypes.Object, ParameterDirection.Input);
			return this;
		}

		public IDbCommand ParameterOut(string name, DataTypes parameterType, int size)
		{
			if (!_data.ContextData.Provider.SupportsOutputParameters)
				throw new FluentDataException("The selected database does not support output parameters");
			Parameter(name, null, parameterType, ParameterDirection.Output, size);
			return this;
		}

		public TParameterType ParameterValue<TParameterType>(string outputParameterName)
		{
			outputParameterName = _data.ContextData.Provider.GetParameterName(outputParameterName);
			if (!_data.InnerCommand.Parameters.Contains(outputParameterName))
				throw new FluentDataException(string.Format("Parameter {0} not found", outputParameterName));

			var value = (_data.InnerCommand.Parameters[outputParameterName] as System.Data.IDataParameter).Value;
			if (value == null)
				return default(TParameterType);

			return (TParameterType) value;
		}
	}

	internal partial class DbCommand
	{
		private TList Query<TEntity, TList>(
								Action<IDataReader, TEntity> customMapperReader,
								Action<dynamic, TEntity> customMapperDynamic)
			where TList : IList<TEntity>
		{
			var items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new GenericQueryHandler<TEntity>().ExecuteListReader<TList>(_data, customMapperReader, customMapperDynamic);
			});

			return items;
		}

		public TList Query<TEntity, TList>()
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(null, null);
		}

		public List<TEntity> Query<TEntity>()
		{
			return Query<TEntity, List<TEntity>>(null, null);
		}

		public TList Query<TEntity, TList>(Action<IDataReader, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(customMapper, null);
		}

		public TList Query<TEntity, TList>(Action<dynamic, TEntity> customMapper)
			where TList : IList<TEntity>
		{
			return Query<TEntity, TList>(null, customMapper);
		}

		public List<TEntity> Query<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(customMapper, null);
		}

		public List<TEntity> Query<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			return Query<TEntity, List<TEntity>>(null, customMapper);
		}
	}

	internal partial class DbCommand
	{
		public TList QueryComplex<TEntity, TList>(Action<IDataReader, IList<TEntity>> customMapper)
			where TList : IList<TEntity>
		{
			TList items = default(TList);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new QueryComplexHandler<TEntity, TList>().Execute(_data, customMapper);
			});

			return items;
		}

		public List<TEntity> QueryComplex<TEntity>(Action<IDataReader, IList<TEntity>> customMapper)
		{
			return QueryComplex<TEntity, List<TEntity>>(customMapper);
		}
	}

	internal partial class DbCommand
	{
		public List<dynamic> Query()
		{
			List<dynamic> items = null;

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				items = new DynamicQueryHandler().ExecuteList(_data);
			});

			return items;
		}

		public dynamic QuerySingle()
		{
			dynamic item = null;

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new DynamicQueryHandler().ExecuteSingle(_data);
			});

			return item;
		}
	}

	internal partial class DbCommand
	{
		public TEntity QuerySingle<TEntity>()
		{
			return QuerySingle<TEntity>(null);
		}

		public TEntity QuerySingle<TEntity>(Action<IDataReader, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new GenericQueryHandler<TEntity>().ExecuteSingle(_data, customMapper, null);
			});

			return item;
		}

		public TEntity QuerySingle<TEntity>(Action<dynamic, TEntity> customMapper)
		{
			var item = default(TEntity);

			_data.ExecuteQueryHandler.ExecuteQuery(true, () =>
			{
				item = new GenericQueryHandler<TEntity>().ExecuteSingle(_data, null, customMapper);
			});

			return item;
		}
	}

	internal partial class DbCommand
	{
		public T QueryValue<T>()
		{
			T value = default(T);

			_data.ExecuteQueryHandler.ExecuteQuery(true,
				() =>
				{
					value = new QueryValueHandler<T>().Execute(_data);
				});

			return value;
		}

		public List<T> QueryValues<T>()
		{
			List<T> values = null;

			_data.ExecuteQueryHandler.ExecuteQuery(true,
				() =>
				{
					values = new QueryValuesHandler<T>().Execute(_data);
				});

			return values;
		}
	}

	internal class QueryNoAutoMapHandler<TEntity>
	{
		internal TList QueryNoAutoMap<TList>(DbCommandData data,
			Func<IDataReader, TEntity> customMapperReader,
			Func<dynamic, TEntity> customMapperDynamic)
			where TList : IList<TEntity>
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			DynamicTypAutoMapper dynamicAutoMapper = null;

			while (data.Reader.Read())
			{
				var item = default(TEntity);

				if (customMapperReader != null)
				{
					item = customMapperReader(data.Reader);
				}
				else if (customMapperDynamic != null)
				{
					if (dynamicAutoMapper == null)
						dynamicAutoMapper = new DynamicTypAutoMapper(data);

					var dynamicObject = dynamicAutoMapper.AutoMap();
					item = customMapperDynamic(dynamicObject);
				}

				items.Add(item);
			}

			return items;
		}
	}

	internal class QuerySingleNoAutoMapHandler<TEntity>
	{
		internal TEntity ExecuteSingleNoAutoMap(DbCommandData data,
			Func<IDataReader, TEntity> customMapperReader,
			Func<dynamic, TEntity> customMapperDynamic)
		{
			var item = default(TEntity);

			if (data.Reader.Read())
			{
				if (customMapperReader != null)
					item = customMapperReader(data.Reader);
				else if (customMapperDynamic != null)
				{
					var dynamicObject = new DynamicTypAutoMapper(data).AutoMap();
					item = customMapperDynamic(dynamicObject);
				}
			}

			return item;
		}
	}

	internal class DynamicQueryHandler
	{
		public List<dynamic> ExecuteList(DbCommandData data)
		{
			var items = new List<dynamic>();

			var autoMapper = new DynamicTypAutoMapper(data);

			while (data.Reader.Read())
			{
				var item = autoMapper.AutoMap();

				items.Add(item);
			}

			return items;
		}

		public dynamic ExecuteSingle(DbCommandData data)
		{
			var autoMapper = new DynamicTypAutoMapper(data);

			ExpandoObject item = null;

			if (data.Reader.Read())
				item = autoMapper.AutoMap();

			return item;
		}
	}

	internal class ExecuteHandler
	{
		public T Execute<T>(DbCommandData data)
		{
			object recordsAffected = data.InnerCommand.ExecuteNonQuery();

			return (T) recordsAffected;
		}
	}

	internal class GenericQueryHandler<TEntity>
	{
		internal TList ExecuteListReader<TList>(
									DbCommandData data,
									Action<IDataReader, TEntity> customMapperReader,
									Action<dynamic, TEntity> customMapperDynamic
					)
			where TList : IList<TEntity>
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			var autoMapper = new AutoMapper<TEntity>(data);

			DynamicTypAutoMapper dynamicAutoMapper = null;

			while (data.Reader.Read())
			{
				var item = (TEntity) data.ContextData.EntityFactory.Create(typeof(TEntity));

				autoMapper.AutoMap(item);

				if (customMapperReader != null)
					customMapperReader(data.Reader, item);

				if (customMapperDynamic != null)
				{
					if (dynamicAutoMapper == null)
						dynamicAutoMapper = new DynamicTypAutoMapper(data);
					var dynamicObject = dynamicAutoMapper.AutoMap();
					customMapperDynamic(dynamicObject, item);
				}

				items.Add(item);
			}

			return items;
		}

		internal TEntity ExecuteSingle(DbCommandData data,
										Action<IDataReader, TEntity> customMapper,
										Action<dynamic, TEntity> customMapperDynamic)
		{
			AutoMapper<TEntity> autoMapper = null;

			autoMapper = new AutoMapper<TEntity>(data);

			var item = default(TEntity);

			if (data.Reader.Read())
			{
				item = (TEntity) data.ContextData.EntityFactory.Create(typeof(TEntity));

				autoMapper.AutoMap(item);

				if (customMapper != null)
					customMapper(data.Reader, item);

				if (customMapperDynamic != null)
				{
					var dynamicAutoMapper = new DynamicTypAutoMapper(data);
					var dynamicObject = dynamicAutoMapper.AutoMap();
					customMapperDynamic(dynamicObject, item);
				}
			}

			return item;
		}
	}

	internal class QueryComplexHandler<TEntity, TList>
		where TList : IList<TEntity>
	{
		public TList Execute(DbCommandData data, Action<IDataReader, IList<TEntity>> customMapper)
		{
			var items = (TList) data.ContextData.EntityFactory.Create(typeof(TList));

			while (data.Reader.Read())
				customMapper(data.Reader, items);

			return items;
		}
	}

	internal class ExecuteQueryHandler
	{
		private readonly DbCommandData _data;
		private readonly DbCommand _command;
		private bool _queryAlreadyExecuted;

		public ExecuteQueryHandler(DbCommandData data, DbCommand command)
		{
			_data = data;
			_command = command;
		}

		internal void ExecuteQuery(bool useReader, Action action)
		{
			try
			{
				PrepareDbCommand(useReader);

				action();

				if (_data.ContextData.OnExecuted != null)
					_data.ContextData.OnExecuted(new OnExecutedEventArgs(_data.InnerCommand));
			}
			catch (Exception exception)
			{
			    HandleQueryException(exception);
			}
			finally
			{
			    HandleQueryFinally();
			}
		}

		private void PrepareDbCommand(bool useReader)
		{
			if (_queryAlreadyExecuted)
			{
				if (_data.UseMultipleResultsets)
					_data.Reader.NextResult();
				else
					throw new FluentDataException("A query has already been executed on this command object. Please create a new command object.");
			}
			else
			{
				FixSql();
				new ParameterHandler().FixParameterType(_data);

				if (_data.ContextData.CommandTimeout != Int32.MinValue)
					_data.InnerCommand.CommandTimeout = _data.ContextData.CommandTimeout;

				if (_data.ContextData.UseTransaction)
				{
					if (_data.ContextData.Transaction == null)
					{
						OpenConnection();
						_data.ContextData.Transaction = _data.ContextData.Connection.BeginTransaction((System.Data.IsolationLevel) _data.ContextData.IsolationLevel);
					}
					_data.InnerCommand.Transaction = _data.ContextData.Transaction;
				}
				else
				{
					if (_data.InnerCommand.Connection.State != ConnectionState.Open)
						OpenConnection();
				}

				if (_data.ContextData.OnExecuting != null)
					_data.ContextData.OnExecuting(new OnExecutingEventArgs(_data.InnerCommand));
				
				if (useReader)
					_data.Reader = new DataReader(_data.InnerCommand.ExecuteReader());

				_queryAlreadyExecuted = true;
			}
		}

		private void OpenConnection()
		{
			if (_data.ContextData.OnConnectionOpening != null)
				_data.ContextData.OnConnectionOpening(new OnConnectionOpeningEventArgs(_data.InnerCommand.Connection));

			_data.InnerCommand.Connection.Open();

			if (_data.ContextData.OnConnectionOpened != null)
				_data.ContextData.OnConnectionOpened(new OnConnectionOpenedEventArgs(_data.InnerCommand.Connection));
		}

		private void HandleQueryFinally()
		{
			if (!_data.UseMultipleResultsets)
			{
				if (_data.Reader != null)
					_data.Reader.Close();

				_command.ClosePrivateConnection();
			}
		}

		private void HandleQueryException(Exception exception)
		{
			if (_data.Reader != null)
				_data.Reader.Close();

			_command.ClosePrivateConnection();
			if (_data.ContextData.UseTransaction)
				_data.Context.CloseSharedConnection();

			if (_data.ContextData.OnError != null)
				_data.ContextData.OnError(new OnErrorEventArgs(_data.InnerCommand, exception));
			
			throw exception;
		}

		private void FixSql()
		{
			_data.ContextData.Provider.FixInStatement(_data.Sql, _data.Parameters);
			_data.InnerCommand.CommandText = _data.Sql.ToString();
		}
	}

	internal class QueryValueHandler<T>
	{
		public T Execute(DbCommandData data)
		{
			var value = default(T);

			if (data.Reader.Read())
			{
				if (data.Reader.GetFieldType(0) == typeof(T))
					value = (T) data.Reader.GetValue(0);
				else
					value = (T) Convert.ChangeType(data.Reader.GetValue(0), typeof(T));
			}
			
			return value;
		}
	}

	internal class QueryValuesHandler<T>
	{
		public List<T> Execute(DbCommandData data)
		{
			var items = new List<T>();

			while (data.Reader.Read())
			{
				T value;

				if (data.Reader.GetFieldType(0) == typeof(T))
					value = (T) data.Reader.GetValue(0);
				else
					value = (T) Convert.ChangeType(data.Reader.GetValue(0), typeof(T));

				items.Add(value);
			}

			return items;
		}
	}

	internal class DataReader : System.Data.IDataReader, IDataReader
	{
		private readonly System.Data.IDataReader _innerReader;

		public DataReader(System.Data.IDataReader reader)
		{
			_innerReader = reader;
		}

		public void Close()
		{
			_innerReader.Close();
		}

		public int Depth
		{
			get { return _innerReader.Depth; }
		}

		public DataTable GetSchemaTable()
		{
			return _innerReader.GetSchemaTable();
		}

		public bool IsClosed
		{
			get { return _innerReader.IsClosed; }
		}

		public bool NextResult()
		{
			return _innerReader.NextResult();
		}

		public bool Read()
		{
			return _innerReader.Read();
		}

		public int RecordsAffected
		{
			get { return _innerReader.RecordsAffected; }
		}

		public void Dispose()
		{
			_innerReader.Dispose();
		}

		public int FieldCount
		{
			get { return _innerReader.FieldCount; }
		}

		public bool GetBoolean(int i)
		{
			return _innerReader.GetBoolean(i);
		}

		public bool GetBoolean(string name)
		{
			return _innerReader.GetBoolean(GetOrdinal(name));
		}

		public byte GetByte(int i)
		{
			return _innerReader.GetByte(i);
		}

		public byte GetByte(string name)
		{
			return _innerReader.GetByte(GetOrdinal(name));
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
		}

		public long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetBytes(GetOrdinal(name), fieldOffset, buffer, bufferoffset, length);
		}

		public char GetChar(int i)
		{
			return _innerReader.GetChar(i);
		}

		public char GetChar(string name)
		{
			return _innerReader.GetChar(GetOrdinal(name));
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return _innerReader.GetChars(GetOrdinal(name), fieldoffset, buffer, bufferoffset, length);
		}

		public System.Data.IDataReader GetData(int i)
		{
			return _innerReader.GetData(i);
		}

		public System.Data.IDataReader GetData(string name)
		{
			return _innerReader.GetData(GetOrdinal(name));
		}

		public string GetDataTypeName(int i)
		{
			return _innerReader.GetDataTypeName(i);
		}

		public string GetDataTypeName(string name)
		{
			return _innerReader.GetDataTypeName(GetOrdinal(name));
		}

		public DateTime GetDateTime(int i)
		{
			return _innerReader.GetDateTime(i);
		}

		public DateTime GetDateTime(string name)
		{
			return _innerReader.GetDateTime(GetOrdinal(name));
		}

		public decimal GetDecimal(int i)
		{
			return _innerReader.GetDecimal(i);
		}

		public decimal GetDecimal(string name)
		{
			return _innerReader.GetDecimal(GetOrdinal(name));
		}

		public double GetDouble(int i)
		{
			return _innerReader.GetDouble(i);
		}

		public double GetDouble(string name)
		{
			return _innerReader.GetDouble(GetOrdinal(name));
		}

		public Type GetFieldType(int i)
		{
			return _innerReader.GetFieldType(i);
		}

		public Type GetFieldType(string name)
		{
			return _innerReader.GetFieldType(GetOrdinal(name));
		}

		public float GetFloat(int i)
		{
			return _innerReader.GetFloat(i);
		}

		public float GetFloat(string name)
		{
			return _innerReader.GetFloat(GetOrdinal(name));
		}

		public Guid GetGuid(int i)
		{
			return _innerReader.GetGuid(i);
		}

		public Guid GetGuid(string name)
		{
			return _innerReader.GetGuid(GetOrdinal(name));
		}

		public short GetInt16(int i)
		{
			return _innerReader.GetInt16(i);
		}

		public short GetInt16(string name)
		{
			return _innerReader.GetInt16(GetOrdinal(name));
		}

		public int GetInt32(int i)
		{
			return _innerReader.GetInt32(i);
		}

		public int GetInt32(string name)
		{
			return _innerReader.GetInt32(GetOrdinal(name));
		}

		public long GetInt64(int i)
		{
			return _innerReader.GetInt64(i);
		}

		public long GetInt64(string name)
		{
			return _innerReader.GetInt64(GetOrdinal(name));
		}

		public string GetName(int i)
		{
			return _innerReader.GetName(i);
		}

		public string GetName(string name)
		{
			return _innerReader.GetName(GetOrdinal(name));
		}

		public int GetOrdinal(string name)
		{
			return _innerReader.GetOrdinal(name);
		}

		public string GetString(int i)
		{
			return _innerReader.GetString(i);
		}

		public string GetString(string name)
		{
			return _innerReader.GetString(GetOrdinal(name));
		}

		public object GetValue(int i)
		{
			return _innerReader.GetValue(i);
		}

		public object GetValue(string name)
		{
			return _innerReader.GetValue(GetOrdinal(name));
		}

		public int GetValues(object[] values)
		{
			return _innerReader.GetValues(values);
		}

		public bool IsDBNull(int i)
		{
			return _innerReader.IsDBNull(i);
		}

		public bool IsDBNull(string name)
		{
			return _innerReader.IsDBNull(GetOrdinal(name));
		}

		public object this[string name]
		{
			get { return _innerReader[name]; }
		}

		public object this[int i]
		{
			get { return GetValue(i); }
		}
	}

	public interface IDataReader
	{
		void Close();
		int Depth { get; }
		void Dispose();
		int FieldCount { get; }
		bool GetBoolean(int i);
		bool GetBoolean(string name);
		byte GetByte(int i);
		byte GetByte(string name);
		long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);
		long GetBytes(string name, long fieldOffset, byte[] buffer, int bufferoffset, int length);
		char GetChar(int i);
		char GetChar(string name);
		long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);
		long GetChars(string name, long fieldoffset, char[] buffer, int bufferoffset, int length);
		global::System.Data.IDataReader GetData(int i);
		global::System.Data.IDataReader GetData(string name);
		string GetDataTypeName(int i);
		string GetDataTypeName(string name);
		DateTime GetDateTime(int i);
		DateTime GetDateTime(string name);
		decimal GetDecimal(int i);
		decimal GetDecimal(string name);
		double GetDouble(int i);
		double GetDouble(string name);
		Type GetFieldType(int i);
		Type GetFieldType(string name);
		float GetFloat(int i);
		float GetFloat(string name);
		Guid GetGuid(int i);
		Guid GetGuid(string name);
		short GetInt16(int i);
		short GetInt16(string name);
		int GetInt32(int i);
		int GetInt32(string name);
		long GetInt64(int i);
		long GetInt64(string name);
		string GetName(int i);
		string GetName(string name);
		int GetOrdinal(string name);
		global::System.Data.DataTable GetSchemaTable();
		string GetString(int i);
		string GetString(string name);
		object GetValue(int i);
		object GetValue(string name);
		int GetValues(object[] values);
		bool IsClosed { get; }
		bool IsDBNull(int i);
		bool IsDBNull(string name);
		bool NextResult();
		bool Read();
		int RecordsAffected { get; }
		object this[int i] { get; }
		object this[string name] { get; }
	}

	public class OnConnectionClosedEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public OnConnectionClosedEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}

	public class OnConnectionOpenedEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public OnConnectionOpenedEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}

	public class OnExecutingEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }

		public OnExecutingEventArgs(System.Data.IDbCommand command)
		{
			Command = command;
		}
	}

	public class OnExecutedEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }

		public OnExecutedEventArgs(System.Data.IDbCommand command)
		{
			Command = command;
		}
	}

	public class OnErrorEventArgs : EventArgs
	{
		public System.Data.IDbCommand Command { get; private set; }
		public Exception Exception { get; set; }

		public OnErrorEventArgs(System.Data.IDbCommand command, Exception exception)
		{
			Command = command;
			Exception = exception;
		}
	}

	public partial class DbContext : IDbContext
	{
		protected DbContextData ContextData;

		public DbContext()
		{
			ContextData = new DbContextData();
		}

		internal void CloseSharedConnection()
		{
			if (ContextData.Connection == null)
				return;

			if (ContextData.UseTransaction)
			{
				if (ContextData.TransactionState == TransactionStates.None)
					Rollback();
			}

			ContextData.Connection.Close();

			if (ContextData.OnConnectionClosed != null)
				ContextData.OnConnectionClosed(new OnConnectionClosedEventArgs(ContextData.Connection));
		}

		public void Dispose()
		{
			CloseSharedConnection();
		}
	}

	public class DbContextData
	{
		public bool UseTransaction { get; set; }
		public bool UseSharedConnection { get; set; }
		public System.Data.IDbConnection Connection { get; set; }
		public IsolationLevel IsolationLevel { get; set; }
		public System.Data.IDbTransaction Transaction { get; set; }
		public TransactionStates TransactionState { get; set; }
		public IDbProvider Provider { get; set; }
		public string ConnectionString { get; set; }
		public IEntityFactory EntityFactory { get; set; }
		public DbProviderTypes ProviderType { get; set; }
		public bool IgnoreIfAutoMapFails { get; set; }
		public int CommandTimeout { get; set; }
		public Action<OnConnectionOpeningEventArgs> OnConnectionOpening { get; set; }
		public Action<OnConnectionOpenedEventArgs> OnConnectionOpened { get; set; }
		public Action<OnConnectionClosedEventArgs> OnConnectionClosed { get; set; }
		public Action<OnExecutingEventArgs> OnExecuting { get; set; }
		public Action<OnExecutedEventArgs> OnExecuted { get; set; }
		public Action<OnErrorEventArgs> OnError { get; set; }

		public DbContextData()
		{
			IgnoreIfAutoMapFails = false;
			UseTransaction = false;
			IsolationLevel = IsolationLevel.ReadCommitted;
			EntityFactory = new EntityFactory();
			CommandTimeout = Int32.MinValue;
		}
	}

	public class EntityFactory : IEntityFactory
	{
		public virtual object Create(Type type)
		{
			return Activator.CreateInstance(type);
		}
	}

	public interface IDbContext : IDisposable
	{
		IDbContext IgnoreIfAutoMapFails { get; }
		IDbContext UseTransaction(bool useTransaction);
		IDbContext UseSharedConnection(bool useSharedConnection);
		IDbContext CommandTimeout(int timeout);
		IDbCommand Sql(string sql, params object[] parameters);
		IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpression);
		IDbCommand MultiResultSql();
		IDbCommand MultiResultSql(string sql, params object[] parameters);
		IDbCommand MultiResultSql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions);
		ISelectBuilder<TEntity> Select<TEntity>(string sql);
		ISelectBuilder<TEntity> Select<TEntity>(string sql, Expression<Func<TEntity, object>> mapToProperty);
		IInsertBuilder Insert(string tableName);
		IInsertBuilder<T> Insert<T>(string tableName, T item);
		IInsertBuilderDynamic Insert(string tableName, ExpandoObject item);
		IUpdateBuilder Update(string tableName);
		IUpdateBuilder<T> Update<T>(string tableName, T item);
		IUpdateBuilderDynamic Update(string tableName, ExpandoObject item);
		IDeleteBuilder Delete(string tableName);
		IDeleteBuilder<T> Delete<T>(string tableName, T item);
		IStoredProcedureBuilder StoredProcedure(string storedProcedureName);
		IStoredProcedureBuilder MultiResultStoredProcedure(string storedProcedureName);
		IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item);
		IStoredProcedureBuilder<T> MultiResultStoredProcedure<T>(string storedProcedureName, T item);
		IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item);
		IStoredProcedureBuilderDynamic MultiResultStoredProcedure(string storedProcedureName, ExpandoObject item);
		IDbContext EntityFactory(IEntityFactory entityFactory);
		IDbContext ConnectionString(string connectionString, DbProviderTypes dbProviderType);
		IDbContext ConnectionString(string connectionString, IDbProvider dbProvider);
		IDbContext ConnectionStringName(string connectionstringName, DbProviderTypes dbProviderType);
		IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider);
		IDbContext IsolationLevel(IsolationLevel isolationLevel);
		IDbContext Commit();
		IDbContext Rollback();
		IDbContext OnConnectionOpening(Action<OnConnectionOpeningEventArgs> action);
		IDbContext OnConnectionOpened(Action<OnConnectionOpenedEventArgs> action);
		IDbContext OnConnectionClosed(Action<OnConnectionClosedEventArgs> action);
		IDbContext OnExecuting(Action<OnExecutingEventArgs> action);
		IDbContext OnExecuted(Action<OnExecutedEventArgs> action);
		IDbContext OnError(Action<OnErrorEventArgs> action);
	}

	public interface IEntityFactory
	{
		object Create(Type type);
	}

	public enum IsolationLevel
	{
		Unspecified = -1,
		Chaos = 16,
		ReadUncommitted = 256,
		ReadCommitted = 4096,
		RepeatableRead = 65536,
		Serializable = 1048576,
		Snapshot = 16777216,
	}

	public class OnConnectionOpeningEventArgs : EventArgs
	{
		public IDbConnection Connection { get; private set; }

		public OnConnectionOpeningEventArgs(System.Data.IDbConnection connection)
		{
			Connection = connection;
		}
	}

	public partial class DbContext : IDbContext
	{
		public IDbContext IgnoreIfAutoMapFails
		{
			get
			{
				ContextData.IgnoreIfAutoMapFails = true;
				return this;
			}
		}
	}

	public partial class DbContext : IDbContext
	{
		public ISelectBuilder<TEntity> Select<TEntity>(string sql)
		{
			return new SelectBuilder<TEntity>(ContextData.Provider, CreateCommand).Select(sql);
		}

		public ISelectBuilder<TEntity> Select<TEntity>(string sql, Expression<Func<TEntity, object>> mapToProperty)
		{
			return new SelectBuilder<TEntity>(ContextData.Provider, CreateCommand).Select(sql, mapToProperty);
		}

		public IInsertBuilder Insert(string tableName)
		{
			return new InsertBuilder(ContextData.Provider, CreateCommand, tableName);
		}

		public IInsertBuilder<T> Insert<T>(string tableName, T item)
		{
			return new InsertBuilder<T>(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IInsertBuilderDynamic Insert(string tableName, ExpandoObject item)
		{
			return new InsertBuilderDynamic(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IUpdateBuilder Update(string tableName)
		{
			return new UpdateBuilder(ContextData.Provider, CreateCommand, tableName);
		}

		public IUpdateBuilder<T> Update<T>(string tableName, T item)
		{
			return new UpdateBuilder<T>(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IUpdateBuilderDynamic Update(string tableName, ExpandoObject item)
		{
			return new UpdateBuilderDynamic(ContextData.Provider, CreateCommand, tableName, item);
		}

		public IDeleteBuilder Delete(string tableName)
		{
			return new DeleteBuilder(ContextData.Provider, CreateCommand, tableName);
		}

		public IDeleteBuilder<T> Delete<T>(string tableName, T item)
		{
			return new DeleteBuilder<T>(ContextData.Provider, CreateCommand, tableName, item);
		}

		private void VerifyStoredProcedureSupport()
		{
			if (!ContextData.Provider.SupportsStoredProcedures)
				throw new FluentDataException("The selected database does not support stored procedures.");
		}

		public IStoredProcedureBuilder StoredProcedure(string storedProcedureName)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder(ContextData.Provider, CreateCommand, storedProcedureName);
		}

		public IStoredProcedureBuilder MultiResultStoredProcedure(string storedProcedureName)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder(ContextData.Provider, CreateCommand.UseMultipleResultset, storedProcedureName);
		}

		public IStoredProcedureBuilder<T> StoredProcedure<T>(string storedProcedureName, T item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder<T>(ContextData.Provider, CreateCommand, storedProcedureName, item);
		}

		public IStoredProcedureBuilder<T> MultiResultStoredProcedure<T>(string storedProcedureName, T item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilder<T>(ContextData.Provider, CreateCommand.UseMultipleResultset, storedProcedureName, item);
		}

		public IStoredProcedureBuilderDynamic StoredProcedure(string storedProcedureName, ExpandoObject item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilderDynamic(ContextData.Provider, CreateCommand, storedProcedureName, item);
		}

		public IStoredProcedureBuilderDynamic MultiResultStoredProcedure(string storedProcedureName, ExpandoObject item)
		{
			VerifyStoredProcedureSupport();
			return new StoredProcedureBuilderDynamic(ContextData.Provider, CreateCommand.UseMultipleResultset, storedProcedureName, item);
		}
	}

	public partial class DbContext : IDbContext
	{
		public IDbContext CommandTimeout(int timeout)
		{
			ContextData.CommandTimeout = timeout;
			return this;
		}
	}

	public partial class DbContext : IDbContext
	{
		private void ConnectionStringInternal(string connectionString, DbProviderTypes dbProviderType, IDbProvider dbProvider)
		{
			ContextData.ConnectionString = connectionString;
			ContextData.ProviderType = dbProviderType;
			ContextData.Provider = dbProvider;
		}

		public IDbContext ConnectionString(string connectionString, DbProviderTypes dbProviderType)
		{
			ConnectionStringInternal(connectionString, dbProviderType, new DbProviderFactory().GetDbProvider(dbProviderType));
			return this;
		}

		public IDbContext ConnectionString(string connectionString, IDbProvider dbProvider)
		{
			ConnectionStringInternal(connectionString, DbProviderTypes.Custom, dbProvider);
			return this;
		}

		public IDbContext ConnectionStringName(string connectionstringName, DbProviderTypes dbProviderType)
		{
			ConnectionStringInternal(GetConnectionStringFromConfig(connectionstringName), dbProviderType, new DbProviderFactory().GetDbProvider(dbProviderType));
			return this;
		}

		public IDbContext ConnectionStringName(string connectionstringName, IDbProvider dbProvider)
		{
			ConnectionStringInternal(GetConnectionStringFromConfig(connectionstringName), DbProviderTypes.Custom, dbProvider);
			return this;
		}

		private string GetConnectionStringFromConfig(string connectionStringName)
		{
			var settings = ConfigurationManager.ConnectionStrings[connectionStringName];
			if (settings == null)
				throw new FluentDataException("A connectionstring with the specified name was not found in the .config file");
			return settings.ConnectionString;
		}
	}

	public partial class DbContext : IDbContext
	{
		public IDbContext EntityFactory(IEntityFactory entityFactory)
		{
			ContextData.EntityFactory = entityFactory;
			return this;
		}
	}

	public partial class DbContext : IDbContext
	{
		public IDbContext OnConnectionOpening(Action<OnConnectionOpeningEventArgs> action)
		{
			ContextData.OnConnectionOpening = action;
			return this;
		}

		public IDbContext OnConnectionOpened(Action<OnConnectionOpenedEventArgs> action)
		{
			ContextData.OnConnectionOpened = action;
			return this;
		}

		public IDbContext OnConnectionClosed(Action<OnConnectionClosedEventArgs> action)
		{
			ContextData.OnConnectionClosed = action;
			return this;
		}

		public IDbContext OnExecuting(Action<OnExecutingEventArgs> action)
		{
			ContextData.OnExecuting = action;
			return this;
		}

		public IDbContext OnExecuted(Action<OnExecutedEventArgs> action)
		{
			ContextData.OnExecuted = action;
			return this;
		}

		public IDbContext OnError(Action<OnErrorEventArgs> action)
		{
			ContextData.OnError = action;
			return this;
		}
	}

	public partial class DbContext : IDbContext
	{
		private DbCommand CreateCommand
		{
			get
			{
				IDbConnection connection = null;

				if (ContextData.UseTransaction
					|| ContextData.UseSharedConnection)
				{
					if (ContextData.Connection == null)
						ContextData.Connection = ContextData.Provider.CreateConnection(ContextData.ConnectionString);
					connection = ContextData.Connection;
				}
				else
					connection = ContextData.Provider.CreateConnection(ContextData.ConnectionString);

				var cmd = connection.CreateCommand();
				cmd.Connection = connection;

				return new DbCommand(this, cmd, ContextData);
			}
		}

		public IDbCommand Sql(string sql, params object[] parameters)
		{
			var command = CreateCommand.Sql(sql);
			if (parameters != null)
				command.Parameters(parameters);
			return command;
		}

		public IDbCommand Sql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			var command = CreateCommand.Sql(sql, mappingExpressions);
			
			return command;
		}

		public IDbCommand MultiResultSql()
		{
			return CreateCommand.UseMultipleResultset;
		}

		public IDbCommand MultiResultSql(string sql, params object[] parameters)
		{
			var command = CreateCommand.UseMultipleResultset.Sql(sql);
			if (parameters != null)
				command.Parameters(parameters);
			return command;
		}

		public IDbCommand MultiResultSql<T>(string sql, params Expression<Func<T, object>>[] mappingExpressions)
		{
			var command = CreateCommand.UseMultipleResultset.Sql(sql, mappingExpressions);
			return command;
		}
	}

	public partial class DbContext : IDbContext
	{
		public IDbContext UseTransaction(bool useTransaction)
		{
			ContextData.UseTransaction = useTransaction;
			return this;
		}

		public IDbContext UseSharedConnection(bool useSharedConnection)
		{
			ContextData.UseSharedConnection = useSharedConnection;
			return this;
		}

		public IDbContext IsolationLevel(IsolationLevel isolationLevel)
		{
			ContextData.IsolationLevel = isolationLevel;
			return this;
		}

		public IDbContext Commit()
		{
			VerifyTransactionSupport();

			if (ContextData.TransactionState == TransactionStates.Rollbacked)
				throw new FluentDataException("The transaction has already been rolledback");

			ContextData.Transaction.Commit();
			ContextData.TransactionState = TransactionStates.Committed;
			return this;
		}

		public IDbContext Rollback()
		{
			if (ContextData.TransactionState == TransactionStates.Rollbacked)
				return this;

			VerifyTransactionSupport();

			if (ContextData.TransactionState == TransactionStates.Committed)
				throw new FluentDataException("The transaction has already been commited");

			if (ContextData.Transaction != null)
				ContextData.Transaction.Rollback();
			ContextData.TransactionState = TransactionStates.Rollbacked;
			return this;
		}

		private void VerifyTransactionSupport()
		{
			if (!ContextData.UseTransaction)
				throw new FluentDataException("Transaction support has not been enabled.");
		}
	}

	public enum TransactionStates
	{
		None = 0,
		Committed = 1,
		Rollbacked = 2
	}

	internal class DataReaderHelper
	{
		internal static List<DataReaderField> GetDataReaderFields(IDataReader reader)
		{
			var columns = new List<DataReaderField>();

			for (var i = 0; i < reader.FieldCount; i++)
			{
				var column = new DataReaderField(i, reader.GetName(i), reader.GetFieldType(i));

				if (columns.SingleOrDefault(x => x.LowerName == column.LowerName) == null)
					columns.Add(column);
			}

			return columns;
		}

		internal static object GetDataReaderValue(IDataReader reader, int index, bool isNullable)
		{
			var value = reader[index];
			var type = value.GetType();

			if (value == DBNull.Value)
			{
				if (isNullable)
					return null;

				if (type == typeof(DateTime))
					return DateTime.MinValue;
			}

			return value;
		}
	}

	public class PropertyExpressionParser<T>
	{
		private readonly object _item;
		private PropertyInfo _property;

		public PropertyExpressionParser(object item, Expression<Func<T, object>> propertyExpression)
		{
			_item = item;
			_property = GetProperty(propertyExpression);
		}

		private static PropertyInfo GetProperty<T>(Expression<Func<T, object>> exp)
		{
			PropertyInfo result;
			if (exp.Body.NodeType == ExpressionType.Convert)
				result = ((MemberExpression) ((UnaryExpression) exp.Body).Operand).Member as PropertyInfo;
			else result = ((MemberExpression) exp.Body).Member as PropertyInfo;

			if (result != null)
				return typeof(T).GetProperty(result.Name);

			throw new ArgumentException(string.Format("Expression '{0}' does not refer to a property.", exp.ToString()));
		}
		
		public object Value
		{
			get { return ReflectionHelper.GetPropertyValue(_item, _property); }
		}

		public string Name
		{
			get { return _property.Name; }
		}

		public Type Type
		{
			get { return ReflectionHelper.GetPropertyType(_property); }
		}
	}

	public class FluentDataException : Exception
	{
		public FluentDataException(string message)
			: base(message)
		{
		}
		public FluentDataException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}

	internal static class ReflectionHelper
	{
		private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _cachedProperties = new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();

		public static string GetPropertyNameFromExpression<T>(Expression<Func<T, object>> expression)
		{
			string propertyPath = null;
			if (expression.Body is UnaryExpression)
			{
				var unaryExpression = (UnaryExpression) expression.Body;
				if (unaryExpression.NodeType == ExpressionType.Convert)
					propertyPath = unaryExpression.Operand.ToString();
			}

			if (propertyPath == null)
				propertyPath = expression.Body.ToString();

			propertyPath = propertyPath.Replace(expression.Parameters[0] + ".", string.Empty);

			return propertyPath;
		}

		public static List<string> GetPropertyNamesFromExpressions<T>(Expression<Func<T, object>>[] expressions)
		{
			var propertyNames = new List<string>();
			foreach (var expression in expressions)
			{
				var propertyName = GetPropertyNameFromExpression(expression);
				propertyNames.Add(propertyName);
			}
			return propertyNames;
		}

		public static object GetPropertyValue(object item, PropertyInfo property)
		{
			var value = property.GetValue(item, null);

			if (property.PropertyType.IsEnum)
				return (int) value;

			return value;
		}

		public static object GetPropertyValue(object item, string propertyName)
		{
			PropertyInfo property;
			foreach (var part in propertyName.Split('.'))
			{
				if (item == null)
					return null;

				var type = item.GetType();

				property = type.GetProperty(part);
				if (property == null)
					return null;

				item = GetPropertyValue(item, property);
			}
			return item;
		}

		public static object GetPropertyValueDynamic(object item, string name)
		{
			var dictionary = (IDictionary<string, object>) item;

			return dictionary[name];
		}

		public static Dictionary<string, PropertyInfo> GetProperties(Type type)
		{
			var properties = _cachedProperties.GetOrAdd(type, BuildPropertyDictionary);

			return properties;
		}

		private static Dictionary<string, PropertyInfo> BuildPropertyDictionary(Type type)
		{
			var result = new Dictionary<string, PropertyInfo>();

			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				result.Add(property.Name.ToLower(), property);
			}
			return result;
		}

		public static bool IsList(object item)
		{
			if (item is ICollection)
				return true;

			return false;
		}

		public static bool IsNullable(PropertyInfo property)
		{
			if (property.PropertyType.IsGenericType &&
				property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				return true;

			return false;
		}

		/// <summary>
		/// Includes a work around for getting the actual type of a Nullable type.
		/// </summary>
		public static Type GetPropertyType(PropertyInfo property)
		{
			if (IsNullable(property))
				return property.PropertyType.GetGenericArguments()[0];

			return property.PropertyType;
		}

		public static object GetDefault(Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance(type);
			return null;
		}

		public static bool IsBasicClrType(Type type)
		{
			if (type.IsEnum
				|| type.IsPrimitive
				|| type.IsValueType
				|| type == typeof(string)
				|| type == typeof(DateTime))
				return true;

			return false;
		}
	}

	internal class AccessProvider : IDbProvider
	{
		public string ProviderName
		{
			get
			{
				return "System.Data.OleDb";
			}
		}

		public bool SupportsOutputParameters
		{
			get { return false; }
		}

		public bool SupportsMultipleQueries
		{
			get { return false; }
		}

		public bool SupportsMultipleResultset
		{
			get { return false; }
		}

		public bool SupportsStoredProcedures
		{
			get { return false; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return true; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public void FixInStatement(StringBuilder sql, ParameterCollection parameters)
		{
			new FixSqlInStatement().FixPotentialInSql(this, sql, parameters);
		}

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = HandleExecuteReturnLastId<T>(data);
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
			
		}

		private T HandleExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			int recordsAffected = data.InnerCommand.ExecuteNonQuery();

			T lastId = default(T);

			if (recordsAffected > 0)
			{
				data.InnerCommand.CommandText = "select @@Identity";

				var value = data.InnerCommand.ExecuteScalar();

				lastId = (T) value;
			}

			return lastId;
		}
	}

	internal class ConnectionFactory
	{
		public static IDbConnection CreateConnection(string providerName, string connectionString)
		{
			var factory = DbProviderFactories.GetFactory(providerName);

			var connection = factory.CreateConnection();
			connection.ConnectionString = connectionString;
			return connection;
		}
	}

	internal class DbTypeMapper
	{
		private static Dictionary<Type, DataTypes> _types;
		private static readonly object _locker = new object();

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			if (_types == null)
			{
				lock (_locker)
				{
					if (_types == null)
					{
						_types = new Dictionary<Type, DataTypes>();

						_types.Add(typeof(Int16), DataTypes.Int16);
						_types.Add(typeof(Int32), DataTypes.Int32);
						_types.Add(typeof(Int64), DataTypes.Int64);
						_types.Add(typeof(string), DataTypes.String);
						_types.Add(typeof(DateTime), DataTypes.DateTime);
						_types.Add(typeof(XDocument), DataTypes.Xml);
						_types.Add(typeof(decimal), DataTypes.Decimal);
						_types.Add(typeof(Guid), DataTypes.Guid);
						_types.Add(typeof(Boolean), DataTypes.Boolean);
						_types.Add(typeof(char), DataTypes.String);
					}
				}
			}

			if (!_types.ContainsKey(clrType))
				return DataTypes.Object;

			return _types[clrType];
		}
	}

	internal class DeleteBuilderSqlGenerator
	{
		public string GenerateSql(string parameterPrefix, BuilderData data)
		{
			var whereSql = "";
			foreach (var column in data.Columns)
			{
				if (whereSql.Length > 0)
					whereSql += " and ";

				whereSql += string.Format("{0} = {1}{2}",
												column.ColumnName,
												parameterPrefix,
												column.ParameterName);
			}

			var sql = string.Format("delete from {0} where {1}", data.ObjectName, whereSql);
			return sql;
		}
	}

	internal class FixSqlInStatement
	{
		public void FixPotentialInSql(IDbProvider provider, StringBuilder sql, ParameterCollection parameters)
		{
			int i = -1;
			while (true)
			{
				i++;
				if (i == parameters.Count)
					break;

				var parameter = parameters[i];

				if (parameter.Direction == ParameterDirection.Output
					|| parameter.DataTypes != DataTypes.Object)
					continue;

				if (ReflectionHelper.IsList(parameter.Value))
				{
					var oldListParameterName = parameter.ParameterName;
					var list = (IEnumerable) parameter.Value;
					
					var newInStatement = new StringBuilder();

					int k = -1;
					foreach (var item in list)
					{
						k++;
						if (k == 0)
						{
							parameter.ParameterName = "p" + parameter.ParameterName + "p0";
							newInStatement.Append(" in(" + provider.GetParameterName(parameter.ParameterName));
							parameter.Value = item;
						}
						else
						{
							var newParameter = new Parameter();
							newParameter.ParameterName = "p" + oldListParameterName + "p" + k.ToString();
							newParameter.Value = item;
							newParameter.DataTypes = DataTypes.Object;
							newParameter.Direction = parameter.Direction;

							parameters.Insert(k, newParameter);

							newInStatement.Append("," + newParameter.GetParameterName(provider));
						}
					}
					newInStatement.Append(")");

					var oldInStatement = string.Format(" in({0})", provider.GetParameterName(oldListParameterName));
					sql.Replace(oldInStatement, newInStatement.ToString());
				}
			}
		}
	}

	internal class InsertBuilderSqlGenerator
	{
		public string GenerateSql(string parameterPrefix, BuilderData data)
		{
			var insertSql = "";
			var valuesSql = "";
			foreach (var column in data.Columns)
			{
				if (insertSql.Length > 0)
				{
					insertSql += ",";
					valuesSql += ",";
				}

				insertSql += column.ColumnName;
				valuesSql += parameterPrefix + column.ParameterName;
			}

			var sql = string.Format("insert into {0}({1}) values({2})",
										data.ObjectName,
										insertSql,
										valuesSql);
			return sql;
		}
	}

	internal class UpdateBuilderSqlGenerator
	{
		public string GenerateSql(string parameterPrefix, BuilderData data)
		{
			var setSql = "";
			foreach (var column in data.Columns)
			{
				if (setSql.Length > 0)
					setSql += ", ";

				setSql += string.Format("{0} = {1}{2}",
									column.ColumnName,
									parameterPrefix,
									column.ParameterName);
			}

			var whereSql = "";
			foreach (var column in data.Where)
			{
				if (whereSql.Length > 0)
					whereSql += " and ";

				whereSql += string.Format("{0} = {1}{2}",
									column.ColumnName,
									parameterPrefix,
									column.ParameterName);
			}

			var sql = string.Format("update {0} set {1} where {2}",
										data.ObjectName,
										setSql,
										whereSql);
			return sql;
		}
	}

	internal class MySqlProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "MySql.Data.MySqlClient";
			} 
		}
		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultset
		{
			get { return true; }
		}

		public bool SupportsMultipleQueries
		{
			get { return true; }
		}

		public bool SupportsStoredProcedures
		{
			get { return true; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return true; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			return data.ObjectName;
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public void FixInStatement(StringBuilder sql, ParameterCollection parameters)
		{
			new FixSqlInStatement().FixPotentialInSql(this, sql, parameters);
		}

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			if (data.Sql[data.Sql.Length - 1] != ';')
				data.Sql.Append(';');

			data.Sql.Append("select LAST_INSERT_ID() as `LastInsertedId`");

			T lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				object value = data.InnerCommand.ExecuteScalar();

				if (value.GetType() == typeof(T))
					lastId = (T) value;

				lastId = (T) Convert.ChangeType(value, typeof(T));
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
		}
	}

	internal class DbProviderFactory
	{
		public virtual IDbProvider GetDbProvider(DbProviderTypes dbProvider)
		{
			IDbProvider provider = null;
			switch (dbProvider)
			{
				case DbProviderTypes.SqlServer:
				case DbProviderTypes.SqlAzure:
					provider = new SqlServerProvider();
					break;
				case DbProviderTypes.SqlServerCompact40:
					provider = new SqlServerCompactProvider();
					break;
				case DbProviderTypes.Oracle:
					provider = new OracleProvider();
					break;
				case DbProviderTypes.MySql:
					provider = new MySqlProvider();
					break;
				case DbProviderTypes.Access:
					provider = new AccessProvider();
					break;
			}

			return provider;
		}
	}

	public enum DbProviderTypes
	{
		Custom = 0,
		SqlServer = 1,
		SqlServerCompact40 = 2,
		SqlAzure = 3,
		Oracle = 4,
		MySql = 5,
		Access = 6
	}

	public interface IDbProvider
	{
		string ProviderName { get; }
		bool SupportsMultipleResultset { get; }
		bool SupportsMultipleQueries { get; }
		bool SupportsOutputParameters { get; }
		bool SupportsStoredProcedures { get; }
		bool SupportsExecuteReturnLastIdWithNoIdentityColumn { get; }
		IDbConnection CreateConnection(string connectionString);
		string GetParameterName(string parameterName);
		string GetSelectBuilderAlias(string name, string alias);
		string GetSqlForSelectBuilder(BuilderData data);
		string GetSqlForInsertBuilder(BuilderData data);
		string GetSqlForUpdateBuilder(BuilderData data);
		string GetSqlForDeleteBuilder(BuilderData data);
		string GetSqlForStoredProcedureBuilder(BuilderData data);
		DataTypes GetDbTypeForClrType(Type clrType);
		void FixInStatement(StringBuilder sql, ParameterCollection parameters);
		T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName);
		void OnCommandExecuting(DbCommandData data);
	}

	internal class OracleProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "Oracle.DataAccess.Client";
			} 
		}

		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultset
		{
			get { return false; }
		}

		public bool SupportsMultipleQueries
		{
			get { return true; }
		}

		public bool SupportsStoredProcedures
		{
			get { return true; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return false; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return ":" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql(":", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql(":", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql(":", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			return data.ObjectName;
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public void FixInStatement(StringBuilder sql, ParameterCollection parameters)
		{
			new FixSqlInStatement().FixPotentialInSql(this, sql, parameters);
		}

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			data.Command.ParameterOut("FluentDataLastInsertedId", data.ContextData.Provider.GetDbTypeForClrType(typeof(T)));
			data.Sql.Append(string.Format(" returning {0} into :FluentDataLastInsertedId", identityColumnName));

			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				data.InnerCommand.ExecuteNonQuery();

				var parameter = (IDbDataParameter) data.InnerCommand.Parameters[":FluentDataLastInsertedId"];
				lastId = (T) parameter.Value;
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
			if (data.InnerCommand.CommandType == CommandType.Text)
			{
				dynamic innerCommand = data.InnerCommand;
				innerCommand.BindByName = true;
			}
		}
	}

	internal class SqlServerCompactProvider : IDbProvider
	{
		public string ProviderName
		{
			get
			{
				return "System.Data.SqlServerCe.4.0";
			}
		}

		public bool SupportsOutputParameters
		{
			get { return false; }
		}

		public bool SupportsMultipleQueries
		{
			get { return false; }
		}

		public bool SupportsMultipleResultset
		{
			get { return false; }
		}

		public bool SupportsStoredProcedures
		{
			get { return false; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return true; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			throw new NotImplementedException();
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public void FixInStatement(StringBuilder sql, ParameterCollection parameters)
		{
			new FixSqlInStatement().FixPotentialInSql(this, sql, parameters);
		}

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			var lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				lastId = HandleExecuteReturnLastId<T>(data);
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
			
		}

		private T HandleExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			int recordsAffected = data.InnerCommand.ExecuteNonQuery();

			T lastId = default(T);

			if (recordsAffected > 0)
			{
				data.InnerCommand.CommandText = "select cast(@@identity as int)";

				var value = data.InnerCommand.ExecuteScalar();

				lastId = (T) value;
			}

			return lastId;
		}
	}

	internal class SqlServerProvider : IDbProvider
	{
		public string ProviderName
		{ 
			get
			{
				return "System.Data.SqlClient";
			} 
		}

		public bool SupportsOutputParameters
		{
			get { return true; }
		}

		public bool SupportsMultipleResultset
		{
			get { return true; }
		}

		public bool SupportsMultipleQueries
		{
			get { return true; }
		}

		public bool SupportsStoredProcedures
		{
			get { return true; }
		}

		public bool SupportsExecuteReturnLastIdWithNoIdentityColumn
		{
			get { return true; }
		}

		public IDbConnection CreateConnection(string connectionString)
		{
			return ConnectionFactory.CreateConnection(ProviderName, connectionString);
		}

		public string GetParameterName(string parameterName)
		{
			return "@" + parameterName;
		}

		public string GetSelectBuilderAlias(string name, string alias)
		{
			return name + " as " + alias;
		}

		public string GetSqlForSelectBuilder(BuilderData data)
		{
			var sql = "";
			if (data.PagingItemsPerPage == 0)
			{
				sql = "select " + data.Select;
				sql += " from " + data.From;
				if (data.WhereSql.Length > 0)
					sql += " where " + data.WhereSql;
				if (data.GroupBy.Length > 0)
					sql += " group by " + data.GroupBy;
				if (data.Having.Length > 0)
					sql += " having " + data.Having;
				if (data.OrderBy.Length > 0)
					sql += " order by " + data.OrderBy;
			}
			else if (data.PagingItemsPerPage > 0)
			{
				sql += " from " + data.From;
				if (data.WhereSql.Length > 0)
					sql += " where " + data.WhereSql;
				if (data.GroupBy.Length > 0)
					sql += " group by " + data.GroupBy;
				if (data.Having.Length > 0)
					sql += " having " + data.Having;

				sql = string.Format(@"with PagedPersons as
										(
											select top 100 percent {0}, row_number() over (order by {1}) as fluentdata_RowNumber
											{2}
										)
										select *
										from PagedPersons
										where fluentdata_RowNumber between {3} and {4}",
											data.Select,
											data.OrderBy,
											sql,
											data.GetFromItems(),
											data.GetToItems());
			}

			return sql;
		}

		public string GetSqlForInsertBuilder(BuilderData data)
		{
			return new InsertBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForUpdateBuilder(BuilderData data)
		{
			return new UpdateBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForDeleteBuilder(BuilderData data)
		{
			return new DeleteBuilderSqlGenerator().GenerateSql("@", data);
		}

		public string GetSqlForStoredProcedureBuilder(BuilderData data)
		{
			return data.ObjectName;
		}

		public DataTypes GetDbTypeForClrType(Type clrType)
		{
			return new DbTypeMapper().GetDbTypeForClrType(clrType);
		}

		public void FixInStatement(StringBuilder sql, ParameterCollection parameters)
		{
			new FixSqlInStatement().FixPotentialInSql(this, sql, parameters);
		}

		public T ExecuteReturnLastId<T>(DbCommandData data, string identityColumnName = null)
		{
			if (data.Sql[data.Sql.Length - 1] != ';')
				data.Sql.Append(';');

			data.Sql.Append("select SCOPE_IDENTITY()");

			T lastId = default(T);

			data.ExecuteQueryHandler.ExecuteQuery(false, () =>
			{
				object value = data.InnerCommand.ExecuteScalar();

				if (value.GetType() == typeof(T))
					lastId = (T) value;

				lastId = (T) Convert.ChangeType(value, typeof(T));
			});

			return lastId;
		}

		public void OnCommandExecuting(DbCommandData data)
		{
		}
	}

}

