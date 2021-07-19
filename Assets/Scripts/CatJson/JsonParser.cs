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
        private static JsonLexer lexer = new JsonLexer();

        /// <summary>
        /// 解析json文本为Json对象
        /// </summary>
        public static JsonObject ParseJson(string json)
        {
            lexer.SetJsonText(json);
            return ParseJsonObject();
        }

        /// <summary>
        /// 解析json对象
        /// </summary>
        private static JsonObject ParseJsonObject()
        {
            //跳过 {
            lexer.GetNextTokenByType(TokenType.LeftBrace);

            JsonObject obj = new JsonObject();

            while (lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                string key = lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();
                JsonValue value = ParseJsonValue(nextTokenType);

                obj[key] = value;

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

            return obj;

        }

        /// <summary>
        /// 解析json值
        /// </summary>
        private static JsonValue ParseJsonValue(TokenType nextTokenType)
        {
            JsonValue value = new JsonValue();

            switch (nextTokenType)
            {

                case TokenType.Null:
                    lexer.GetNextTokenByType(nextTokenType);
                    value.Type = ValueType.Null;
                    break;
                case TokenType.True:
                    lexer.GetNextTokenByType(nextTokenType);
                    value.Type = ValueType.Boolean;
                    value.Boolean = true;
                    break;
                case TokenType.False:
                    lexer.GetNextTokenByType(nextTokenType);
                    value.Type = ValueType.Boolean;
                    value.Boolean = false;
                    break;
                case TokenType.Number:
                    string token = lexer.GetNextTokenByType(nextTokenType);
                    value.Type = ValueType.Number;
                    value.Number = double.Parse(token);
                    break;
                case TokenType.String:
                    token = lexer.GetNextTokenByType(nextTokenType);
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
        /// 解析Json数组
        /// </summary>
        /// <returns></returns>
        private static JsonValue[] ParseJsonArray()
        {
            List<JsonValue> list = new List<JsonValue>();

            //跳过[
            lexer.GetNextTokenByType(TokenType.LeftBracket);

            while (lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();
                JsonValue value = ParseJsonValue(nextTokenType);

                list.Add(value);

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

            JsonValue[] array = list.ToArray();

            return array;
        }

        #region 先转换为JsonObject再转换为对象的写法
        ///// <summary>
        ///// 解析json为指定类型的对象
        ///// </summary>
        //public static T ParseJson<T>(string json)
        //{
        //    return (T)ParseJson(json, typeof(T));
        //}



        ///// <summary>
        ///// 解析json为指定类型的对象
        ///// </summary>
        //public static object ParseJson(string json,Type type)
        //{
        //    JsonObject jsonObj = ParseJson(json);
        //    return ConvertObjectByType(jsonObj, type);
        //}

        ///// <summary>
        ///// 将JsonObject转换为指定Type的实例对象
        ///// </summary>
        //private static object ConvertObjectByType(JsonObject jsonObj,Type type)
        //{
        //    object obj = Activator.CreateInstance(type);

        //    //获取类型的字段信息
        //    FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);

        //    foreach (FieldInfo field in fields)
        //    {
        //        if (jsonObj.TryGetValue(field.Name,out JsonValue jsonValue))
        //        {
        //            //有同名字段 获取值然后赋值给这个字段
        //            object value = GetValueByType(jsonValue, field.FieldType);
        //            field.SetValue(obj, value);
        //        }
        //    }

        //    //获取类型的属性信息
        //    PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        //    foreach (PropertyInfo prop in props)
        //    {
        //        if (jsonObj.TryGetValue(prop.Name,out JsonValue jsonValue))
        //        {
        //            object value = GetValueByType(jsonValue, prop.PropertyType);
        //            prop.SetValue(obj, value);
        //        }
        //    }

        //    return obj;
        //}

        ///// <summary>
        ///// 从JsonValue中获取指定Type的实例值
        ///// </summary>
        //private static object GetValueByType(JsonValue jsonValue,Type type)
        //{
        //    //null
        //    if (jsonValue.Type == ValueType.Null && !type.IsAssignableFrom(typeof(ValueType)))
        //    {
        //        return null;
        //    }

        //    //bool
        //    if (jsonValue.Type == ValueType.Boolean && type == typeof(bool))
        //    {
        //        return jsonValue.Boolean;
        //    }

        //    //float double
        //    if (jsonValue.Type == ValueType.Number)
        //    {
        //        if (type == typeof(double))
        //        {
        //            return jsonValue.Number;
        //        }

        //        if (type == typeof(float))
        //        {
        //            return (float)jsonValue.Number;
        //        }

        //        if (type == typeof(int))
        //        {
        //            return (int)jsonValue.Number;
        //        }
        //    }

        //    //string
        //    if (jsonValue.Type == ValueType.String && type == typeof(string))
        //    {
        //        return jsonValue.Str;
        //    }

        //    //数组 或 list
        //    if (jsonValue.Type == ValueType.Array)
        //    {
        //        int length = jsonValue.Array.Length;

        //        //数组
        //        if (type.IsArray)
        //        {
        //            Type elementType = type.GetElementType();  //数组元素类型
        //            Array array = Array.CreateInstance(elementType, length);  //数组实例
        //            for (int i = 0; i < length; i++)
        //            {
        //                JsonValue elementJsonValue = jsonValue.Array[i];
        //                object element = GetValueByType(elementJsonValue, elementType);
        //                array.SetValue(element, i);
        //            }
        //            return array;
        //        }

        //        //List<T>
        //        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        //        {
        //            Type elementType = type.GetGenericArguments()[0];  //T的类型
        //            object listObj = Activator.CreateInstance(type,length);  //list实例
        //            MethodInfo mi = type.GetMethod("Add");  //list.add

        //            for (int i = 0; i < length; i++)
        //            {
        //                JsonValue elementJsonValue = jsonValue.Array[i];
        //                object element = GetValueByType(elementJsonValue, elementType);
        //                paramObjs[0] = element;
        //                mi.Invoke(listObj, paramObjs);
        //            }

        //            return listObj;
        //        }
        //    }

        //    //数据类对象
        //    if (jsonValue.Type == ValueType.Object)
        //    {
        //        return ConvertObjectByType(jsonValue.Obj, type);
        //    }

        //    throw new Exception($"GetValueByType调用失败，JsonValueType:{jsonValue.Type},Type:{type}");
        //}
        #endregion

        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static T ParseJson<T>(string json)
        {
            return (T)ParseJson(json, typeof(T));
        }

        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static object ParseJson(string json,Type type)
        {
            lexer.SetJsonText(json);
            return ParseJsonObjectByType(type);
        }

        /// <summary>
        /// 解析json对象为指定类型的对象实例
        /// </summary>
        private static object ParseJsonObjectByType(Type type)
        {
            //跳过 {
            lexer.GetNextTokenByType(TokenType.LeftBrace);

            object obj = Activator.CreateInstance(type);

            while (lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                string key = lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();

                PropertyInfo pi = type.GetProperty(key, BindingFlags.Instance | BindingFlags.Public);
                if (pi != null)
                {
                    object value = ParseJsonValueByType(nextTokenType, pi.PropertyType);
                    pi.SetValue(obj, value);
                }
                else
                {
                    FieldInfo fi = type.GetField(key, BindingFlags.Instance | BindingFlags.Public);
                    if (fi != null)
                    {
                        object value = ParseJsonValueByType(nextTokenType, fi.FieldType);
                        fi.SetValue(obj, value);
                       
                    }
                    else
                    {
                        //这个json key既不是数据类的字段也不是属性，跳过
                        ParseJsonValue(nextTokenType);
                    }
                }

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

            return obj;

        }

        /// <summary>
        /// 解析json值为指定类型的实例值
        /// </summary>
        private static object ParseJsonValueByType(TokenType nextTokenType,Type type)
        {
           

            switch (nextTokenType)
            {
                case TokenType.Null:
                    lexer.GetNextTokenByType(nextTokenType);
                    if (!type.IsAssignableFrom(typeof(ValueType)))
                    {
                        return null;
                    }
                    break;

                case TokenType.True:
                    lexer.GetNextTokenByType(nextTokenType);
                    if (type == typeof(bool))
                    {
                        return true;
                    }
                    break;
                case TokenType.False:
                    lexer.GetNextTokenByType(nextTokenType);
                    if (type == typeof(bool))
                    {
                        return false;
                    }
                    break;

                case TokenType.Number:
                    string token = lexer.GetNextTokenByType(nextTokenType);
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
                    token = lexer.GetNextTokenByType(nextTokenType);
                    if (type == typeof(string))
                    {
                        return token;
                    }
                    break;

                case TokenType.LeftBracket:
                    if (type.IsArray)
                    {
                        //数组
                        return ParseJsonArrayByArray(type.GetElementType());
                    }

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        //List<T>
                        return ParseJsonArrayByList(type, type.GetGenericArguments()[0]);
                    }
                    break;

                case TokenType.LeftBrace:

                    return ParseJsonObjectByType(type);
                    
            }

            throw new Exception("ParseJsonValueByType调用失败，tokenType == " + nextTokenType);
        }

        /// <summary>
        /// 解析Json数组为指定元素类型的数组
        /// </summary>
        private static object ParseJsonArrayByArray(Type elementType)
        {

            List<object> list = new List<object>();

            //跳过[
            lexer.GetNextTokenByType(TokenType.LeftBracket);

            while (lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();
                object value = ParseJsonValueByType(nextTokenType,elementType);

                list.Add(value);

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

        
            Array array = Array.CreateInstance(elementType, list.Count);  //数组实例
            for (int i = 0; i < list.Count; i++)
            {

                object element = list[i];
                array.SetValue(element, i);
            }

            return array;
        }

        /// <summary>
        /// 解析json数组为指定元素类型的List<T>
        /// </summary>
        private static object ParseJsonArrayByList(Type listType, Type elementType)
        {
            IList list = (IList)Activator.CreateInstance(listType);  //list实例

            //跳过[
            lexer.GetNextTokenByType(TokenType.LeftBracket);

            while (lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();
                object value = ParseJsonValueByType(nextTokenType, elementType);

                list.Add(value);

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

            return list;
        }

    }

}
