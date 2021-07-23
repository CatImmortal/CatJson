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
                RangeString? rs;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("datetime")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.datetime = rs.Value.ToString();
				}
				else if (key.Equals(new RangeString("remark")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.remark = rs.Value.ToString();
				}
				else if (key.Equals(new RangeString("zone")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.zone = rs.Value.ToString();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
