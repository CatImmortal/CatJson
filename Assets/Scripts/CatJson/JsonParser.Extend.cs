using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
namespace CatJson
{
    public static partial class JsonParser
    {
        static JsonParser()
        {
            #region 第一种扩展方法
            //扩展Vector3的解析
            extendParseFuncDict.Add(typeof(Vector3), () =>
            {
                Vector3 v3 = new Vector3();

                ////这种写法会装箱
                //ParseJsonObjectProcedure(null, null, (userdata1, userdata2, key, nextTokenType) =>
                //{
                //    switch (key.ToString())
                //    {
                //        case "x":
                //            string token = Lexer.GetNextToken(out _).Value.ToString();
                //            v3.x = float.Parse(token);
                //            break;
                //        case "y":
                //            token = Lexer.GetNextToken(out _).Value.ToString();
                //            v3.y = float.Parse(token);
                //            break;
                //        case "z":
                //            token = Lexer.GetNextToken(out _).Value.ToString();
                //            v3.z = float.Parse(token);
                //            break;

                //        default:
                //            throw new Exception("Vector3解析中发现了额外key");
                //    }
                //});


                //这种写法不会装箱，但比较繁琐 

                //跳过 {
                Lexer.GetNextTokenByType(TokenType.LeftBrace);

                while (Lexer.LookNextTokenType() != TokenType.RightBrace)
                {
                    //提取key
                    RangeString key = Lexer.GetNextTokenByType(TokenType.String).Value;

                    //跳过 :
                    Lexer.GetNextTokenByType(TokenType.Colon);

                    switch (key.ToString())
                    {
                        case "x":
                            string token = Lexer.GetNextToken(out _).Value.ToString();
                            v3.x = float.Parse(token);
                            break;
                        case "y":
                            token = Lexer.GetNextToken(out _).Value.ToString();
                            v3.y = float.Parse(token);
                            break;
                        case "z":
                            token = Lexer.GetNextToken(out _).Value.ToString();
                            v3.z = float.Parse(token);
                            break;

                        default:
                            throw new Exception("Vector3解析中发现了额外key");
                    }


                    //有逗号就跳过逗号
                    if (Lexer.LookNextTokenType() == TokenType.Comma)
                    {
                        Lexer.GetNextTokenByType(TokenType.Comma);

                        if (Lexer.LookNextTokenType() == TokenType.RightBracket)
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
                Lexer.GetNextTokenByType(TokenType.RightBrace);

                return v3;
            });
            #endregion




        }
    }

}
