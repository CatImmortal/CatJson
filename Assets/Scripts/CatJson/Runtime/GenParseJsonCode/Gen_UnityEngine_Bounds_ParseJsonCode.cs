using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityEngine.Bounds ParseJson_UnityEngine_Bounds()
        {
            UnityEngine.Bounds temp = new UnityEngine.Bounds();
            TokenType tokenType;

            JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (JsonParser.Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
               

                RangeString key = JsonParser.Lexer.GetNextTokenByType(TokenType.String);

                JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);

				if (key.Equals(new RangeString("center")))
				{
				temp.center = ParseJson_UnityEngine_Vector3();
				}
				else if (key.Equals(new RangeString("size")))
				{
				temp.size = ParseJson_UnityEngine_Vector3();
				}

                else{
                    JsonParser.ParseJsonValue(JsonParser.Lexer.LookNextTokenType());
                }


                if (JsonParser.Lexer.LookNextTokenType() == TokenType.Comma)
                {
                    JsonParser.Lexer.GetNextTokenByType(TokenType.Comma);

                    if (JsonParser.Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new System.Exception("Json对象不能以逗号结尾");
                    }
                }
                else
                {
                    break;
                }

            }

            JsonParser.Lexer.GetNextTokenByType(TokenType.RightBrace);

            return temp;
        }
    }

}
