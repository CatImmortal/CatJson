using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_Root Gen_Json2_Root(JsonLexer lexer)
        {
            Json2_Root obj = new Json2_Root();

            lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                string key = lexer.GetNextTokenByType(TokenType.String).Value.ToString();

                lexer.GetNextTokenByType(TokenType.Colon);

                TokenType nextTokenType = lexer.LookNextTokenType();

                switch (key)
                {
                    case "resultcode":
                        obj.resultcode = lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "reason":
                        obj.reason = lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "result":
                        obj.result = Gen_Json2_Result(lexer);
                        break;
                    case "error_code":
                        string token = lexer.GetNextToken(out _).Value.ToString();
                        obj.error_code = int.Parse(token);
                        break;
                    default:
                        JsonParser.ParseJsonValue(nextTokenType);
                        break;
                }

                if (lexer.LookNextTokenType() == TokenType.Comma)
                {
                    lexer.GetNextTokenByType(TokenType.Comma);

                    if (lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("Json对象不能以逗号结尾");
                    }
                }
                else
                {
                    break;
                }

            }
            lexer.GetNextTokenByType(TokenType.RightBrace);

            return obj;
        }
    }

}
