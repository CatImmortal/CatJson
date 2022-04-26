using System.Collections.Generic;
using  System;

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



        public static string ToJson<T>(T obj)
        {
            InternalToJson<T>(obj);

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();

            return json;
        }

        public static string ToJson(object obj, Type type)
        {
            InternalToJson(obj, type);

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();

            return json;
        }
        
        public static T ParseJson<T>(string json)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson<T>();
        }

        public static object ParseJson(string json, Type type)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson(type);
        }

        internal static void InternalToJson<T>(T obj, int depth = 0)
        {
            InternalToJson(obj, typeof(T), depth);
        }
        
        internal static T InternalParseJson<T>()
        {
            return (T) InternalParseJson(typeof(T));
        }

        internal static void InternalToJson(object obj, Type type, int depth = 0)
        {
            if (obj is null)
            {
                nullFormatter.ToJson(null,type, depth);
                return;
            }
            
            
            if (formatterDict.TryGetValue(type, out IJsonFormatter formatter))
            {
                formatter.ToJson(obj,type, depth);
                return;
            }

            if (type.IsGenericType && formatterDict.TryGetValue(type.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                formatter.ToJson(obj,type,depth);
                return;
            }
            
            if (obj is Array array)
            {
                //特殊处理数组
                arrayFormatter.ToJson(array,type, depth);
                return;
            }
            
            //使用处理自定义类的formatter
            defaultFormatter.ToJson(obj,type,depth);
        }
        
        internal static object InternalParseJson(Type type)
        {
            if (Lexer.LookNextTokenType() == TokenType.Null)
            {
                return nullFormatter.ParseJson(type);
            }

            object result;
            
            if (formatterDict.TryGetValue(type, out IJsonFormatter formatter))
            {
               result = formatter.ParseJson(type);
               return result;
            }
            
            if (type.IsGenericType &&  formatterDict.TryGetValue(type.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                result = formatter.ParseJson(type);
                return result;
            }
            
            if (type.IsArray)
            {
                //特殊处理数组
                result = arrayFormatter.ParseJson(type);
                return result;
 
            }
            
            //使用处理自定义类的formatter
            result = defaultFormatter.ParseJson(type);
            return result;
        }

    }

}
