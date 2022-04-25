using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public class ArrayFormatter : BaseJsonFormatter<Array>
    {
        public Type ElementType { 
            get;
            set; 
        }
        
        public override void ToJson(Array value, int depth)
        {
            TextUtil.AppendLine("[");
            Type elementType = TypeUtil.GetArrayOrListElementType(value.GetType());
            for (int i = 0; i < value.Length; i++)
            {
                object element = value.GetValue(i);
                TextUtil.AppendTab(depth + 1);
                if (element == null)
                {
                    TextUtil.Append("null");
                }
                else
                {
                    JsonParser.InternalToJson(element,elementType,depth + 1);
                }
                if (i < value.Length-1)
                {
                    TextUtil.AppendLine(",");
                }
                 
            }
            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("]", depth);
        }

        public override Array ParseJson()
        {
            List<object> list = new List<object>();
            
            JsonParser.ParseJsonArrayProcedure(list, ElementType, (userdata1, userdata2) =>
            {
                object value = JsonParser.InternalParseJson((Type) userdata2);
                ((IList) userdata1).Add(value);
            });
            
            Array array = Array.CreateInstance(ElementType, list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                object element = list[i];
                array.SetValue(element, i);
            }

            return array;
        }
    }
}