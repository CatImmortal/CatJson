using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CatJson
{
    public unsafe class UnsafeFieldInfo
    {
        public string Name;
        public Type FieldType;
        private bool isValueType;
        private TypeCode typeCode;
        private int offset;
        private FieldInfo fi;

        public UnsafeFieldInfo(string name, Type fieldType,bool isValueType,TypeCode typeCode,int offset,FieldInfo fi)
        {
            Name = name;
            FieldType = fieldType;
            this.isValueType = isValueType;
            this.typeCode = typeCode;
            this.offset = offset;
            this.fi = fi;
        }

        public object GetValue(object obj)
        {
            return fi.GetValue(obj);
        }

        public void SetValue(object obj, object value)
        {
            fi.SetValue(obj,value);
        }
        
        public void* UnsafeGetValue(object obj)
        {
            IntPtr ptr = UnsafeUtil.GetPtr(obj) + offset;
            void* value = ptr.ToPointer();

            return value;
        }
        
        public void UnsafeSetValue(object obj, void* value)
        {
            IntPtr ptr = UnsafeUtil.GetPtr(obj) + offset;
            
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    *(bool*) ptr = *(bool*) value;
                    break;

                case TypeCode.Int32:
                    *(int*) ptr = *(int*) value;
                    break;
                
                case TypeCode.Single:
                    *(float*) ptr = *(float*) value;
                    break;
               
            }
        }
    }
}