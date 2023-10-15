using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// List类型的Json格式化器
    /// </summary>
    public class ListFormatter : BaseJsonFormatter<IList>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, IList value, Type type, int depth)
        {
            parser.AppendLine("[");
            Type elementType = TypeUtil.GetArrayOrListElementType(type);
            for (int i = 0; i < value.Count; i++)
            {
                object element = value[i];
                parser.AppendTab(depth);
                if (element == null)
                {
                    parser.Append("null");
                }
                else
                {
                    parser.InternalToJson(element,elementType,depth + 1);
                }
                if (i < value.Count - 1)
                {
                    parser.AppendLine(",");
                }
                 
            }
            parser.AppendLine(string.Empty);
            parser.Append("]", depth - 1);
        }

        /// <inheritdoc />
        public override IList ParseJson(JsonParser parser, Type type)
        {
            IList list = (IList)TypeUtil.CreateInstance(type,parser.IsUseParamCtor);
            Type elementType = TypeUtil.GetArrayOrListElementType(type);
            
            ParserHelper.ParseJsonArrayProcedure(parser,list, elementType, (localParser, userdata1,userdata2) =>
            {
                IList localList = (IList) userdata1;
                Type localElementType = (Type) userdata2;
                
                object value = localParser.InternalParseJson(localElementType);
                localList.Add(value);
            });

            return list;
        }
    }
}