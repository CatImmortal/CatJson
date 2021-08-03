using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static UnityEngine.Keyframe ParseJson_UnityEngine_Keyframe()
        {
            UnityEngine.Keyframe temp = new UnityEngine.Keyframe();
            TokenType tokenType;

            JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (JsonParser.Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
               

                RangeString key = JsonParser.Lexer.GetNextTokenByType(TokenType.String);

                JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);

				if (key.Equals(new RangeString("time")))
				{
				temp.time = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("value")))
				{
				temp.value = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("inTangent")))
				{
				temp.inTangent = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("outTangent")))
				{
				temp.outTangent = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("inWeight")))
				{
				temp.inWeight = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("outWeight")))
				{
				temp.outWeight = System.Single.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
				}
				else if (key.Equals(new RangeString("weightedMode")))
				{
				temp.weightedMode = (UnityEngine.WeightedMode)int.Parse(JsonParser.Lexer.GetNextToken(out _).ToString());
				}
				else if (key.Equals(new RangeString("tangentMode")))
				{
				temp.tangentMode = System.Int32.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());
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
