using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
#if FUCK_LUA
using ILRuntime.Reflection;
#endif

namespace CatJson
{
    public static partial class JsonParser
    {
        /// <summary>
        /// 将指定类型的对象转换为Json文本
        /// </summary>
        public static string ToJson<T>(T obj, bool reflection = true)
        {
            return ToJson(obj, GetType(obj), reflection);
        }

        /// <summary>
        /// 将指定类型的对象转换为Json文本
        /// </summary>
        public static string ToJson(object obj, Type type, bool reflection = true)
        {
            if (obj is IJsonParserCallbackReceiver receiver)
            {
                //触发转换开始回调
                receiver.OnToJsonStart();
            }

            if (reflection)
            {
                //反射转换

                if (Util.IsArrayOrList(obj))
                {
                    //数组或list
                    AppendJsonArray(type, obj, 0);
                }
                else if (Util.IsDictionary(obj))
                {
                    //字典
                    AppendJsonDict(obj, 0);
                }
                else
                {
                    //自定义类
                    AppendJsonObject(obj, type, 0);
                }
            }
            else
            {
                if (GenJsonCodes.ToJsonCodeFuncDict.TryGetValue(type, out Action<object, int> action))
                {
                    //使用预生成代码转换
                    action(obj, 0);
                }
                else
                {
                    throw new Exception($"没有为{type}类型预生成的转换代码");
                }
            }

            string json = Util.CachedSB.ToString();
            Util.CachedSB.Clear();
            return json;
        }

        /// <summary>
        /// 追加Json数据类对象文本
        /// </summary>
        private static void AppendJsonObject(object obj, Type type, int depth)
        {
            if (!propertyInfoDict.ContainsKey(type) && !fieldInfoDict.ContainsKey(type))
            {
                //初始化反射信息
                AddToReflectionMap(type);
            }

            //是否需要删除最后一个逗号
            bool needRemoveLastComma = false;
            
            Util.AppendLine("{");
            propertyInfoDict.TryGetValue(type, out Dictionary<RangeString, PropertyInfo> piDict);
            if (piDict != null)
            {
                foreach (KeyValuePair<RangeString, PropertyInfo> item in piDict)
                {
                    object value = item.Value.GetValue(obj);
                    Type piType = item.Value.PropertyType;
                    string piName = item.Value.Name;

                    if (Util.IsDefaultValue(value))
                    {
                        //默认值跳过序列化
                        continue;
                    }

                    AppendJsonKey(piName, depth+1);
                    AppendJsonValue(piType, value, depth+1);
                    Util.AppendLine(",");
                    needRemoveLastComma = true;
                }
            }

            fieldInfoDict.TryGetValue(type, out Dictionary<RangeString, FieldInfo> fiDict);
            if (fiDict != null)
            {
                foreach (KeyValuePair<RangeString, FieldInfo> item in fiDict)
                {
                    object value = item.Value.GetValue(obj);
                    Type fiType = item.Value.FieldType;
                    string fiName = item.Value.Name;
                    if (Util.IsDefaultValue(value))
                    {
                        //默认值跳过序列化
                        continue;
                    }
                    AppendJsonKey(fiName, depth+1);
                    AppendJsonValue(fiType, value, depth+1);
                    Util.AppendLine(",");
                    needRemoveLastComma = true;
                }
            }

            if (needRemoveLastComma)
            {

                //需要删除的字符长度
                int needRemoveLength = 1;
                if (IsFormat)
                {
                    //开启了格式化序列化，需要额外删除一个换行符
                    needRemoveLength += Util.NewLineLength;
                }

                //最后一个逗号的位置
                int lastCommaIndex = Util.CachedSB.Length - needRemoveLength;
                
                Util.CachedSB.Remove(lastCommaIndex, needRemoveLength);
            }
            
            Util.AppendLine(string.Empty);
            Util.Append("}",depth);
        }

        /// <summary>
        /// 追加json key文本
        /// </summary>
        public static void AppendJsonKey(string key, int depth)
        {
            Util.Append("\"", depth);
            Util.Append(key);
            Util.Append("\"");
            Util.Append(":");
        }

        /// <summary>
        /// 追加Json 值文本
        /// </summary>
        public static void AppendJsonValue(Type valueType, object value, int depth)
        {
            valueType = CheckType(valueType);

            if (ExtensionToJsonFuncDict.TryGetValue(valueType, out Action<object> action))
            {
                //自定义转换Json文本方法
                action(value);
                return;
            }

            //根据属性值的不同类型进行序列化
            if (Util.IsNumber(value))
            {
                //数字
                Util.Append(value.ToString());
                return;
            }

            if (value is string || value is char)
            {
                //字符串
                Util.Append("\"");
                Util.Append(value.ToString());
                Util.Append("\"");
                return;
            }

            if (value is bool)
            {
                //bool值
                bool b = (bool)value;
                if (b == true)
                {
                    Util.Append("true");
                }
                else
                {
                    Util.Append("false");
                }

                return;
            }

            if (value is Enum)
            {
                //枚举
                int enumInt = (int)value;
                Util.Append(enumInt.ToString());
                return;
            }

            if (Util.IsArrayOrList(value))
            {
                //数组或List
                AppendJsonArray(valueType, value, depth);
                return;
            }


            if (Util.IsDictionary(value))
            {
                //字典
                AppendJsonDict(value, depth);
                return;
            }

            //自定义类对象
            AppendJsonObject(value, valueType, depth);
        }

