﻿using System;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// JsonObject类型的Json格式化器
    /// </summary>
    public class JsonObjectFormatter : BaseJsonFormatter<JsonObject>
    {
        /// <inheritdoc />
        public override void ToJson(JsonObject value,Type type,int depth)
        {
            TextUtil.AppendLine("{");

            if (value.ValueDict != null)
            {
                int index = 0;
                foreach (KeyValuePair<string, JsonValue> item in value.ValueDict)
                {
                    
                    TextUtil.Append("\"", depth + 1);
                    TextUtil.Append(item.Key);
                    TextUtil.Append("\"");

                    TextUtil.Append(":");

                    JsonParser.InternalToJson(item.Value,depth);

                    if (index < value.ValueDict.Count-1)
                    {
                        TextUtil.AppendLine(",");
                    }
                    index++;
                }
            }

            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("}", depth);
        }

        /// <inheritdoc />
        public override JsonObject ParseJson(Type type)
        {
            JsonObject obj = new JsonObject();

            ParserHelper.ParseJsonKeyValuePairProcedure(obj,default,default, (userdata,_,_, key) =>
            {
                 JsonValue value = JsonParser.InternalParseJson<JsonValue>();
                ((JsonObject)userdata)[key.ToString()] = value;
            });

            return obj;
        }


    }
}