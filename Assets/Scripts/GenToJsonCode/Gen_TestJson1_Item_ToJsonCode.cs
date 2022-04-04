using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodesHelper
    {
        private static void ToJson_TestJson1_Item(object obj,int depth)
        {
            TestJson1_Item data = (TestJson1_Item)obj;
            TextUtil.AppendLine("{");

			if (data.b != default)
			{
				JsonParser.AppendJsonKey("b", depth + 1);
				JsonParser.AppendJsonValue(data.b);
				TextUtil.AppendLine(",");
			}
				
			if (Math.Abs(data.num - default(float)) > 1E-6f)
			{
				JsonParser.AppendJsonKey("num", depth + 1);
				JsonParser.AppendJsonValue(data.num);
				TextUtil.AppendLine(",");
			}
				
			if (data.str != default)
			{
				JsonParser.AppendJsonKey("str", depth + 1);
				JsonParser.AppendJsonValue(data.str);
				
			}
				

            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("}", depth);
        }
    }

}
