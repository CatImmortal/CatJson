using System;
using System.Collections.Generic;


namespace CatJson
{
	public static partial class GenJsonCodes
	{
		private static void ToJson_ToJson1_Data(object obj, int depth)
		{
			ToJson1_Data data = (ToJson1_Data)obj;
			bool flag = false;
			Util.AppendLine("{");

			if (data.b != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("b", depth);
				JsonParser.AppendJsonValue(data.b);
				Util.AppendLine(",");
			}
			if (data.n != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("n", depth);
				JsonParser.AppendJsonValue(data.n);
				Util.AppendLine(",");
			}
			if (data.s != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("s", depth);
				JsonParser.AppendJsonValue(data.s);
				Util.AppendLine(",");
			}
			if (data.intDict != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("intDict", depth);
				Util.AppendLine("{");
				foreach (var item in data.intDict)
				{
					JsonParser.AppendKeyJson(item.Key, depth + 1);
					JsonParser.AppendJsonValue(item.Value);
					Util.AppendLine(",");
				}
				if (data.intDict.Count > 0)
				{
					Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
				}
				Util.Append("}", depth);
				Util.AppendLine(",");
			}
			if (data.boolList != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("boolList", depth);
				Util.AppendLine("[");
				foreach (var item in data.boolList)
				{
					Util.AppendTab(depth + 1);
					JsonParser.AppendJsonValue(item);
					Util.AppendLine(",");
				}
				if (data.boolList.Count > 0)
				{
					Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
				}
				Util.Append("]", depth);
				Util.AppendLine(",");
			}
			if (data.d != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("d", depth);
				ToJson_ToJson1_Data(data.d, depth + 1);
				Util.AppendLine(",");
			}
			if (data.dictDcit != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("dictDcit", depth);
				Util.AppendLine("{");
				foreach (var item in data.dictDcit)
				{
					JsonParser.AppendKeyJson(item.Key, depth + 1);
					Util.AppendLine("{");
					foreach (var item1 in item.Value)
					{
						JsonParser.AppendKeyJson(item1.Key, depth + 1 + 1);
						ToJson_ToJson1_Data(item1.Value, depth + 1 + 1 + 1);
						Util.AppendLine(",");
					}
					if (item.Value.Count > 0)
					{
						Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
					}
					Util.Append("}", depth);
					Util.AppendLine(",");
				}
				if (data.dictDcit.Count > 0)
				{
					Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
				}
				Util.Append("}", depth);
				Util.AppendLine(",");
			}
			if (data.listList != default)
			{
				flag = true;
				JsonParser.AppendKeyJson("listList", depth);
				Util.AppendLine("[");
				foreach (var item in data.listList)
				{
					Util.AppendTab(depth + 1);
					Util.AppendLine("[");
					foreach (var item1 in item)
					{
						Util.AppendTab(depth + 1 + 1);
						JsonParser.AppendJsonValue(item1);
						Util.AppendLine(",");
					}
					if (item.Count > 0)
					{
						Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
					}
					Util.Append("]", depth);
					Util.AppendLine(",");
				}
				if (data.listList.Count > 0)
				{
					Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
				}
				Util.Append("]", depth);
				Util.AppendLine(",");
			}


			if (flag)
			{
				Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
			}

			Util.Append("}", depth - 1);

		}
	}

}
