using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class GenCodes
    {
        private static void ToJson_ToJson1_Data(object obj,int depth)
        {
            ToJson1_Data data = (ToJson1_Data)obj;

            bool flag = false;

            Util.AppendLine("{");

            if (data.b != default)
            {
                JsonParser.AppendJsonKey("b", depth);
                JsonParser.AppendJsonValue(data.b, depth);
            }

            if (data.n != default)
            {
                JsonParser.AppendJsonKey("n", depth);
                JsonParser.AppendJsonValue(data.n, depth);
            }

            if (data.s != default)
            {
                JsonParser.AppendJsonKey("s", depth);
                JsonParser.AppendJsonValue(data.s, depth);
            }


            if (data.intDict != default)
            {
                JsonParser.AppendJsonKey("intDict", depth);
                Util.AppendLine("{");
                foreach (var item in data.intDict)
                {
                    flag = true;
                    JsonParser.AppendJsonKey(item.Key, depth + 1);
                    JsonParser.AppendJsonValue(item.Value);
                }
                if (flag == true)
                {
                    flag = false;
                    Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
                }
                Util.AppendLine("},", depth);
            }

            if (data.boolList != default)
            {
                JsonParser.AppendJsonKey("boolList", depth);
                Util.AppendLine("[");
                foreach (var item in data.boolList)
                {
                    flag = true;
                    JsonParser.AppendJsonValue(item, depth);
                }
                if (flag == true)
                {
                    flag = false;
                    Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
                }
                Util.AppendLine("]", depth);
            }

            if (data.d != default)
            {
                ToJson_ToJson1_Data(data.d, depth + 1);
            }

            

            Util.Append("}", depth - 1);
        }
    }

}
