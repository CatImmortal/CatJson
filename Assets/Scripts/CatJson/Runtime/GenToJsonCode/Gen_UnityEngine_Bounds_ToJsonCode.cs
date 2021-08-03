using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Bounds(object obj,int depth)
        {
            UnityEngine.Bounds data = (UnityEngine.Bounds)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.center != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("center", depth + 1);
			ToJson_UnityEngine_Vector3(data.center,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.size != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("size", depth + 1);
			ToJson_UnityEngine_Vector3(data.size,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.extents != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("extents", depth + 1);
			ToJson_UnityEngine_Vector3(data.extents,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.min != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("min", depth + 1);
			ToJson_UnityEngine_Vector3(data.min,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.max != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("max", depth + 1);
			ToJson_UnityEngine_Vector3(data.max,depth+1);
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
