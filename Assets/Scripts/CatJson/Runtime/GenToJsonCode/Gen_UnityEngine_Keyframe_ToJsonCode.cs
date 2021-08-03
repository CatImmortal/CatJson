using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_UnityEngine_Keyframe(object obj,int depth)
        {
            UnityEngine.Keyframe data = (UnityEngine.Keyframe)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.time != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("time", depth + 1);
			JsonParser.AppendJsonValue(data.time);
			Util.AppendLine(",");
			}
				
			if (data.value != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("value", depth + 1);
			JsonParser.AppendJsonValue(data.value);
			Util.AppendLine(",");
			}
				
			if (data.inTangent != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("inTangent", depth + 1);
			JsonParser.AppendJsonValue(data.inTangent);
			Util.AppendLine(",");
			}
				
			if (data.outTangent != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("outTangent", depth + 1);
			JsonParser.AppendJsonValue(data.outTangent);
			Util.AppendLine(",");
			}
				
			if (data.inWeight != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("inWeight", depth + 1);
			JsonParser.AppendJsonValue(data.inWeight);
			Util.AppendLine(",");
			}
				
			if (data.outWeight != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("outWeight", depth + 1);
			JsonParser.AppendJsonValue(data.outWeight);
			Util.AppendLine(",");
			}
				
			flag = true;
			JsonParser.AppendJsonKey("weightedMode", depth + 1);
			JsonParser.AppendJsonValue((int)data.weightedMode);
			Util.AppendLine(",");
				
			flag = true;
			JsonParser.AppendJsonKey("tangentMode", depth + 1);
			JsonParser.AppendJsonValue(data.tangentMode);
			Util.AppendLine(",");
				


            if (flag)
            {
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }

            Util.Append("}", depth);
         
        }
    }

}