        /// <summary>
        /// 追加json数组文本
        /// </summary>
        private static void AppendJsonArray(Type valueType, object value, int depth)
        {
            Util.AppendLine("[");
            
            if (valueType.IsArray)
            {
                Array array = (Array)value;
                for (int i = 0; i < array.Length; i++)
                {
                    object element = array.GetValue(i);
                    Util.AppendTab(depth + 1);
                    if (element == null)
                    {
                        Util.Append("null");
                    }
                    else
                    {
                        AppendJsonValue(GetType(element), element, depth+1);
                    }
                    if (i< array.Length-1)
                    {
                        Util.AppendLine(",");
                    }
                 
                }
            }
            else
            {
                IList list = (IList)value;
                for (int i = 0; i < list.Count; i++)
                {
                    object element = list[i];
                    Util.AppendTab(depth + 1);
                    if (element == null)
                    {
                        Util.Append("null");
                    }
                    else
                    {
                        AppendJsonValue(GetType(element), element, depth + 1);
                    }

                    if (i< list.Count-1)
                    {
                        Util.AppendLine(",");
                    }
                }
            }
            Util.AppendLine(string.Empty);
            Util.Append("]", depth);
        }

        /// <summary>
        /// 追加json字典文本
        /// </summary>
        private static void AppendJsonDict(object value, int depth)
        {
            //字典
            IDictionary dict = (IDictionary)value;
            IDictionaryEnumerator enumerator = dict.GetEnumerator();

            Util.AppendLine("{");
            int index = 0;
            while (enumerator.MoveNext())
            {
                if (enumerator.Value == null)
                {
                    AppendJsonKey(enumerator.Key.ToString(), depth + 1);
                    Util.Append("null");
                    Util.AppendLine(",");
                }
                else
                {
                    AppendJsonKey(enumerator.Key.ToString(), depth+1);
                    AppendJsonValue(GetType(enumerator.Value), enumerator.Value, depth+1);
                    if (index < dict.Count-1)
                    {
                        Util.AppendLine(",");
                    }
                }

                index++;
            }

            Util.AppendLine(string.Empty);
            Util.Append("}", depth);
        }

        //------------------------为内置基础类型提供append方法，以供生成的代码调用------------------------------------
        public static void AppendJsonValue(bool b, int depth = 0)
        {
            if (b == true)
            {
                Util.Append("true", depth);
            }
            else
            {
                Util.Append("false", depth);
            }
        }

        public static void AppendJsonValue(string s, int depth = 0)
        {
            Util.Append("\"", depth);
            Util.Append(s);
            Util.Append("\"");
        }

        public static void AppendJsonValue(char c, int depth = 0)
        {
            Util.Append("\"", depth);
            Util.Append(c.ToString());
            Util.Append("\"");
        }

        public static void AppendJsonValue(byte b, int depth = 0)
        {
            Util.Append(b.ToString(), depth);
        }

        public static void AppendJsonValue(int i, int depth = 0)
        {
            Util.Append(i.ToString(), depth);
        }

        public static void AppendJsonValue(long l, int depth = 0)
        {
            Util.Append(l.ToString(), depth);
        }

        public static void AppendJsonValue(float f, int depth = 0)
        {
            Util.Append(f.ToString(CultureInfo.InvariantCulture), depth);
        }

        public static void AppendJsonValue(double d, int depth = 0)
        {
            Util.Append(d.ToString(CultureInfo.InvariantCulture), depth);
        }

        public static void AppendJsonValue(sbyte s, int depth = 0)
        {
            Util.Append(s.ToString(), depth);
        }

        public static void AppendJsonValue(short s, int depth = 0)
        {
            Util.Append(s.ToString(), depth);
        }

        public static void AppendJsonValue(uint ui, int depth = 0)
        {
            Util.Append(ui.ToString(), depth);
        }

        public static void AppendJsonValue(ulong ul, int depth = 0)
        {
            Util.Append(ul.ToString(), depth);
        }

        public static void AppendJsonValue(ushort us, int depth = 0)
        {
            Util.Append(us.ToString(), depth);
        }

        public static void AppendJsonValue(decimal d, int depth = 0)
        {
            Util.Append(d.ToString(CultureInfo.InvariantCulture), depth);
        }
    }
}