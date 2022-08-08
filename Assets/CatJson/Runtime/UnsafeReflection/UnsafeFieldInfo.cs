using System;
using Unity.Collections.LowLevel.Unsafe;

namespace CatJson
{

    public unsafe class UnsafeFieldInfo
    {
        public string Name;
        public Type FieldType;
        private bool isValueType;
        private TypeCode typeCode;
        
        /// <summary>
        /// 此字段在对象上的偏移值
        /// </summary>
        private int offset;

        public UnsafeFieldInfo(string name, Type fieldType,bool isValueType,TypeCode typeCode,int offset)
        {
            Name = name;
            FieldType = fieldType;
            this.isValueType = isValueType;
            this.typeCode = typeCode;
            this.offset = offset;
        }
        
        /// <summary>
        /// 获取字段值
        /// </summary>
        public object GetValue(object obj)
        {
            IntPtr ptr = UnsafeUtil.GetPtr(obj) + offset;
            object value = null;
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    value = *(bool*) ptr;
                    break;
                case TypeCode.Byte:
                    value = *(byte*) ptr;
                    break;
                case TypeCode.Char:
                    value = *(char*) ptr;
                    break;
                case TypeCode.DateTime:
                    value = *(DateTime*) ptr;
                    break;
                case TypeCode.Decimal:
                    value = *(decimal*) ptr;
                    break;
                case TypeCode.Double:
                    value = *(double*) ptr;
                    break;
                case TypeCode.Int16:
                    value = *(short*) ptr;
                    break;
                case TypeCode.Int32:
                    value = *(int*) ptr;
                    break;
                case TypeCode.Int64:
                    value = *(long*) ptr;
                    break;
                case TypeCode.SByte:
                    value = *(sbyte*) ptr;
                    break;
                case TypeCode.Single:
                    value = *(float*) ptr;
                    break;
                case TypeCode.UInt16:
                    value = *(ushort*) ptr;
                    break;
                case TypeCode.UInt32:
                    value = *(uint*) ptr;
                    break;
                case TypeCode.UInt64:
                    value = *(ulong*) ptr;
                    break;
                
                case TypeCode.DBNull:
                case TypeCode.String:
                case TypeCode.Object:
                    if (isValueType)
                    {
                        value = UnsafeUtil.GetStruct(ptr, FieldType);
                    }
                    else
                    {
                        value = UnsafeUtil.GetObject(ptr);
                    }
                    break;
            }

            return value;
        }

        /// <summary>
        /// 设置字段值
        /// </summary>
        public void SetValue(object obj, object value)
        {
            IntPtr ptr = UnsafeUtil.GetPtr(obj) + offset;
            
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    *(bool*) ptr = (bool) value;
                    break;
                case TypeCode.Byte:
                    *(byte*) ptr = (byte) value;
                    break;
                case TypeCode.Char:
                    *(char*) ptr = (char) value;
                    break;
                case TypeCode.DateTime:
                    *(DateTime*) ptr = (DateTime) value;
                    break;
                case TypeCode.Decimal:
                    *(decimal*) ptr = (decimal) value;
                    break;
                case TypeCode.Double:
                    *(double*) ptr = (double) value;
                    break;
                case TypeCode.Int16:
                    *(short*) ptr = (short) value;
                    break;
                case TypeCode.Int32:
                    *(int*) ptr = (int) value;
                    break;
                case TypeCode.Int64:
                    *(long*) ptr = (long) value;
                    break;
                case TypeCode.SByte:
                    *(sbyte*) ptr = (sbyte) value;
                    break;
                case TypeCode.Single:
                    *(float*) ptr = (float) value;
                    break;
                case TypeCode.UInt16:
                    *(ushort*) ptr = (ushort) value;
                    break;
                case TypeCode.UInt32:
                    *(uint*) ptr = (uint) value;
                    break;
                case TypeCode.UInt64:
                    *(ulong*) ptr = (ulong) value;
                    break;
                
                case TypeCode.DBNull:
                case TypeCode.String:
                case TypeCode.Object:
                    if (isValueType)
                    {
                        UnsafeUtil.SetStruct(ptr,value);
                    }
                    else
                    {
                        UnsafeUtil.SetObject(ptr,value);
                    }
                    break;
            }
        }
    }
}