using System;
using System.Collections.Generic;


namespace CatJson
{
	public static partial class ParseCode
	{
		private static Json7_Data Parse_Json7_Data()
		{
			Json7_Data obj = new Json7_Data();

			JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
			{
				Json7_Data temp = (Json7_Data)userdata1;
				RangeString? rs;
				TokenType tokenType;

				if (key.Equals(new RangeString("dict1")))
				{
					Dictionary<string, System.String> dict = new Dictionary<string, System.String>();
					JsonParser.ParseJsonObjectProcedure(dict, null, (userdata11, userdata21, key1, nextTokenType1) =>
					{
						((Dictionary<string, System.String>)userdata11).Add(key1.ToString(), JsonParser.Lexer.GetNextToken(out _).Value.ToString());
					});
					temp.dict1 = dict;
				}
				else if (key.Equals(new RangeString("dict2")))
				{
					Dictionary<string, Dictionary<string, System.Int32>> dict = new Dictionary<string, Dictionary<string, System.Int32>>();
					JsonParser.ParseJsonObjectProcedure(dict, null, (userdata11, userdata21, key1, nextTokenType1) =>
					{
						Dictionary<string, System.Int32> dict1 = new Dictionary<string, System.Int32>();
						JsonParser.ParseJsonObjectProcedure(dict1, null, (userdata111, userdata211, key11, nextTokenType11) =>
					{
						((Dictionary<string, System.Int32>)userdata111).Add(key11.ToString(), System.Int32.Parse(JsonParser.Lexer.GetNextToken(out _).Value.ToString()));
					});
						((Dictionary<string, Dictionary<string, System.Int32>>)userdata11).Add(key1.ToString(), dict1);
					});
					temp.dict2 = dict;
				}
				else if (key.Equals(new RangeString("dict3")))
				{
					Dictionary<string, Dictionary<string, Dictionary<string, System.Boolean>>> dict = new Dictionary<string, Dictionary<string, Dictionary<string, System.Boolean>>>();
					JsonParser.ParseJsonObjectProcedure(dict, null, (userdata11, userdata21, key1, nextTokenType1) =>
					{
						Dictionary<string, Dictionary<string, System.Boolean>> dict1 = new Dictionary<string, Dictionary<string, System.Boolean>>();
						JsonParser.ParseJsonObjectProcedure(dict1, null, (userdata111, userdata211, key11, nextTokenType11) =>
					{
						Dictionary<string, System.Boolean> dict11 = new Dictionary<string, System.Boolean>();
						JsonParser.ParseJsonObjectProcedure(dict11, null, (userdata1111, userdata2111, key111, nextTokenType111) =>
					{
						JsonParser.Lexer.GetNextToken(out tokenType);
						((Dictionary<string, System.Boolean>)userdata1111).Add(key111.ToString(), tokenType == TokenType.True);
					});
						((Dictionary<string, Dictionary<string, System.Boolean>>)userdata111).Add(key11.ToString(), dict11);
					});
						((Dictionary<string, Dictionary<string, Dictionary<string, System.Boolean>>>)userdata11).Add(key1.ToString(), dict1);
					});
					temp.dict3 = dict;
				}

				else
				{
					JsonParser.ParseJsonValue(nextTokenType);
				}

			});


			return obj;
		}
	}

}
