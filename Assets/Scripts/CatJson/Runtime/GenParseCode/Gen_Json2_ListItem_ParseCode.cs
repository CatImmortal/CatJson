using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class ParseCode
    {
        private static Json2_ListItem Parse_Json2_ListItem()
        {
            Json2_ListItem obj = new Json2_ListItem();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_ListItem temp = (Json2_ListItem)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("datetime")))
				{
				temp.datetime = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
				}
				else if (key.Equals(new RangeString("remark")))
				{
				temp.remark = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
				}
				else if (key.Equals(new RangeString("zone")))
				{
				temp.zone = JsonParser.Lexer.GetNextToken(out tokenType).ToString();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
