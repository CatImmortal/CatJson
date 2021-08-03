using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Color(object obj,int depth)
        {
            UnityEngine.Color data = (UnityEngine.Color)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.r != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("r", depth + 1);
			JsonParser.AppendJsonValue(data.r);
			Util.AppendLine(",");
			}
				
			if (data.g != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("g", depth + 1);
			JsonParser.AppendJsonValue(data.g);
			Util.AppendLine(",");
			}
				
			if (data.b != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("b", depth + 1);
			JsonParser.AppendJsonValue(data.b);
			Util.AppendLine(",");
			}
				
			if (data.a != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("a", depth + 1);
			JsonParser.AppendJsonValue(data.a);
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
