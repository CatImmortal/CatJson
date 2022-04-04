using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodesHelper
    {
        private static void ToJson_TestJson1_Root(object obj,int depth)
        {
            TestJson1_Root data = (TestJson1_Root)obj;
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
				TextUtil.AppendLine(",");
			}
				
			if (data.intList != default)
			{
				JsonParser.AppendJsonKey("intList", depth + 1);
				TextUtil.AppendLine("[");
				int index = 0;
				foreach (var item in data.intList)
				{
					TextUtil.AppendTab(depth+1+1);
					JsonParser.AppendJsonValue(item);
					if (index < data.intList.Count-1)
					{
						TextUtil.AppendLine(",");
					}
					index++;
				}
				TextUtil.AppendLine(string.Empty);
				TextUtil.Append("]",depth+1);
				TextUtil.AppendLine(",");
			}
				
			if (data.intDict != default)
			{
				JsonParser.AppendJsonKey("intDict", depth + 1);
				TextUtil.AppendLine("{");
				int index = 0;
				foreach (var item in data.intDict)
				{
					JsonParser.AppendJsonKey(item.Key, depth+1+1);
					JsonParser.AppendJsonValue(item.Value);
					if (index < data.intDict.Count-1)
					{
						TextUtil.AppendLine(",");
					}
					index++;
				}
				TextUtil.AppendLine(string.Empty);
				TextUtil.Append("}",depth+1);
				TextUtil.AppendLine(",");
			}
				
			if (data.item != default)
			{
				JsonParser.AppendJsonKey("item", depth + 1);
				ToJson_TestJson1_Item(data.item,depth+1);
				TextUtil.AppendLine(",");
			}
				
			if (data.itemList != default)
			{
				JsonParser.AppendJsonKey("itemList", depth + 1);
				TextUtil.AppendLine("[");
				int index = 0;
				foreach (var item in data.itemList)
				{
					TextUtil.AppendTab(depth+1+1);
					if (item == null)
					{
						TextUtil.Append("null");
					}
					else
					{
						ToJson_TestJson1_Item(item,depth+1+1);
					}
					if (index < data.itemList.Count-1)
					{
						TextUtil.AppendLine(",");
					}
					index++;
				}
				TextUtil.AppendLine(string.Empty);
				TextUtil.Append("]",depth+1);
				TextUtil.AppendLine(",");
			}
				
			if (data.itemDict != default)
			{
				JsonParser.AppendJsonKey("itemDict", depth + 1);
				TextUtil.AppendLine("{");
				int index = 0;
				foreach (var item in data.itemDict)
				{
					JsonParser.AppendJsonKey(item.Key, depth+1+1);
					if (item.Value == null)
					{
						TextUtil.Append("null");
					}
					else
					{
						ToJson_TestJson1_Item(item.Value,depth+1+1);
					}
					if (index < data.itemDict.Count-1)
					{
						TextUtil.AppendLine(",");
					}
					index++;
				}
				TextUtil.AppendLine(string.Empty);
				TextUtil.Append("}",depth+1);
				
			}
				

            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("}", depth);
        }
    }

}
