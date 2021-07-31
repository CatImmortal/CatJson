using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static ToJson1_Data ParseJson_ToJson1_Data()
        {
            ToJson1_Data obj = new ToJson1_Data();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                ToJson1_Data temp = (ToJson1_Data)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("b")))
				{
				JsonParser.Lexer.GetNextToken(out tokenType);
				temp.b = tokenType == TokenType.True;
				}
				else if (key.Equals(new RangeString("n")))
				{
				temp.n = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("s")))
				{
				temp.s = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
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
				else if (key.Equals(new RangeString("boolList")))
				{
				List<System.Boolean> list = new List<System.Boolean>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata21, nextTokenType1) =>
				{
				JsonParser.Lexer.GetNextToken(out tokenType);
				((List<System.Boolean>)userdata11).Add(tokenType == TokenType.True);
				});
				temp.boolList = list;
				}
				else if (key.Equals(new RangeString("d")))
				{
				temp.d = ParseJson_ToJson1_Data();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
