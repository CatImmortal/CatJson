using System;

namespace CatJson
{
    public abstract unsafe class BaseUnmanagedJsonFormatter<TValue> : IUnmanagedJsonFormatter,IJsonFormatter where TValue:unmanaged
    {
        void IUnmanagedJsonFormatter.ToJson(void* value, Type type, Type realType, int depth)
        {
            ToJson(*(TValue*)value,type,realType, depth);
        }

        void IJsonFormatter.ToJson(object value, Type type, Type realType, int depth)
        {
            ToJson((TValue)value,type,realType,depth);
        }

        object IJsonFormatter.ParseJson(Type type, Type realType)
        {
            return ParseJson(type, realType);
        }
        
        /// <summary>
        /// 将对象序列化为Json文本
        /// </summary>
        public abstract void ToJson(TValue value, Type type, Type realType, int depth);
        
        /// <summary>
        /// 将Json文本反序列化为对象
        /// </summary>
        public abstract TValue ParseJson(Type type, Type realType);
    }
}