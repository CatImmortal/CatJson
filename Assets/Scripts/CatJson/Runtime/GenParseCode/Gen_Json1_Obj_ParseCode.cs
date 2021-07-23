using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class ParseCode
    {
        private static Json1_Obj Parse_Json1_Obj()
        {
            Json1_Obj obj = new Json1_Obj();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json1_Obj temp = (Json1_Obj)userdata1;
                RangeString? rs;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("a")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.a = rs.Value.ToString();
				}
				else if (key.Equals(new RangeString("c")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.c = rs.Value.ToString();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
