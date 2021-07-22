using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_Root Parse_Json2_Root()
        {
            Json2_Root obj = new Json2_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_Root temp = (Json2_Root)userdata1;
                RangeString? rs;
                TokenType tokenType;
                switch (key.ToString())
                {
					case "resultcode":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.resultcode = rs.Value.ToString();
					break;
					
					case "reason":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.reason = rs.Value.ToString();
					break;
					
					case "result":
					temp.result = Parse_Json2_Result();
					break;
					
					case "error_code":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.error_code = System.Int32.Parse(rs.Value.ToString());
					break;
					

                    default:
                        JsonParser.ParseJsonValue(nextTokenType);
                        break;
                }
            });


            return obj;
        }
    }

}
