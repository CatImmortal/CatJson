using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CatJson
{
    public static partial class JsonParser
    {
        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static T ParseJson<T>(string json, bool reflection = true)
        {
            return (T)ParseJson(json, typeof(T), reflection);
        }

        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static object ParseJson(string json, Type type, bool reflection = true)
        {
            Lexer.SetJsonText(json);

            if (reflection)
            {
                return ParseJsonObjectByType(type);
            }

            if (ParseCode.ParseCodeFuncDict.TryGetValue(type, out Func<object> func))
            {
                return func();
            }

            throw new Exception($"没有为{type}类型预生成的解析代码");

        }


        /// <summary>
        /// 解析json值为指定类型的实例值
        /// </summary>
        private static object ParseJsonValueByType(TokenType nextTokenType, Type type)
        {
            if (extensionParseFuncDict.TryGetValue(type, out Func<object> func))
            {
                //自定义解析
                return func();
            }

            switch (nextTokenType)
            {
                case TokenType.Null:
                    Lexer.GetNextToken(out _);
                    if (!type.IsValueType)
                    {
                        return null;
                    }
                    break;

                case TokenType.True:
                case TokenType.False:
                    Lexer.GetNextToken(out _);
                    if (type == typeof(bool))
                    {
                        return nextTokenType == TokenType.True;
                    }
                    break;

                case TokenType.Number:
                    RangeString? token = Lexer.GetNextToken(out _);
                    string str = token.Value.ToString();
                    if (type == typeof(int))
                    {
                        return int.Parse(str);
                    }
                    if (type == typeof(long))
                    {
                        return long.Parse(str);
                    }
                    if (type == typeof(float))
                    {
                        return float.Parse(str);
                    }
                    if (type == typeof(double))
                    {
                        return double.Parse(str);
                    }
                    break;

                case TokenType.String:
                    token = Lexer.GetNextToken(out _);
                    if (type == typeof(string))
                    {
                        return token.Value.ToString();
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
                        return ParseJsonObjectByDict(type, type.GetGenericArguments()[1]);
                    }

                    //类对象
                    return ParseJsonObjectByType(type);

            }

            throw new Exception("ParseJsonValueByType调用失败，tokenType == " + nextTokenType + ",type == " + type.FullName);
        }

        /// <summary>
        /// 解析json对象为指定类型的对象实例
        /// </summary>
        public static object ParseJsonObjectByType(Type type)
        {
            object obj = Activator.CreateInstance(type);

            if (!propertyInfoDict.ContainsKey(type) && !fieldInfoDict.ContainsKey(type))
            {
                //初始化反射信息
                AddToReflectionMap(type);
            }

            ParseJsonObjectProcedure(obj, type, (userdata1, userdata2, key, nextTokenType) => {

                Type t = (Type)userdata2;

                propertyInfoDict.TryGetValue(t, out Dictionary<RangeString, PropertyInfo> dict1);
                if (dict1 != null && dict1.TryGetValue(key, out PropertyInfo pi))
                {
                    //先尝试获取名为key的属性
                    object value = ParseJsonValueByType(nextTokenType, pi.PropertyType);
                    pi.SetValue(userdata1, value);
                }
                else
                {
                    //属性没有 再试试字段
                    fieldInfoDict.TryGetValue(t, out Dictionary<RangeString, FieldInfo> dict2);
                    if (dict2 != null && dict2.TryGetValue(key, out FieldInfo fi))
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
                dict.Add(key.ToString(), value);
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

