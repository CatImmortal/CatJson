using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// 类型相关的工具类
    /// </summary>
    public static class TypeUtil
    {
        /// <summary>
        /// 对比Type是否为同一类型的Type
        /// </summary>
        public static bool TypeEquals(Type t1, Type t2)
        {
            return t1 == t2;
        }

        /// <summary>
        /// 获取obj的Type
        /// </summary>
        public static Type GetType(object obj)
        {
            return obj.GetType();
        }

        /// <summary>
        /// 获取Type对象序列化后的字符串
        /// </summary>
        public static string GetTypeString(Type value)
        {
            return $"\"{value.FullName},{value.Assembly.GetName().Name}\"";
        }

        /// <summary>
        /// 获取对象的真实Type
        /// </summary>
        public static Type GetRealType(string realTypeValue)
        {
            return Type.GetType(realTypeValue);
        }

        /// <summary>
        /// 创建type的实例
        /// </summary>
        public static object CreateInstance(Type type, bool isUseParamCtor)
        {
            object obj;

            try
            {
                obj = Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                if (e is MissingMethodException && isUseParamCtor)
                {
                    //没有无参构造 并且允许使用任意有参构造
                    obj = TypeMetaDataManager.CreateInstanceWithParamCtor(type);
                }
                else
                {
                    throw;
                }
            }

            return obj;

        }

        /// <summary>
        /// obj是否为数字
        /// </summary>
        public static bool IsNumber(object obj)
        {
            return obj is byte || obj is int || obj is long || obj is float || obj is double
                   || obj is uint || obj is ulong || obj is ushort || obj is short || obj is decimal
                   || obj is sbyte;
        }

        /// <summary>
        /// type是否为数字类型
        /// </summary>
        public static bool IsNumberType(Type type)
        {
            return type == typeof(byte) || type == typeof(int) || type == typeof(long) || type == typeof(float) ||
                   type == typeof(double)
                   || type == typeof(uint) || type == typeof(ulong) || type == typeof(ushort) ||
                   type == typeof(short) || type == typeof(decimal)
                   || type == typeof(sbyte);
        }


        /// <summary>
        /// obj是否为数组或List
        /// </summary>
        public static bool IsArrayOrList(object obj)
        {
            return obj is Array || obj is IList;
        }

        /// <summary>
        /// type是否为数组或List类型
        /// </summary>
        public static bool IsArrayOrListType(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }

        /// <summary>
        /// 获取数组或List的元素类型
        /// </summary>
        public static Type GetArrayOrListElementType(Type arrayType)
        {
            Type elementType;
            if (arrayType.IsArray)
            {
                //数组

                elementType = arrayType.GetElementType();

            }
            else
            {
                //List

                elementType = arrayType.GenericTypeArguments[0];

            }

            return elementType;
        }

        /// <summary>
        /// obj是否为字典
        /// </summary>
        public static bool IsDictionary(object obj)
        {
            return obj is IDictionary;
        }

        /// <summary>
        /// type是否为字典类型
        /// </summary>
        public static bool IsDictionaryType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        /// <summary>
        /// 获取字典key的类型
        /// </summary>
        public static Type GetDictKeyType(Type dictType)
        {
            Type keyType = dictType.GetGenericArguments()[0];

            return keyType;
        }

        /// <summary>
        /// 获取字典value的类型
        /// </summary>
        public static Type GetDictValueType(Type dictType)
        {
            Type valueType = dictType.GetGenericArguments()[1];

            return valueType;
        }


        /// <summary>
        /// value是否为内置基础类型的默认值(null 0 false)
        /// </summary>
        public static bool IsDefaultValue(object value)
        {
            if (!(value is System.ValueType))
            {
                return value == default;
            }

            if (value is byte b)
            {
                return b == default;
            }

            if (value is int i)
            {
                return i == default;
            }

            if (value is long l)
            {
                return l == default;
            }

            if (value is float f)
            {
                return Math.Abs(f - default(float)) < float.Epsilon;
            }

            if (value is double d)
            {
                return Math.Abs(d - default(double)) < double.Epsilon;
            }

            if (value is bool boolean)
            {
                return boolean == default;
            }

            if (value is sbyte sb)
            {
                return sb == default;
            }

            if (value is short s)
            {
                return s == default;
            }

            if (value is uint ui)
            {
                return ui == default;
            }

            if (value is ulong ul)
            {
                return ul == default;
            }

            if (value is ushort us)
            {
                return us == default;
            }

            if (value is decimal de)
            {
                return de == default;
            }

            return false;
        }



    }
}

