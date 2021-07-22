using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Reflection;
namespace CatJson
{
    public static class Util
    {
        public static StringBuilder CachedSB = new StringBuilder();

        /// <summary>
        /// type是否为数字类型（int float double)
        /// </summary>
        public static bool IsNumber(Type type)
        {
            return type == typeof(int) || type == typeof(float) || type == typeof(double);
        }

        /// <summary>
        /// type是否为数组或List<T>类型
        /// </summary>
        public static bool IsArrayOrList(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }

        /// <summary>
        /// type是否为字典类型
        /// </summary>
        public static bool IsDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        /// <summary>
        /// type是否为需要生成解析代码的json数据类
        /// </summary>
        public static bool IsGenParseCode(Type type)
        {
            return type.GetCustomAttribute<GenParseCodeRootAttribute>() != null;
        }
    }

}
