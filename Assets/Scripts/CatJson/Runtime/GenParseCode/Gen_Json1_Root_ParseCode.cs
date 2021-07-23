using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class ParseCode
    {
        private static Json1_Root Parse_Json1_Root()
        {
            Json1_Root obj = new Json1_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json1_Root temp = (Json1_Root)userdata1;
                RangeString? rs;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("array")))
				{
				List<System.Int32> list = new List<System.Int32>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata22, nextTokenType2) =>
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				((List<System.Int32>)userdata11).Add(System.Int32.Parse(rs.Value.ToString()));
				});
				temp.array = list.ToArray();
				}
				else if (key.Equals(new RangeString("type")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.type = tokenType == TokenType.True;
				}
				else if (key.Equals(new RangeString("number")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.number = System.Int32.Parse(rs.Value.ToString());
				}
				else if (key.Equals(new RangeString("obj")))
				{
				temp.obj = Parse_Json1_Obj();
				}
				else if (key.Equals(new RangeString("str")))
				{
				rs = JsonParser.Lexer.GetNextToken(out tokenType);
				temp.str = rs.Value.ToString();
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
