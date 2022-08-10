using System;

namespace CatJson
{
    /// <summary>
    /// 序列化此类型下保持默认值的字段/属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonCareDefaultValueAttribute : Attribute
    {
        
    }
}