using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_AnimationCurve(object obj,int depth)
        {
            UnityEngine.AnimationCurve data = (UnityEngine.AnimationCurve)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.keys != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("keys", depth + 1);
			Util.AppendLine("[");
			foreach (var item in data.keys)
			{
			Util.AppendTab(depth+1+1);
			ToJson_UnityEngine_Keyframe(item,depth+1+1);
			Util.AppendLine(",");
			}
			if (data.keys.Length > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("]",depth+1);
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
