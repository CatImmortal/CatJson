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

            //自定义扩展主要针对json object格式的json文本，即{"key":value,...}
            //添加自定义扩展的用处主要有2个
            //1.key value不能直接用反射解析为类对象的字段名和值
            //2.需要通过手写解析代码来加速解析运行

            //加速Vector3的解析
            extendParseFuncDict.Add(typeof(Vector3), () =>
            {
                Vector3 v3 = new Vector3();

                //跳过 {
                Lexer.GetNextTokenByType(TokenType.LeftBrace);

                while (Lexer.LookNextTokenType() != TokenType.RightBrace)
                {
                    //提取key
                    RangeString key = Lexer.GetNextTokenByType(TokenType.String).Value;

                    //跳过 :
                    Lexer.GetNextTokenByType(TokenType.Colon);

                    //识别需要反序列化的字段值
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

        }
    }

}
