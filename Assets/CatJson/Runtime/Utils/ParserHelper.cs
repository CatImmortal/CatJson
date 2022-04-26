using System;

namespace CatJson
{
    public static class ParserHelper
    {
        /// <summary>
        /// 解析Json键值对的通用流程
        /// </summary>
        public static void ParseJsonKeyValuePairProcedure(object userdata1,object userdata2,bool isIntKey,Action<object,object,bool,RangeString> action)
        {
            //跳过 {
            JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while ( JsonParser.Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                RangeString key =  JsonParser.Lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                JsonParser.Lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                action(userdata1,userdata2,isIntKey,key);

                //此时value已经被提取了
                
                //有逗号就跳过逗号
                if ( JsonParser.Lexer.LookNextTokenType() == TokenType.Comma)
                {
                     JsonParser.Lexer.GetNextTokenByType(TokenType.Comma);

                    if ( JsonParser.Lexer.LookNextTokenType() == TokenType.RightBracket)
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
             JsonParser.Lexer.GetNextTokenByType(TokenType.RightBrace);
        }

        /// <summary>
        /// 解析Json数组的通用流程
        /// </summary>
        public static void ParseJsonArrayProcedure(object userdata1,object userdata2, Action<object,object> action)
        {
            //跳过[
             JsonParser.Lexer.GetNextTokenByType(TokenType.LeftBracket);

            while ( JsonParser.Lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                action(userdata1,userdata2);

                //此时value已经被提取了
                
                //有逗号就跳过
                if ( JsonParser.Lexer.LookNextTokenType() == TokenType.Comma)
                {
                     JsonParser.Lexer.GetNextTokenByType(TokenType.Comma);

                    if ( JsonParser.Lexer.LookNextTokenType() == TokenType.RightBracket)
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
             JsonParser.Lexer.GetNextTokenByType(TokenType.RightBracket);
        }
    }
}