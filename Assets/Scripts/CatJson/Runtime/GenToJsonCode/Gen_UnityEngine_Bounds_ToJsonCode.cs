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
				


            if (flag)
            {
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }

            Util.Append("}", depth);
         
        }
    }

}
