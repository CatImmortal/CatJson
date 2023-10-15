using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// 数组类型的Json格式化器
    /// </summary>
    public class ArrayFormatter : BaseJsonFormatter<Array>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, Array value, Type type, int depth)
        {
            parser.AppendLine("[");
            Type elementType = TypeUtil.GetArrayOrListElementType(type);
            for (int i = 0; i < value.Length; i++)
            {
                object element = value.GetValue(i);
                parser.AppendTab(depth);
                if (element == null)
                {
                    parser.Append("null");
                }
                else
                {
                    parser.InternalToJson(element,elementType,depth + 1);
                }
                if (i < value.Length-1)
                {
                    parser.AppendLine(",");
                }
                 
            }
            parser.AppendLine(string.Empty);
            parser.Append("]", depth - 1);
        }

        /// <inheritdoc />
        public override Array ParseJson(JsonParser parser, Type type)
        {
            List<object> list = new List<object>();
            Type elementType = TypeUtil.GetArrayOrListElementType(type);
            
            ParserHelper.ParseJsonArrayProcedure(parser,list, elementType, (localParser, userdata1,userdata2) =>
            {
                IList localList = (IList) userdata1;
                Type localElementType = (Type) userdata2;
                object value = localParser.InternalParseJson(localElementType);
                localList.Add(value);
            });
            
            Array array = Array.CreateInstance(elementType, list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                object element = list[i];
                array.SetValue(element, i);
            }

            return array;
        }
    }
}