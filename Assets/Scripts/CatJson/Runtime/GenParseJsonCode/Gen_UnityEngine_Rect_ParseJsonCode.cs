using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityEngine.Rect ParseJson_UnityEngine_Rect()
        {
            UnityEngine.Rect temp = new UnityEngine.Rect();
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
				else if (key.Equals(new RangeString("position")))
				{
				temp.position = ParseJson_UnityEngine_Vector2();
				}
				else if (key.Equals(new RangeString("center")))
				{
				temp.center = ParseJson_UnityEngine_Vector2();
				}
				else if (key.Equals(new RangeString("min")))
				{
				temp.min = ParseJson_UnityEngine_Vector2();
				}
				else if (key.Equals(new RangeString("max")))
				{
				temp.max = ParseJson_UnityEngine_Vector2();
				}
				else if (key.Equals(new RangeString("width")))
				{
				temp.width = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("height")))
				{
				temp.height = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("size")))
				{
				temp.size = ParseJson_UnityEngine_Vector2();
				}
				else if (key.Equals(new RangeString("xMin")))
				{
				temp.xMin = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("yMin")))
				{
				temp.yMin = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("xMax")))
				{
				temp.xMax = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("yMax")))
				{
				temp.yMax = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
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
