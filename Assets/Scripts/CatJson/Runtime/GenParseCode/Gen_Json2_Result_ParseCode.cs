using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class ParseCode
    {
        private static Json2_Result Parse_Json2_Result()
        {
            Json2_Result obj = new Json2_Result();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_Result temp = (Json2_Result)userdata1;
                RangeString? rs;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("company")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.company = rs.Value.ToString();
				}
				else if (key.Equals(new RangeString("com")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.com = rs.Value.ToString();
				}
				else if (key.Equals(new RangeString("no")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.no = rs.Value.ToString();
				}
				else if (key.Equals(new RangeString("list")))
				{
				List<Json2_ListItem> list = new List<Json2_ListItem>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata22, nextTokenType2) =>
				{
				((List<Json2_ListItem>)userdata11).Add(Parse_Json2_ListItem());
				});
				temp.list = list;
				}
				else if (key.Equals(new RangeString("status")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.status = System.Int32.Parse(rs.Value.ToString());
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
