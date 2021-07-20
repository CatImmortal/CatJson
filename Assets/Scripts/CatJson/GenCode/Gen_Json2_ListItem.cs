using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class Gen
    {
        private static Json2_ListItem Gen_Json2_ListItem(JsonLexer lexer)
        {
            Json2_ListItem obj = new Json2_ListItem();

            //跳过 {
            lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                string key = lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();

                switch (key)
                {
                    case "datetime":
                        obj.datetime = lexer.GetNextToken(out _);
                        break;
                    case "remark":
                        obj.remark = lexer.GetNextToken(out _);
                        break;
                    case "zone":
                        obj.zone = lexer.GetNextToken(out _);
                        break;

                    default:
                        lexer.GetNextToken(out _);
                        break;
                }


                //有逗号就跳过逗号
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
                    //没有逗号就说明结束了
                    break;
                }

            }

            //跳过 }
            lexer.GetNextTokenByType(TokenType.RightBrace);

            return obj;
        }
    }

}
