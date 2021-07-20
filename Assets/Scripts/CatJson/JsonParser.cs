using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CatJson
{
    /// <summary>
    /// json解析器
    /// </summary>
    public static class JsonParser
    {
        /// <summary>
        /// Json词法分析器
        /// </summary>
        private static JsonLexer lexer = new JsonLexer();

        private static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoMap = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        private static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoMap = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        /// <summary>
        /// 解析JsonObject的通用流程
        /// </summary>
        private static void ParseJsonObjectProcedure(object userdata1,object userdata2,Action<object,object,string, TokenType> action)
        {
            //跳过 {
            lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                string key = lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();

                action(userdata1,userdata2,key, nextTokenType);

                //有逗号就跳过逗号
                if (lexer.LookNextTokenType() == TokenType.Comma)
                {
                    lexer.GetNextTokenByType(TokenType.Comma);

                    if (lexer.LookNextTokenType() == TokenType.RightBracket)
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
            lexer.GetNextTokenByType(TokenType.RightBrace);
        }

        /// <summary>
        /// 解析Json数组的通用流程
        /// </summary>
        private static void ParseJsonArrayProcedure(object userdata1,object userdata2, Action<object,object,TokenType> action)
        {
            //跳过[
            lexer.GetNextTokenByType(TokenType.LeftBracket);

            while (lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();

                action(userdata1,userdata2,nextTokenType);

                //有逗号就跳过
                if (lexer.LookNextTokenType() == TokenType.Comma)
                {
                    lexer.GetNextTokenByType(TokenType.Comma);

                    if (lexer.LookNextTokenType() == TokenType.RightBracket)
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
            lexer.GetNextTokenByType(TokenType.RightBracket);
        }

        /// <summary>
        /// 将type的公有实例字段和属性都放入字典中缓存
        /// </summary>
        private static void AddToReflectionMap(Type type)
        {
            PropertyInfo[] pis = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<string, PropertyInfo> dict1 = null;
            if (pis.Length > 0)
            {
                dict1 = new Dictionary<string, PropertyInfo>(pis.Length);
                for (int i = 0; i < pis.Length; i++)
                {
                    PropertyInfo pi = pis[i];
                    dict1.Add(pi.Name, pi);
                }
                
            }
            propertyInfoMap.Add(type, dict1);

            FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<string, FieldInfo> dict2 = null;
            if (fis.Length > 0)
            {
                dict2 = new Dictionary<string, FieldInfo>(fis.Length);
                for (int i = 0; i < fis.Length; i++)
                {
                    FieldInfo fi = fis[i];
                    dict2.Add(fi.Name, fi);
                }

            }
            fieldInfoMap.Add(type, dict2);
        }

        /// <summary>
        /// 解析json文本为Json对象
        /// </summary>
        public static JsonObject ParseJson(string json)
        {
            lexer.SetJsonText(json);

            return ParseJsonObject();
        }

        /// <summary>
        /// 解析json值
        /// </summary>
        public static JsonValue ParseJsonValue(TokenType nextTokenType)
        {
            JsonValue value = new JsonValue();

            switch (nextTokenType)
            {

                case TokenType.Null:
                    lexer.GetNextToken(out _);
                    value.Type = ValueType.Null;
                    break;
                case TokenType.True:
                    lexer.GetNextToken(out _);
                    value.Type = ValueType.Boolean;
                    value.Boolean = true;
                    break;
                case TokenType.False:
                    lexer.GetNextToken(out _);
                    value.Type = ValueType.Boolean;
                    value.Boolean = false;
                    break;
                case TokenType.Number:
                    string token = lexer.GetNextToken(out _);
                    value.Type = ValueType.Number;
                    value.Number = double.Parse(token);
                    break;
                case TokenType.String:
                    token = lexer.GetNextToken(out _);
                    value.Type = ValueType.String;
                    value.Str = token;
                    break;
                case TokenType.LeftBracket:
                    value.Type = ValueType.Array;
                    value.Array = ParseJsonArray();
                    break;
                case TokenType.LeftBrace:
                    value.Type = ValueType.Object;
                    value.Obj = ParseJsonObject();
                    break;
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

            return value;
        }

        /// <summary>
        /// 解析json对象
        /// </summary>
        private static JsonObject ParseJsonObject()
        {
            JsonObject obj = new JsonObject();

            ParseJsonObjectProcedure(obj,null, (userdata1,userdata2,key, nextTokenType) => {
                JsonValue value = ParseJsonValue(nextTokenType);
                JsonObject jo = (JsonObject)userdata1;
                jo[key] = value;
            });

            return obj;

        }
   
        /// <summary>
        /// 解析Json数组
        /// </summary>
        private static JsonValue[] ParseJsonArray()
        {
            List<JsonValue> list = new List<JsonValue>();

            ParseJsonArrayProcedure(list,null,(userdata1,_,nextTokenType) => {
                JsonValue value = ParseJsonValue(nextTokenType);
                List<JsonValue> valueList = (List<JsonValue>)userdata1;
                valueList.Add(value);
            });

            return list.ToArray();
        }

        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static T ParseJson<T>(string json,bool reflection = true)
        {
            return (T)ParseJson(json, typeof(T),reflection);
        }

        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static object ParseJson(string json,Type type, bool reflection = true)
        {
            lexer.SetJsonText(json);

            if (reflection)
            {
                return ParseJsonObjectByType(type);
            }

            if (Gen.GenCodeDict.TryGetValue(type,out Func<JsonLexer, object> func))
            {
                return func(lexer);
            }

            throw new Exception($"没有{type}类型的预生成的反序列代码");
            
        }


        /// <summary>
        /// 解析json值为指定类型的实例值
        /// </summary>
        private static object ParseJsonValueByType(TokenType nextTokenType, Type type)
        {

            switch (nextTokenType)
            {
                case TokenType.Null:
                    lexer.GetNextToken(out _);
                    if (!type.IsAssignableFrom(typeof(ValueType)))
                    {
                        return null;
                    }
                    break;

                case TokenType.True:
                    lexer.GetNextToken(out _);
                    if (type == typeof(bool))
                    {
                        return true;
                    }
                    break;
                case TokenType.False:
                    lexer.GetNextToken(out _);
                    if (type == typeof(bool))
                    {
                        return false;
                    }
                    break;

                case TokenType.Number:
                    string token = lexer.GetNextToken(out _);
                    if (type == typeof(int))
                    {
                        return int.Parse(token);
                    }
                    if (type == typeof(float))
                    {
                        return float.Parse(token);
                    }
                    if (type == typeof(double))
                    {
                        return double.Parse(token);
                    }
                    break;

                case TokenType.String:
                    token = lexer.GetNextToken(out _);
                    if (type == typeof(string))
                    {
                        return token;
                    }
                    break;

                case TokenType.LeftBracket:
                    if (type.IsArray)
                    {
                        //数组
                        return ParseJsonArrayByType(type.GetElementType());
                    }

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        //List<T>
                        return ParseJsonArrayByType(type.GetGenericArguments()[0], type);
                    }

                    break;

                case TokenType.LeftBrace:

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        //字典
                        return ParseJsonObjectByDict(type,type.GetGenericArguments()[1]);
                    }

                    //数据类
                    return ParseJsonObjectByType(type);

            }

            throw new Exception("ParseJsonValueByType调用失败，tokenType == " + nextTokenType);
        }

        /// <summary>
        /// 解析json对象为指定类型的对象实例
        /// </summary>
        private static object ParseJsonObjectByType(Type type)
        {
            object obj = Activator.CreateInstance(type);

            if (!propertyInfoMap.ContainsKey(type))
            {
                AddToReflectionMap(type);
            }

            ParseJsonObjectProcedure(obj,type, (userdata1,userdata2, key, nextTokenType) => {

                Type t = (Type)userdata2;

                Dictionary<string, PropertyInfo> dict1 = propertyInfoMap[type];
                if (dict1 != null && dict1.TryGetValue(key,out PropertyInfo pi))
                {
                    object value = ParseJsonValueByType(nextTokenType, pi.PropertyType);
                    pi.SetValue(userdata1, value);
                }
                else
                {
                    Dictionary<string, FieldInfo> dict2 = fieldInfoMap[type];
                    if (dict2 != null && dict2.TryGetValue(key,out FieldInfo fi))
                    {
                        object value = ParseJsonValueByType(nextTokenType, fi.FieldType);
                        fi.SetValue(userdata1, value);
                    }
                    else
                    {
                        //这个json key既不是数据类的字段也不是属性，跳过
                        ParseJsonValue(nextTokenType);
                    }
                }


                //PropertyInfo pi = t.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
                //if (pi != null)
                //{
                //    object value = ParseJsonValueByType(nextTokenType, pi.PropertyType);
                //    pi.SetValue(userdata1, value);
                //}
                //else
                //{
                //    FieldInfo fi = t.GetField(key, BindingFlags.Instance | BindingFlags.Public);
                //    if (fi != null)
                //    {
                //        object value = ParseJsonValueByType(nextTokenType, fi.FieldType);
                //        fi.SetValue(userdata1, value);

                //    }
                //    else
                //    {
                //        //这个json key既不是数据类的字段也不是属性，跳过
                //        lexer.GetNextTokenByType(nextTokenType);
                //    }
                //}
            });

            return obj;

        }

        /// <summary>
        /// 解析json对象为字典，key为string类型
        /// </summary>
        private static object ParseJsonObjectByDict(Type dictType, Type valueType)
        {
            IDictionary dict = (IDictionary)Activator.CreateInstance(dictType);

            ParseJsonObjectProcedure(dict, valueType, (userdata1, userdata2, key, nextTokenType) => {
                Type t = (Type)userdata2;
                object value = ParseJsonValueByType(nextTokenType, t);
                dict.Add(key, value);
            });

            return dict;
        }


        /// <summary>
        /// 解析Json数组为指定类型的Array或List<T>
        /// </summary>
        private static object ParseJsonArrayByType(Type elementType, Type listType = null)
        {
            IList list;
            if (listType == null)
            {
                //数组
                list = new List<object>();
            }
            else
            {
                //List<T>
                list = (IList)Activator.CreateInstance(listType); 
            }

            ParseJsonArrayProcedure(list, elementType, (userdata1, userdata2, nextTokenType) =>
            {
                object value = ParseJsonValueByType(nextTokenType, (Type)userdata2);
                IList valueList = (IList)userdata1;
                valueList.Add(value);
            });

            //返回List<T>
            if (listType != null)
            {
                return list;
            }

            //返回数组
            Array array = Array.CreateInstance(elementType, list.Count);  
            for (int i = 0; i < list.Count; i++)
            {

                object element = list[i];
                array.SetValue(element, i);
            }

            return array;

         
            
        }


    }

}
