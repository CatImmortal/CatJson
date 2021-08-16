using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodesHelper
    {
        private static TestJson1_Root ParseJson_TestJson1_Root()
        {
            if (JsonParser.Lexer.LookNextTokenType() == TokenType.Null)
            {
                JsonParser.Lexer.GetNextToken(out _);
				return null;
            }

            TestJson1_Root obj = new TestJson1_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null,false, (userdata1, userdata2,isIntKey, key, nextTokenType) =>
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
				Dictionary<System.String,System.Int32> dict = new Dictionary<System.String,System.Int32>();
				JsonParser.ParseJsonObjectProcedure(dict, null,false, (userdata11, userdata21,isIntKey1,key1, nextTokenType1) =>
				{
				((Dictionary<System.String, System.Int32>)userdata11).Add(key1.ToString(), System.Int32.Parse(JsonParser.Lexer.GetNextToken(out _).ToString()));
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
				if (nextTokenType1 == TokenType.Null)
				{
				 JsonParser.Lexer.GetNextToken(out _);
				((List<TestJson1_Item>)userdata11).Add(null);
				return;
				}
				((List<TestJson1_Item>)userdata11).Add(ParseJson_TestJson1_Item());
				});
				temp.itemList = list;
				}
				else if (key.Equals(new RangeString("itemDict")))
				{
				Dictionary<System.String,TestJson1_Item> dict = new Dictionary<System.String,TestJson1_Item>();
				JsonParser.ParseJsonObjectProcedure(dict, null,false, (userdata11, userdata21,isIntKey1,key1, nextTokenType1) =>
				{
				if (nextTokenType1 == TokenType.Null)
				{
				 JsonParser.Lexer.GetNextToken(out _);
				((Dictionary<System.String, TestJson1_Item>)userdata11).Add(key1.ToString(),null);
				return;
				}
				((Dictionary<System.String, TestJson1_Item>)userdata11).Add(key1.ToString(),ParseJson_TestJson1_Item());
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
