using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static IntKeyJson_Data ParseJson_IntKeyJson_Data()
        {
            if (JsonParser.Lexer.LookNextTokenType() == TokenType.Null)
            {
                JsonParser.Lexer.GetNextToken(out _);
				return null;
            }

            IntKeyJson_Data obj = new IntKeyJson_Data();

            JsonParser.ParseJsonObjectProcedure(obj, null,false, (userdata1, userdata2,isIntKey, key, nextTokenType) =>
            {
                IntKeyJson_Data temp = (IntKeyJson_Data)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("dict")))
				{
				Dictionary<int,System.String> dict = new Dictionary<int,System.String>();
				JsonParser.ParseJsonObjectProcedure(dict, null,false, (userdata11, userdata21,isIntKey1,key1, nextTokenType1) =>
				{
				if (nextTokenType1 == TokenType.Null)
				{
				 JsonParser.Lexer.GetNextToken(out _);
				((Dictionary<System.Int32, System.String>)userdata11).Add(int.Parse(key1.ToString()),null);
				return;
				}
				((Dictionary<System.Int32, System.String>)userdata11).Add(int.Parse(key1.ToString()),JsonParser.Lexer.GetNextToken(out _).ToString());
				});
				temp.dict = dict;
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
