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

            //添加自定义扩展解析方法的作用主要有2个
            //1.json值不能直接用反射解析为默认的类型，比如"2016/5/9 13:09:55"可能需要被解析为DateTime而不是string
            //2.不使用反射，通过手写解析代码来加速解析运行
            
            //解析DateTime
            extensionParseFuncDict.Add(typeof(DateTime), () =>
            {
                RangeString rs = Lexer.GetNextTokenByType(TokenType.String);
                return DateTime.Parse(rs.ToString());
            });


            //加速Vector3的解析运行
            extensionParseFuncDict.Add(typeof(Vector3), () =>
             {
                 Vector3 v3 = new Vector3();

                 //跳过 {
                 Lexer.GetNextTokenByType(TokenType.LeftBrace);

                 while (Lexer.LookNextTokenType() != TokenType.RightBrace)
                 {
                     //提取key
                     RangeString key = Lexer.GetNextTokenByType(TokenType.String);

                     //跳过 :
                     Lexer.GetNextTokenByType(TokenType.Colon);

                     //主要手写这段if else
                     //识别需要反序列化的字段值 
                     if (key.Equals(new RangeString("x")))
                     {
                         string token = Lexer.GetNextToken(out _).ToString();
                         v3.x = float.Parse(token);
                     }
                     else if (key.Equals(new RangeString("y")))
                     {
                         string token = Lexer.GetNextToken(out _).ToString();
                         v3.y = float.Parse(token);
                     }
                     else if (key.Equals(new RangeString("z")))
                     {
                         string token = Lexer.GetNextToken(out _).ToString();
                         v3.z = float.Parse(token);
                     }
                     else
                     {
                         //跳过
                         ParseJsonValue(Lexer.LookNextTokenType());
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
