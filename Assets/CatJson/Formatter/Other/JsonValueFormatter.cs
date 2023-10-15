using System;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// JsonValue类型的Json格式化器
    /// </summary>
    public class JsonValueFormatter : BaseJsonFormatter<JsonValue>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, JsonValue value, Type type, int depth)
        {
            switch (value.Type)
            {
                case ValueType.Null:
                    parser.Append("null");
                    break;
                case ValueType.Boolean:
                    parser.InternalToJson((bool)value,typeof(bool),0);
                    break;
                case ValueType.Number:
                    parser.InternalToJson((double)value,typeof(double),0);
                    break;
                case ValueType.String:
                    parser.InternalToJson((string)value,typeof(string),0);
                    break;
                case ValueType.Array:
                    parser.InternalToJson((List<JsonValue>)value,typeof(List<JsonValue>), depth);
                    break;
                case ValueType.Object:
                    parser.InternalToJson((JsonObject)value,typeof(JsonObject), depth);
                    break;
            }
        }

        /// <inheritdoc />
        public override JsonValue ParseJson(JsonParser parser, Type type)
        {
            //这里只能look不能get，get交给各类型的formatter去进行
            TokenType nextTokenType = parser.Lexer.LookNextTokenType();
            
            switch (nextTokenType)
            {
                case TokenType.Null:
                    parser.Lexer.GetNextTokenByType(TokenType.Null);
                    return new JsonValue();
                
                case TokenType.True:
                case TokenType.False:
                    return new JsonValue(parser.ParseJson<bool>());
                
                case TokenType.Number:
                    return new JsonValue(parser.ParseJson<double>());
                
                case TokenType.String:
                    return new JsonValue(parser.ParseJson<string>());
                
                case TokenType.LeftBracket:
                    return new JsonValue(parser.ParseJson<List<JsonValue>>());
                
                case TokenType.LeftBrace:
                    return new JsonValue(parser.ParseJson<JsonObject>());
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

        }
    }
}