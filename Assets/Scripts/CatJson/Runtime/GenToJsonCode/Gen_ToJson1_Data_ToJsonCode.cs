using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_ToJson1_Data(object obj,int depth)
        {
            ToJson1_Data data = (ToJson1_Data)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.b != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("b", depth);
			JsonParser.AppendJsonValue(data.b);
			Util.AppendLine(",");
			}
			if (data.n != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("n", depth);
			JsonParser.AppendJsonValue(data.n);
			Util.AppendLine(",");
			}
			if (data.s != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("s", depth);
			JsonParser.AppendJsonValue(data.s);
			Util.AppendLine(",");
			}
			if (data.intDict != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("intDict", depth);
			Util.AppendLine("{");
			foreach (var item in data.intDict)
			{
			JsonParser.AppendJsonKey(item.Key, depth+1);
			JsonParser.AppendJsonValue(item.Value);
			Util.AppendLine(",");
			}
			if (data.intDict.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("}",depth);
			Util.AppendLine(",");
			}
			if (data.boolList != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("boolList", depth);
			Util.AppendLine("[");
			foreach (var item in data.boolList)
			{
			Util.AppendTab(depth+1);
			JsonParser.AppendJsonValue(item);
			Util.AppendLine(",");
			}
			if (data.boolList.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("]",depth);
			Util.AppendLine(",");
			}
			if (data.d != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("d", depth);
			ToJson_ToJson1_Data(data.d,depth + 1);
			Util.AppendLine(",");
			}


            if (flag)
            {
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }

            Util.Append("}", depth - 1);
         
        }
    }

}
