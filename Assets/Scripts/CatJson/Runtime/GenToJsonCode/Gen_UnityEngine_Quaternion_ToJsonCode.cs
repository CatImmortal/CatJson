using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Quaternion(object obj,int depth)
        {
            UnityEngine.Quaternion data = (UnityEngine.Quaternion)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.x != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("x", depth + 1);
			JsonParser.AppendJsonValue(data.x);
			Util.AppendLine(",");
			}
				
			if (data.y != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("y", depth + 1);
			JsonParser.AppendJsonValue(data.y);
			Util.AppendLine(",");
			}
				
			if (data.z != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("z", depth + 1);
			JsonParser.AppendJsonValue(data.z);
			Util.AppendLine(",");
			}
				
			if (data.w != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("w", depth + 1);
			JsonParser.AppendJsonValue(data.w);
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
