using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityEngine.Vector3 ParseJson_UnityEngine_Vector3()
        {
            UnityEngine.Vector3 temp = new UnityEngine.Vector3();
            TokenType tokenType;

            JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (JsonParser.Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
               

                RangeString key = JsonParser.Lexer.GetNextTokenByType(TokenType.String);

                JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);

				if (key.Equals(new RangeString("x")))
				{
				temp.x = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("y")))
				{
				temp.y = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("z")))
				{
				temp.z = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
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
