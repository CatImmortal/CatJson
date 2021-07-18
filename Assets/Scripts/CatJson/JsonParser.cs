using System.Collections;
using System.Collections.Generic;
using System;

namespace CatJson
{
    /// <summary>
    /// Json解析器
    /// </summary>
    public static class JsonParser
    {


        /// <summary>
        /// 从json文本反序列化JsonValue
        /// </summary>
        public static JsonData FromJson(string json)
        {
            JsonLexer lexer = new JsonLexer(json);
            JsonData data = new JsonData();

            lexer.GetNextToken(out TokenType type, out string token);

            switch (type)
            {
                case TokenType.Eof:
                    break;
                case TokenType.Null:
                    data.DataType = JsonDataType.Null;
                    break;
                case TokenType.True:
                    data.DataType = JsonDataType.True;
                    break;
                case TokenType.False:
                    data.DataType = JsonDataType.False;
                    break;
                case TokenType.Number:
                    data.DataType = JsonDataType.Number;
                    data.Number = double.Parse(token);
                    UnityEngine.Debug.Log(data.Number);
                    break;
            }

            lexer.GetNextToken(out type, out token);
            if (type != TokenType.Eof)
            {
                throw new Exception("Json解析错误");
            }

            return data;
        }


    
      
    }
}

