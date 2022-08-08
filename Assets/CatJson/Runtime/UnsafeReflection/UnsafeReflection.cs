using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;

namespace CatJson
{
    /// <summary>
    /// 不安全的快速反射
    /// </summary>
    public static class UnsafeReflection
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

                UnsafeFieldInfo ufi = new UnsafeFieldInfo(fi.Name,fi.FieldType,fi.FieldType.IsValueType,Type.GetTypeCode(fi.FieldType),UnsafeUtil.GetFieldOffset(fi));
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
    }
}