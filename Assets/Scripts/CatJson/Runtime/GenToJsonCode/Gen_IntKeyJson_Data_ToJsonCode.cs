using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_IntKeyJson_Data(object obj,int depth)
        {
            IntKeyJson_Data data = (IntKeyJson_Data)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.dict != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("dict", depth + 1);
			Util.AppendLine("{");
			foreach (var item in data.dict)
			{
			JsonParser.AppendJsonKey(item.Key.ToString(), depth+1+1);
			if (item.Value == null)
			{
			Util.Append("null");
			}
			else
			{
			JsonParser.AppendJsonValue(item.Value);
			}
			Util.AppendLine(",");
			}
			if (data.dict.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("}",depth+1);
			Util.AppendLine(",");
			}
				


            if (flag)
            {
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }

            Util.Append("}", depth);
         
        }
    }

}
