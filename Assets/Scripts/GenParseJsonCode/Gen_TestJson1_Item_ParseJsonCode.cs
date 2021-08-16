using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodesHelper
    {
        private static TestJson1_Item ParseJson_TestJson1_Item()
        {
            if (JsonParser.Lexer.LookNextTokenType() == TokenType.Null)
            {
                JsonParser.Lexer.GetNextToken(out _);
				return null;
            }

            TestJson1_Item obj = new TestJson1_Item();

            JsonParser.ParseJsonObjectProcedure(obj, null,false, (userdata1, userdata2,isIntKey, key, nextTokenType) =>
            {
                TestJson1_Item temp = (TestJson1_Item)userdata1;
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

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
