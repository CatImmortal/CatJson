using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// json解析器
    /// </summary>
    public static class JsonParser
    {
        private static JsonLexer lexer = new JsonLexer();
        private static List<JsonValue> cachedList = new List<JsonValue>();


        public static JsonObject ParseJson(string json)
        {
            lexer.SetJsonText(json);
            return ParseJsonObject();
        }

        /// <summary>
        /// 解析json对象
        /// </summary>
        private static JsonObject ParseJsonObject()
        {
            JsonObject obj = new JsonObject();

            //跳过 {
            lexer.GetNextTokenOfType(TokenType.LeftBrace);

            while (lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                string key = lexer.GetNextTokenOfType(TokenType.String);

                //跳过 :
                lexer.GetNextTokenOfType(TokenType.Colon);

                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();
                JsonValue value = ParseJsonValue(nextTokenType);

                obj[key] = value;

                //有逗号就跳过逗号
                if (lexer.LookNextTokenType() == TokenType.Comma)
                {
                    lexer.GetNextTokenOfType(TokenType.Comma);
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }
               
            }

            //跳过 {
            lexer.GetNextTokenOfType(TokenType.RightBrace);

            return obj;

        }

        /// <summary>
        /// 解析json值
        /// </summary>
        private static JsonValue ParseJsonValue(TokenType nextTokenType)
        {
            JsonValue value = new JsonValue();

            switch (nextTokenType)
            {

                case TokenType.Null:
                    lexer.GetNextTokenOfType(nextTokenType);
                    value.Type = ValueType.Null;
                    break;
                case TokenType.True:
                    lexer.GetNextTokenOfType(nextTokenType);
                    value.Type = ValueType.Boolean;
                    value.Boolean = true;
                    break;
                case TokenType.False:
                    lexer.GetNextTokenOfType(nextTokenType);
                    value.Type = ValueType.Boolean;
                    value.Boolean = false;
                    break;
                case TokenType.Number:
                    string token = lexer.GetNextTokenOfType(nextTokenType);
                    value.Type = ValueType.Number;
                    value.Number = double.Parse(token);
                    break;
                case TokenType.String:
                    token = lexer.GetNextTokenOfType(nextTokenType);
                    value.Type = ValueType.String;
                    value.Str = token;
                    break;
                case TokenType.LeftBracket:
                    value.Type = ValueType.Array;
                    value.Array = ParseJsonArray();
                    break;
                case TokenType.LeftBrace:
                    value.Type = ValueType.Object;
                    value.Obj = ParseJsonObject();
                    break;
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

            return value;
        }
   
        /// <summary>
        /// 解析Json数组
        /// </summary>
        /// <returns></returns>
        private static JsonValue[] ParseJsonArray()
        {

            //跳过[
            lexer.GetNextTokenOfType(TokenType.LeftBracket);

            while (lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = lexer.LookNextTokenType();
                JsonValue value = ParseJsonValue(nextTokenType);

                cachedList.Add(value);

                //有逗号就跳过
                if (lexer.LookNextTokenType() == TokenType.Comma)
                {
                    lexer.GetNextTokenOfType(TokenType.Comma);
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }
            }

            //跳过]
            lexer.GetNextTokenOfType(TokenType.RightBracket);

            JsonValue[] array = cachedList.ToArray();
            cachedList.Clear();

            return array;
        }
    }

}
