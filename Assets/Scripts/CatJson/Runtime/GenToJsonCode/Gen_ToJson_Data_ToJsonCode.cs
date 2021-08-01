using System;
using System.Collections.Generic;


namespace CatJson
{
	public static partial class GenJsonCodes
	{
		private static void ToJson_ToJson_Data(object obj, int depth)
		{
			ToJson_Data data = (ToJson_Data)obj;
			bool flag = false;
			Util.AppendLine("{");

			if (data.dataListList != default)
			{
				flag = true;
				JsonParser.AppendJsonKey("dataListList", depth + 1);
				Util.AppendLine("[");
				foreach (var item in data.dataListList)
				{
					Util.AppendTab(depth + 1 + 1);
					if (item == null)
					{
						Util.Append("null");
					}
					else
					{
						Util.AppendLine("[");
						foreach (var item1 in item)
						{
							Util.AppendTab(depth + 1 + 1 + 1);
							if (item1 == null)
							{
								Util.Append("null");
							}
							else
							{
								ToJson_ToJson_Data(item1, depth + 1 + 1 + 1);
							}
							Util.AppendLine(",");
						}
						if (item.Count > 0)
						{
							Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
						}
						Util.Append("]", depth + 1 + 1);
					}
					Util.AppendLine(",");
				}
				if (data.dataListList.Count > 0)
				{
					Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
				}
				Util.Append("]", depth + 1);
				Util.AppendLine(",");
			}
			if (data.dataDictDict != default)
			{
				flag = true;
				JsonParser.AppendJsonKey("dataDictDict", depth + 1);
				Util.AppendLine("{");
				foreach (var item in data.dataDictDict)
				{
					JsonParser.AppendJsonKey(item.Key, depth + 1 + 1);
					if (item.Value == null)
					{
						Util.Append("null");
					}
					else
					{
						Util.AppendLine("{");
						foreach (var item1 in item.Value)
						{
							JsonParser.AppendJsonKey(item1.Key, depth + 1 + 1 + 1);
							if (item1.Value == null)
							{
								Util.Append("null");
							}
							else
							{
								ToJson_ToJson_Data(item1.Value, depth + 1 + 1 + 1);
							}
							Util.AppendLine(",");
						}
						if (item.Value.Count > 0)
						{
							Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
						}
						Util.Append("}", depth + 1 + 1);
					}
					Util.AppendLine(",");
				}
				if (data.dataDictDict.Count > 0)
				{
					Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
				}
				Util.Append("}", depth + 1);
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
