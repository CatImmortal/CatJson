using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class Gen
    {
        private static Json2_Result Gen_Json2_Result(JsonLexer lexer)
        {
            Json2_Result obj = new Json2_Result();

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
                    case "company":
                        obj.company = lexer.GetNextToken(out _);
                        break;
                    case "com":
                        obj.com = lexer.GetNextToken(out _); 
                        break;
                    case "no":
                        obj.no = lexer.GetNextToken(out _);
                        break;
                    case "list":
                        List<Json2_ListItem> list = new List<Json2_ListItem>();
                        obj.list = list;

                        //跳过[
                        lexer.GetNextTokenByType(TokenType.LeftBracket);

                        while (lexer.LookNextTokenType() != TokenType.RightBracket)
                        {
                            //提取value
                            //array和json obj需要完整的[]和{}，所以只能look
                            Json2_ListItem item = Gen_Json2_ListItem(lexer);

                            list.Add(item);

                            //有逗号就跳过
                            if (lexer.LookNextTokenType() == TokenType.Comma)
                            {
                                lexer.GetNextTokenByType(TokenType.Comma);

                                if (lexer.LookNextTokenType() == TokenType.RightBracket)
                                {
                                    throw new Exception("数组不能以逗号结尾");
                                }
                            }
                            else
                            {
                                //没有逗号就说明结束了
                                break;
                            }
                        }

                        //跳过]
                        lexer.GetNextTokenByType(TokenType.RightBracket);

                        break;
                    case "status":
                        string token = lexer.GetNextToken(out _);
                        obj.status = int.Parse(token);
                        break;
                    default:
                        JsonParser.ParseJsonValue(nextTokenType);
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
