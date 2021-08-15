using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static void ToJson_TestJson1_Root(object obj,int depth)
        {
            TestJson1_Root data = (TestJson1_Root)obj;
            bool flag = false;
            Util.AppendLine("{");

			flag = true;
			JsonParser.AppendJsonKey("b", depth + 1);
			JsonParser.AppendJsonValue(data.b);
			Util.AppendLine(",");
				
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
				
			if (data.intList != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("intList", depth + 1);
			Util.AppendLine("[");
			foreach (var item in data.intList)
			{
			Util.AppendTab(depth+1+1);
			JsonParser.AppendJsonValue(item);
			Util.AppendLine(",");
			}
			if (data.intList.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("]",depth+1);
			Util.AppendLine(",");
			}
				
			if (data.intDict != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("intDict", depth + 1);
			Util.AppendLine("{");
			foreach (var item in data.intDict)
			{
			JsonParser.AppendJsonKey(item.Key, depth+1+1);
			JsonParser.AppendJsonValue(item.Value);
			Util.AppendLine(",");
			}
			if (data.intDict.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("}",depth+1);
			Util.AppendLine(",");
			}
				
			if (data.item != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("item", depth + 1);
			ToJson_TestJson1_Item(data.item,depth+1);
			Util.AppendLine(",");
			}
				
			if (data.itemList != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("itemList", depth + 1);
			Util.AppendLine("[");
			foreach (var item in data.itemList)
			{
			Util.AppendTab(depth+1+1);
			if (item == null)
			{
			Util.Append("null");
			}
			else
			{
			ToJson_TestJson1_Item(item,depth+1+1);
			}
			Util.AppendLine(",");
			}
			if (data.itemList.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("]",depth+1);
			Util.AppendLine(",");
			}
				
			if (data.itemDict != default)
			{
			flag = true;
			JsonParser.AppendJsonKey("itemDict", depth + 1);
			Util.AppendLine("{");
			foreach (var item in data.itemDict)
			{
			JsonParser.AppendJsonKey(item.Key, depth+1+1);
			if (item.Value == null)
			{
			Util.Append("null");
			}
			else
			{
			ToJson_TestJson1_Item(item.Value,depth+1+1);
			}
			Util.AppendLine(",");
			}
			if (data.itemDict.Count > 0)
			{
			 Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}
			Util.Append("}",depth+1);
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
