using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;

namespace CatJson
{
    /// <summary>
    /// Unsafe相关工具类
    /// </summary>
    public unsafe class UnsafeUtil
    {
        /// <summary>
        /// 获取指定对象的内存地址
        /// </summary>
        public static IntPtr GetPtr(object obj)
        {
            ulong gcHandle;
            IntPtr ptr = (IntPtr)UnsafeUtility.PinGCObjectAndGetAddress(obj, out gcHandle);
            UnsafeUtility.ReleaseGCObject(gcHandle);
            return ptr;
        }

        /// <summary>
        /// 将指定引用类型对象的内存地址赋值给目标指针
        /// </summary>
        public static void SetObject(IntPtr ptr, object value)
        {
            UnsafeUtility.CopyObjectAddressToPtr(value,(void*)ptr);
        }

        /// <summary>
        /// 获取目标指针上的引用类型对象
        /// </summary>
        public static object GetObject(IntPtr ptr)
        {
            object obj = UnsafeUtility.ReadArrayElement<object>((void*) ptr, 0);
            return obj;
        }

        /// <summary>
        /// 将指定结构体复制到目标内存地址上
        /// </summary>
        public static void SetStruct(IntPtr ptr, object value)
        {
            //UnsafeUtility.CopyStructureToPtr();
            Marshal.StructureToPtr(value,ptr,true);
        }

        /// <summary>
        /// 获取目标内存地址上的结构体
        /// </summary>
        public static object GetStruct(IntPtr ptr, Type type)
        {
            object obj = Marshal.PtrToStructure(ptr, type);
            return obj;
        }

        /// <summary>
        /// 获取字段偏移量
        /// </summary>
        public static int GetFieldOffset(FieldInfo field)
        {
            int offset = UnsafeUtility.GetFieldOffset(field);
            return offset;
        }
    }
}