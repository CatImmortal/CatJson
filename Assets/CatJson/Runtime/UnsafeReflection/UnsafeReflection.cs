using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;

namespace CatJson
{
    /// <summary>
    /// 不安全反射
    /// </summary>
    public static unsafe class UnsafeReflection
    {
        private static Dictionary<Type, Dictionary<RangeString, UnsafeFieldInfo>> fieldInfoDict =
            new Dictionary<Type, Dictionary<RangeString, UnsafeFieldInfo>>();

        /// <summary>
        /// 添加反射信息到字典中
        /// </summary>
        public static void AddReflectionInfo(Type type)
        {
            if (fieldInfoDict.ContainsKey(type))
            {
                return;
            }
            
            FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<RangeString, UnsafeFieldInfo> fiDict = new Dictionary<RangeString, UnsafeFieldInfo>(fis.Length);
            for (int i = 0; i < fis.Length; i++)
            {
                FieldInfo fi = fis[i];
                // if (IsIgnore(fi,type,fi.Name))
                // {
                //     //需要忽略
                //     continue;
                // }
                UnsafeFieldInfo ufi = new UnsafeFieldInfo(fi.Name,fi.FieldType,fi.FieldType.IsValueType,Type.GetTypeCode(fi.FieldType),UnsafeUtil.GetFieldOffset(fi),fi);
                fiDict.Add(new RangeString(fi.Name), ufi);
            }
            fieldInfoDict.Add(type, fiDict);
        }
        


        /// <summary>
        /// 获取指定类型的所有字段信息
        /// </summary>
        public static Dictionary<RangeString, UnsafeFieldInfo> GetFieldInfos(Type type)
        {
            return fieldInfoDict[type];
        }
        

        /// <summary>
        /// 设置字段值
        /// </summary>
        public static void UnsafeSetValue(UnsafeFieldInfo fi,object obj)
        {
            //只能手动把所有非托管类型全部if一遍
            void* value = null;
            if (fi.FieldType == typeof(int))
            {
                int result = JsonParser.GetUnmanagedJsonFormatter<int>().ParseJson(fi.FieldType,null);
                value = &result;
            }
            else if (fi.FieldType == typeof(float))
            {
                float result = JsonParser.GetUnmanagedJsonFormatter<float>().ParseJson(fi.FieldType,null);
                value = &result;
            }
            else if (fi.FieldType == typeof(bool))
            {
                bool result = JsonParser.GetUnmanagedJsonFormatter<bool>().ParseJson(fi.FieldType,null);
                value = &result;
            }
                        
            //void* value = JsonParser.UnmanagedParseJson(fi.FieldType);
            fi.UnsafeSetValue(obj,value);
        }
    }
}