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
        public static JsonLexer Lexer = new JsonLexer();

        /// <summary>
        /// 序列化时是否开启格式化
        /// </summary>
        public static bool IsFormat { get; set; } = true;

        private static NullFormatter nullFormatter = new NullFormatter();
        private static ArrayFormatter arrayFormatter = new ArrayFormatter();
        private static DictionaryFormatter dictionaryFormatter = new DictionaryFormatter();

        public static Dictionary<Type, IJsonFormatter> FormatterDict = new Dictionary<Type, IJsonFormatter>()
        {
            {typeof(bool), new BooleanFormatter()},
            {typeof(double), new DoubleFormatter()},
            {typeof(string), new StringFormatter()},
            {typeof(int), new Int32Formatter()},

            {typeof(JsonObject), new JsonObjectFormatter()},
            {typeof(JsonValue), new JsonValueFormatter()},

            {typeof(Dictionary<,>), new DictionaryFormatter()}
        };



        public static string ToJson<T>(T obj)
        {
            InternalToJson(obj);

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
            
            
            if (FormatterDict.TryGetValue(type, out IJsonFormatter formatter))
            {
                formatter.ToJson(obj,type, depth);
                return;
            }
            
            if (obj is Array array)
            {
                //特殊处理数组
                arrayFormatter.ToJson(array,type, depth);
                return;
            }
            
            if (type.IsGenericType && FormatterDict.TryGetValue(type.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                formatter.ToJson(obj,type,depth);
                return;
            }

            throw new Exception($"未找到{type}类型的Json格式化器");
        }
        
        internal static object InternalParseJson(Type type)
        {
            if (Lexer.LookNextTokenType() == TokenType.Null)
            {
                return nullFormatter.ParseJson(type);
            }

            if (FormatterDict.TryGetValue(type, out IJsonFormatter formatter))
            {
               object result = formatter.ParseJson(type);
               return result;
            }
            
            if (type.IsGenericType &&  FormatterDict.TryGetValue(type.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                object result = formatter.ParseJson(type);
                return result;
            }
            
            if (type.IsArray)
            {
                //特殊处理数组
                object result = arrayFormatter.ParseJson(type);
                return result;
            }
            
            throw new Exception($"未找到{type}类型的Json格式化器");
        }

    }

}
