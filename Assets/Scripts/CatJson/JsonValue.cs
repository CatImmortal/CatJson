using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// Json值
    /// </summary>
    public class JsonValue
    {
        public ValueType Type;



        public bool Boolean;
        public double Number;
        public string Str;
        public JsonValue[] Array;
        public JsonObject Obj;

        public override string ToString()
        {
            switch (Type)
            {
                case ValueType.Null:
                    return "null";
                case ValueType.Boolean:
                    return Boolean.ToString();
                case ValueType.Number:
                    return Number.ToString();
                case ValueType.String:
                    return Str;
                case ValueType.Array:
                    string str = "[";
                    for (int i = 0; i < Array.Length; i++)
                    {
                        str += Array[i];

                        if (i != Array.Length - 1)
                        {
                            str += " ,";
                        };
                    }
                    str += "] ";
                    return str;
                case ValueType.Object:
                    return Obj.ToString();
            }

            return "";
        }
    }

}
