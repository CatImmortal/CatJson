using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Rect(object obj,int depth)
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
				
			if (data.position != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("position", depth + 1);
			ToJson_UnityEngine_Vector2(data.position,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.center != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("center", depth + 1);
			ToJson_UnityEngine_Vector2(data.center,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.min != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("min", depth + 1);
			ToJson_UnityEngine_Vector2(data.min,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.max != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("max", depth + 1);
			ToJson_UnityEngine_Vector2(data.max,depth+1);
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
				
			if (data.size != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("size", depth + 1);
			ToJson_UnityEngine_Vector2(data.size,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.xMin != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("xMin", depth + 1);
			JsonParser.AppendJsonValue(data.xMin);
			Util.AppendLine(",");
			}
				
			if (data.yMin != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("yMin", depth + 1);
			JsonParser.AppendJsonValue(data.yMin);
			Util.AppendLine(",");
			}
				
			if (data.xMax != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("xMax", depth + 1);
			JsonParser.AppendJsonValue(data.xMax);
			Util.AppendLine(",");
			}
				
			if (data.yMax != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("yMax", depth + 1);
			JsonParser.AppendJsonValue(data.yMax);
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
