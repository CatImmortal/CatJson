using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_ListItem Parse_Json2_ListItem()
        {
            Json2_ListItem obj = new Json2_ListItem();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_ListItem temp = (Json2_ListItem)userdata1;
                RangeString? rs;
                TokenType tokenType;
                switch (key.ToString())
                {
					case "datetime":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.datetime = rs.Value.ToString();
					break;
					
					case "remark":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.remark = rs.Value.ToString();
					break;
					
					case "zone":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.zone = rs.Value.ToString();
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
