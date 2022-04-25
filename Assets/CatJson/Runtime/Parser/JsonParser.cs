using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public static Dictionary<Type, IJsonFormatter> FormatterDict = new Dictionary<Type, IJsonFormatter>()
        {
            {typeof(bool), new BooleanFormatter()},
            {typeof(double), new DoubleFormatter()},
            {typeof(string), new StringFormatter()},
            {typeof(int), new Int32Formatter()},
            
            {typeof(JsonObject), new JsonObjectFormatter()},
            {typeof(JsonValue), new JsonValueFormatter()},
            
            {typeof(Array), new ArrayFormatter()},
        };

        /// <summary>
        /// 解析Json键值对的通用流程
        /// </summary>
        public static void ParseJsonKeyValuePairProcedure(object userdata,Action<object,RangeString> action)
        {

            //跳过 {
            Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                RangeString key = Lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                Lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                action(userdata,key);

                //此时value已经被提取了
                
                //有逗号就跳过逗号
                if (Lexer.LookNextTokenType() == TokenType.Comma)
                {
                    Lexer.GetNextTokenByType(TokenType.Comma);

                    if (Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("Json对象不能以逗号结尾");
                    }
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }

            }

            //跳过 }
            Lexer.GetNextTokenByType(TokenType.RightBrace);
        }

        /// <summary>
        /// 解析Json数组的通用流程
        /// </summary>
        public static void ParseJsonArrayProcedure(object userdata1,object userdata2, Action<object,object> action)
        {
            //跳过[
            Lexer.GetNextTokenByType(TokenType.LeftBracket);

            while (Lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                action(userdata1,userdata2);

                //此时value已经被提取了
                
                //有逗号就跳过
                if (Lexer.LookNextTokenType() == TokenType.Comma)
                {
                    Lexer.GetNextTokenByType(TokenType.Comma);

                    if (Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("数组不能以逗号结尾");
                    }
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }
            }

            //跳过]
            Lexer.GetNextTokenByType(TokenType.RightBracket);
        }
        
        public static string ToJson<T>(T obj)
        {
            InternalToJson(obj);
            
            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();
            
            return json;
        }

        public static string ToJson(object obj,Type type)
        {
            InternalToJson(obj,type);
            
            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();
            
            return json;
        }

        internal static void InternalToJson<T>(T obj, int depth = 0)
        {
            InternalToJson(obj,typeof(T),depth);
        }
        
        internal static void InternalToJson(object obj,Type type,int depth = 0)
        {
            if (FormatterDict.TryGetValue(type,out IJsonFormatter formatter))
            {
                formatter.ToJson(obj,depth);
            }
            else
            {
                //数组要特殊处理
                if (obj is Array array)
                {
                    ArrayFormatter arrayFormatter = (ArrayFormatter)FormatterDict[typeof(Array)];
                    arrayFormatter.ToJson(array,depth);
                }
            }
        }
        
        
        public static T ParseJson<T>(string json)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson<T>();
        }
        
        public static object ParseJson(string json,Type type)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson(type);
        }
        
        
        internal static T InternalParseJson<T>()
        {
            return (T)InternalParseJson(typeof(T));
        }
        
        internal static object InternalParseJson(Type type)
        {
            object result = null;
            if (FormatterDict.TryGetValue(type,out IJsonFormatter formatter))
            {
                result = formatter.ParseJson();

            }
            else
            {
                //数组要特殊处理
                if (type.IsArray)
                {
                    ArrayFormatter arrayFormatter = (ArrayFormatter)FormatterDict[typeof(Array)];
                    arrayFormatter.ElementType = TypeUtil.GetArrayOrListElementType(type);
                    result = arrayFormatter.ParseJson();
                }
            }

            return result;
        }
        
    }

}
