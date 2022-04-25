
public abstract class BaseJsonFormatter<TValue> : IJsonFormatter
{

    void IJsonFormatter.ToJson(object value, int depth)
    {
        ToJson((TValue)value, depth);
    }

    object IJsonFormatter.ParseJson()
    {
        return ParseJson();
    }
    
    public abstract void ToJson(TValue value,int depth);
    public abstract TValue ParseJson();
}