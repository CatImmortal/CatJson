using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityEngine.Color ParseJson_UnityEngine_Color()
        {
            UnityEngine.Color temp = new UnityEngine.Color();
            TokenType tokenType;

            JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (JsonParser.Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
               

                RangeString key = JsonParser.Lexer.GetNextTokenByType(TokenType.String);

                JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);

				if (key.Equals(new RangeString("r")))
				{
				temp.r = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("g")))
				{
				temp.g = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("b")))
				{
				temp.b = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("a")))
				{
				temp.a = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
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
