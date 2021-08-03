using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityJson_Data(object obj,int depth)
        {
            UnityJson_Data data = (UnityJson_Data)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.v2 != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("v2", depth + 1);
			ToJson_UnityEngine_Vector2(data.v2,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.v3 != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("v3", depth + 1);
			ToJson_UnityEngine_Vector3(data.v3,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.v4 != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("v4", depth + 1);
			ToJson_UnityEngine_Vector4(data.v4,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.rotate != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("rotate", depth + 1);
			ToJson_UnityEngine_Quaternion(data.rotate,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.color != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("color", depth + 1);
			ToJson_UnityEngine_Color(data.color,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.bounds != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("bounds", depth + 1);
			ToJson_UnityEngine_Bounds(data.bounds,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.rect != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("rect", depth + 1);
			ToJson_UnityEngine_Rect(data.rect,depth+1);
			Util.AppendLine(",");
			}
				
			flag = true;
			JsonParser.AppendJsonKey("keyframe", depth + 1);
			ToJson_UnityEngine_Keyframe(data.keyframe,depth+1);
			Util.AppendLine(",");
				
			if (data.ac != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("ac", depth + 1);
			ToJson_UnityEngine_AnimationCurve(data.ac,depth+1);
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
