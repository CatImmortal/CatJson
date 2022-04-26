using System.Collections.Generic;
using  System;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Json解析器
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Json词法分析器
        /// </summary>
        public static readonly JsonLexer Lexer = new JsonLexer();

        /// <summary>
        /// 序列化时是否开启格式化
        /// </summary>
        public static bool IsFormat { get; set; } = true;

        private static readonly NullFormatter nullFormatter = new NullFormatter();
        private static readonly ArrayFormatter arrayFormatter = new ArrayFormatter();
        private static readonly DefaultFormatter defaultFormatter = new DefaultFormatter();
        private static readonly PolymorphicFormatter polymorphicFormatter = new PolymorphicFormatter();
        
        /// <summary>
        /// Json格式化器字典
        /// </summary>
        private static readonly Dictionary<Type, IJsonFormatter> formatterDict = new Dictionary<Type, IJsonFormatter>()
        {
            //基元类型
            {typeof(bool), new BooleanFormatter()},
            {typeof(int), new Int32Formatter()},
            {typeof(float), new SingleFormatter()},
            {typeof(double), new DoubleFormatter()},
            {typeof(string), new StringFormatter()},
            
            //容器类型
            {typeof(List<>), new ListFormatter()},
            {typeof(Dictionary<,>), new DictionaryFormatter()},
            
            //特殊类型
            {typeof(JsonObject), new JsonObjectFormatter()},
            {typeof(JsonValue), new JsonValueFormatter()},
        };


        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        public static string ToJson<T>(T obj)
        {
            InternalToJson<T>(obj);

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();

            return json;
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        public static string ToJson(object obj, Type type)
        {
            InternalToJson(obj, type);

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();

            return json;
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        public static T ParseJson<T>(string json)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson<T>();
        }

        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        public static object ParseJson(string json, Type type)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson(type);
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        internal static void InternalToJson<T>(T obj, int depth = 0)
        {
            InternalToJson(obj, typeof(T),null, depth);
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        internal static T InternalParseJson<T>()
        {
            return (T) InternalParseJson(typeof(T));
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        internal static void InternalToJson(object obj, Type type, Type realType = null, int depth = 0,bool isPolymorphicCheck = true)
        {
            if (obj is null)
            {
                nullFormatter.ToJson(null,type,null, depth);
                return;
            }

            if (realType == null)
            {
                realType = TypeUtil.GetType(obj);
            }
            
            if (isPolymorphicCheck && !TypeUtil.TypeEquals(type,realType))
            {
                //开启了多态序列化检测
                //只要定义类型和真实类型不一致，就要进行多态序列化
                polymorphicFormatter.ToJson(obj,type,realType,depth);
                return;;
            }
            
            if (formatterDict.TryGetValue(realType, out IJsonFormatter formatter))
            {
                formatter.ToJson(obj,type,realType, depth);
                return;
            }

            if (realType.IsGenericType && formatterDict.TryGetValue(realType.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                formatter.ToJson(obj,type,realType,depth);
                return;
            }
            
            if (obj is Array array)
            {
                //特殊处理数组
                arrayFormatter.ToJson(array,type,realType, depth);
                return;
            }
            
            //使用处理自定义类的formatter
            defaultFormatter.ToJson(obj,type,realType,depth);
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        internal static object InternalParseJson(Type type,Type realType = null)
        {
            if (Lexer.LookNextTokenType() == TokenType.Null)
            {
                return nullFormatter.ParseJson(type,null);
            }

            object result;

            //这段多态序列化处理有点复杂
            //首先根据是否能读取到realTypeKey来判断是否进行多态处理
            //如果没读取到，并且调用者没传入realType参数，那么就将type作为realType来使用
            //如果读取到了，就将读取到的tempRealType作为realType使用，并且调用多态处理
            if (!ParserHelper.TryParseRealType(type,out Type tempRealType))
            {
                //未进行多态序列化
                if (realType == null)
                {
                    //未传入realType，使用type作为realType
                    realType = type;
                }

            }
            else
            {
                //进行了多态序列化
                //处理多态
                realType = tempRealType;

                result = polymorphicFormatter.ParseJson(type, realType);
                return result;
            }

            if (formatterDict.TryGetValue(realType, out IJsonFormatter formatter))
            {
               result = formatter.ParseJson(type,realType);
               return result;
            }
            
            if (realType.IsGenericType &&  formatterDict.TryGetValue(realType.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                result = formatter.ParseJson(type,realType);
                return result;
            }
            
            if (realType.IsArray)
            {
                //特殊处理数组
                result = arrayFormatter.ParseJson(type,realType);
                return result;
 
            }
            
            //使用处理自定义类的formatter
            result = defaultFormatter.ParseJson(type,realType);
            return result;
        }

    }

}
