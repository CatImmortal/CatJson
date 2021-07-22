using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class Generator
    {
        private static Json1_Obj Parse_Json1_Obj()
        {
            Json1_Obj obj = new Json1_Obj();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json1_Obj temp = (Json1_Obj)userdata1;
                RangeString? rs;
                TokenType tokenType;
                switch (key.ToString())
                {
					case "a":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.a = rs.Value.ToString();
					break;
					
					case "c":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.c = rs.Value.ToString();
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
