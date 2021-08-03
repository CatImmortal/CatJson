using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Rect(UnityEngine.Rect obj,int depth)
        {
            UnityEngine.Rect data = (UnityEngine.Rect)obj;
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
				
			if (data.width != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("width", depth + 1);
			JsonParser.AppendJsonValue(data.width);
			Util.AppendLine(",");
			}
				
			if (data.height != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("height", depth + 1);
			JsonParser.AppendJsonValue(data.height);
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
