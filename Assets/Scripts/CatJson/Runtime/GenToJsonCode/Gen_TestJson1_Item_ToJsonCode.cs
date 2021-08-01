using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_TestJson1_Item(object obj,int depth)
        {
            TestJson1_Item data = (TestJson1_Item)obj;
            bool flag = false;
            Util.AppendLine("{");

			if (data.b != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("b", depth + 1);
			JsonParser.AppendJsonValue(data.b);
			Util.AppendLine(",");
			}
			if (data.num != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("num", depth + 1);
			JsonParser.AppendJsonValue(data.num);
			Util.AppendLine(",");
			}
			if (data.str != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("str", depth + 1);
			JsonParser.AppendJsonValue(data.str);
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
