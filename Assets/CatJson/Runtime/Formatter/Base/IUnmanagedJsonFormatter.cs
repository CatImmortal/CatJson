using System;

namespace CatJson
{
    public unsafe interface IUnmanagedJsonFormatter
    {
        /// <summary>
        /// 将对象序列化为Json文本
        /// </summary>
        void ToJson(void* value, Type type, Type realType, int depth);
    }
}