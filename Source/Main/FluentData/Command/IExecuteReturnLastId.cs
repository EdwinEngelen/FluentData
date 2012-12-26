namespace FluentData
{
    public interface IExecuteReturnLastId
    {
        T ExecuteReturnLastId<T>(string identityColumnName = null);        
    }
}