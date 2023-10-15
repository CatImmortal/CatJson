using System;

namespace CatJson
{
    public partial class JsonParser
    {
        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        public string ToJson<T>(T obj)
        {
            InternalToJson(obj, typeof(T));

            string json = CachedSB.ToString();
            CachedSB.Clear();

            return json;
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        public string ToJson(object obj, Type type)
        {
            InternalToJson(obj, type);

            string json = CachedSB.ToString();
            CachedSB.Clear();

            return json;
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        internal void InternalToJson(object obj, Type type, int depth = 1)
        {
            if (obj == null)
            {
                nullFormatter.ToJson(this, null, null, depth);
                return;
            }

            if (obj is IJsonParserCallbackReceiver receiver)
            {
                //触发序列化开始回调
                receiver.OnToJsonStart();
            }

            if (IsPolymorphic)
            {
                //开启了多态序列化检测
                Type realType = TypeUtil.GetType(obj);
                if (!TypeUtil.TypeEquals(type, realType))
                {
                    //并且定义类型和真实类型不一致
                    //就要进行多态序列化
                    polymorphicFormatter.ToJson(this, obj, realType, depth);
                    return;
                }
            }

            if (!type.IsGenericType)
            {
                if (formatterDict.TryGetValue(type, out IJsonFormatter formatter))
                {
                    //使用通常的formatter处理
                    formatter.ToJson(this, obj, type, depth);
                    return;
                }
            }
            else
            {
                if (formatterDict.TryGetValue(type.GetGenericTypeDefinition(), out IJsonFormatter formatter))
                {
                    //使用泛型类型formatter处理
                    formatter.ToJson(this, obj, type, depth);
                    return;
                }
            }

            if (obj is Enum e)
            {
                //使用枚举formatter处理
                enumFormatter.ToJson(this, e, type, depth);
                return;
            }

            if (obj is Array array)
            {
                //使用数组formatter处理
                arrayFormatter.ToJson(this, array, type, depth);
                return;
            }


            //使用反射formatter处理
            reflectionFormatter.ToJson(this, obj, type, depth);
        }
    }
}