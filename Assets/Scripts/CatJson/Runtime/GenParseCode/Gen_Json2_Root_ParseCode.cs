using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class ParseCode
    {
        private static Json2_Root Parse_Json2_Root()
        {
            Json2_Root obj = new Json2_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_Root temp = (Json2_Root)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("resultcode")))
				{
				temp.resultcode = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
				}
				else if (key.Equals(new RangeString("reason")))
				{
				temp.reason = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
				}
				else if (key.Equals(new RangeString("result")))
				{
				temp.result = Parse_Json2_Result();
				}
				else if (key.Equals(new RangeString("error_code")))
				{
				temp.error_code = System.Int32.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
