namespace FluentData
{
    public interface IParameterValue
    {
        TParameterType ParameterValue<TParameterType>(string outputParameterName);        
    }
}