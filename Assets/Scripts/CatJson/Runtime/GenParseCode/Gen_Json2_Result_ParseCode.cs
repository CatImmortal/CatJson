using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_Result Parse_Json2_Result()
        {
            Json2_Result obj = new Json2_Result();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_Result temp = (Json2_Result)userdata1;
                RangeString? rs;
                TokenType tokenType;
                switch (key.ToString())
                {
					case "company":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.company = rs.Value.ToString();
					break;
					
					case "com":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.com = rs.Value.ToString();
					break;
					
					case "no":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.no = rs.Value.ToString();
					break;
					
					case "list":
					List<Json2_ListItem> list = new List<Json2_ListItem>();
					JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata22, nextTokenType2) =>
					{
					((List<Json2_ListItem>)userdata11).Add(Parse_Json2_ListItem());
					});
					temp.list = list;
					break;
					
					case "status":
					rs = JsonParser.Lexer.GetNextToken(out tokenType);
					temp.status = System.Int32.Parse(rs.Value.ToString());
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
