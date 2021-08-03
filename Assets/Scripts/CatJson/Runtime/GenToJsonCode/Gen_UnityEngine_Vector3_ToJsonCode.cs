using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Vector3(UnityEngine.Vector3 obj,int depth)
        {
            UnityEngine.Vector3 data = (UnityEngine.Vector3)obj;
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
				


            if (flag)
            {
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }

            Util.Append("}", depth);
         
        }
    }

}
