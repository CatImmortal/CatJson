using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static TestJson1_Root ParseJson_TestJson1_Root()
        {
            TestJson1_Root obj = new TestJson1_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                TestJson1_Root temp = (TestJson1_Root)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("b")))
				{
				JsonParser.Lexer.GetNextToken(out tokenType);
				temp.b = tokenType == TokenType.True;
				}
				else if (key.Equals(new RangeString("num")))
				{
				temp.num = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("str")))
				{
				temp.str = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
				}
				else if (key.Equals(new RangeString("intList")))
				{
				List<System.Int32> list = new List<System.Int32>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata21, nextTokenType1) =>
				{
				((List<System.Int32>)userdata11).Add(System.Int32.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString()));
				});
				temp.intList = list;
				}
				else if (key.Equals(new RangeString("intDict")))
				{
				Dictionary<string,System.Int32> dict = new Dictionary<string,System.Int32>();
				JsonParser.ParseJsonObjectProcedure(dict, null, (userdata11, userdata21,key1, nextTokenType1) =>
				{
				((Dictionary<string, System.Int32>)userdata11).Add(key1.ToString(), System.Int32.Parse(JsonParser.Lexer.GetNextToken(out _).ToString()));
				});
				temp.intDict = dict;
				}
				else if (key.Equals(new RangeString("item")))
				{
				temp.item = ParseJson_TestJson1_Item();
				}
				else if (key.Equals(new RangeString("itemList")))
				{
				List<TestJson1_Item> list = new List<TestJson1_Item>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata21, nextTokenType1) =>
				{
				((List<TestJson1_Item>)userdata11).Add(ParseJson_TestJson1_Item());
				});
				temp.itemList = list;
				}
				else if (key.Equals(new RangeString("itemDict")))
				{
				Dictionary<string,TestJson1_Item> dict = new Dictionary<string,TestJson1_Item>();
				JsonParser.ParseJsonObjectProcedure(dict, null, (userdata11, userdata21,key1, nextTokenType1) =>
				{
				((Dictionary<string, TestJson1_Item>)userdata11).Add(key1.ToString(),ParseJson_TestJson1_Item());
				});
				temp.itemDict = dict;
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
