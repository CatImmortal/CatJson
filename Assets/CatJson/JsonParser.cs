using System.Collections.Generic;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace CatJson
{
    /// <summary>
    /// Json解析器
    /// </summary>
    public partial class JsonParser
    {

        /// <summary>
        /// 默认Json解析器对象
        /// </summary>
        public static JsonParser Default { get; } = new JsonParser();
        
        private static NullFormatter nullFormatter = new NullFormatter();
        private static EnumFormatter enumFormatter = new EnumFormatter();
        private static ArrayFormatter arrayFormatter = new ArrayFormatter();
        private static ReflectionFormatter reflectionFormatter = new ReflectionFormatter();
        private static PolymorphicFormatter polymorphicFormatter = new PolymorphicFormatter();
        
        /// <summary>
        /// Json格式化器字典
        /// </summary>
        private static readonly Dictionary<Type, IJsonFormatter> formatterDict = new Dictionary<Type, IJsonFormatter>()
        {
            //基元类型
            {typeof(bool), new BooleanFormatter()},
            
            {typeof(byte), new ByteFormatter()},
            {typeof(sbyte), new SByteFormatter()},
            
            {typeof(short), new Int16Formatter()},
            {typeof(ushort), new UInt16Formatter()},
            
            {typeof(int), new Int32Formatter()},
            {typeof(uint), new UInt32Formatter()},
            
            {typeof(long), new Int64Formatter()},
            {typeof(ulong), new UInt64Formatter()},
            
            {typeof(float), new SingleFormatter()},
            {typeof(double), new DoubleFormatter()},
            {typeof(decimal), new DecimalFormatter()},
            
            {typeof(char), new CharFormatter()},
            {typeof(string), new StringFormatter()},
            
            //容器类型
            {typeof(List<>), new ListFormatter()},
            {typeof(Dictionary<,>), new DictionaryFormatter()},
            
            //Json通用对象类型
            {typeof(JsonObject), new JsonObjectFormatter()},
            {typeof(JsonValue), new JsonValueFormatter()},
            
            //Unity特有类型
            {typeof(Hash128), new Hash128Formatter()},
            {typeof(Vector2),new Vector2Formatter()},
            {typeof(Vector3),new Vector3Formatter()},
            {typeof(Vector4),new Vector4Formatter()},
            {typeof(Quaternion),new QuaternionFormatter()},
            {typeof(Color),new ColorFormatter()},
            {typeof(Bounds),new BoundsFormatter()},
            {typeof(Rect),new RectFormatter()},
            {typeof(Keyframe),new KeyFrameFormatter()},
            
            //其他
            {Type.GetType("System.RuntimeType,mscorlib"),new RuntimeTypeFormatter()},  //Type类型的变量其对象一般为RuntimeType类型，但是不能直接typeof(RuntimeType)，只能这样了
            {typeof(DateTime),new DateTimeFormatter()},
        };

        /// <summary>
        /// 添加自定义的Json格式化器
        /// </summary>
        public static void AddCustomJsonFormatter(Type type, IJsonFormatter formatter)
        {
            formatterDict[type] = formatter;
        }
        
        /// <summary>
        /// 设置用于获取字段/属性的BindingFlags
        /// </summary>
        public static void SetBindingFlags(BindingFlags bindingFlags)
        {
            TypeMetaDataManager.Flags = bindingFlags;
        }
        
        /// <summary>
        /// 添加需要忽略的成员
        /// </summary>
        public static void AddIgnoreMember(Type type, string memberName)
        {
            TypeMetaDataManager.AddIgnoreMember(type,memberName);
        }
        
        /// <summary>
        /// 设置字段的自定义JsonKey
        /// </summary>
        public static void SetJsonKey(Type type, string key, FieldInfo fi)
        {
            TypeMetaDataManager.SetJsonKey(type,key,fi);
        }
        
        /// <summary>
        /// 设置属性的自定义JsonKey
        /// </summary>
        public static void SetJsonKey(Type type,string key, PropertyInfo pi)
        {
            TypeMetaDataManager.SetJsonKey(type,key,pi);
        }

        /// <summary>
        /// Json词法分析器
        /// </summary>
        public JsonLexer Lexer { get; } = new JsonLexer();
        
        internal StringBuilder CachedSB { get; } = new StringBuilder();
        
        /// <summary>
        /// 序列化时是否开启格式化
        /// </summary>
        public bool IsFormat { get; set; } = true;

        /// <summary>
        /// 序列化时是否忽略默认值
        /// </summary>
        public bool IgnoreDefaultValue { get; set; } = true;

        /// <summary>
        /// 是否进行多态序列化/反序列化
        /// </summary>
        public bool IsPolymorphic { get; set; } = true;

        /// <summary>
        /// 是否在没有无参构造时使用任意有参构造
        /// </summary>
        public bool IsUseParamCtor { get; set; } = false;

        

        /// <summary>
        /// 跳过一个Json值
        /// </summary>
        internal void JumpJsonValue()
        {
            formatterDict[typeof(JsonValue)].ParseJson(this, null);
        }
       
        public void Append( char c, int tabNum = 0)
        {
            if (tabNum > 0 && IsFormat)
            {
                AppendTab(tabNum);
            }
           
            CachedSB.Append(c);
        }

        
        public void Append(string str,int tabNum = 0)
        {
            if (tabNum > 0 && IsFormat)
            {
                AppendTab(tabNum);
            }
           
            CachedSB.Append(str);
        }
        
        public void Append(RangeString rs,int tabNum = 0)
        {
            if (tabNum > 0 && IsFormat)
            {
                AppendTab(tabNum);
            }

            for (int i = 0; i < rs.Length; i++)
            {
                CachedSB.Append(rs[i]);
            }
            
           
        }
        
        public void AppendTab(int tabNum)
        {
            if (!IsFormat)
            {
                return;
            }
            for (int i = 0; i < tabNum; i++)
            {
                CachedSB.Append('\t');
            }
        }

        public void AppendLine(string str, int tabNum = 0)
        {
            if (tabNum > 0 && IsFormat)
            {
                AppendTab(tabNum);
            }

            if (IsFormat)
            {
                CachedSB.AppendLine(str);
            }
            else
            {
                CachedSB.Append(str);
            }
        }
    }

}
