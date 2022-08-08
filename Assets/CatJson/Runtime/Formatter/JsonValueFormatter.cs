using System;

namespace CatJson
{
    /// <summary>
    /// JsonValue类型的Json格式化器
    /// </summary>
    public class JsonValueFormatter : BaseJsonFormatter<JsonValue>
    {
        /// <inheritdoc />
        public override void ToJson(JsonValue value, Type type, Type realType, int depth)
        {
            switch (value.Type)
            {
                case ValueType.Null:
                    TextUtil.Append("null");
                    break;
                case ValueType.Boolean:
                    JsonParser.ToJson(value.Boolean,0);
                    break;
                case ValueType.Number:
                    JsonParser.ToJson(value.Number,0);
                    break;
                case ValueType.String:
                    JsonParser.ToJson(value.Str,0);
                    break;
                case ValueType.Array:
                    JsonParser.ToJson(value.Array, depth + 1);
                    break;
                case ValueType.Object:
                    JsonParser.ToJson(value.Obj, depth + 1);
                    break;
            }
        }

        /// <inheritdoc />
        public override JsonValue ParseJson(Type type, Type realType)
        {
            //这里只能look不能get，get交给各类型的formatter去进行
            TokenType nextTokenType = JsonParser.Lexer.LookNextTokenType();
            
            switch (nextTokenType)
            {
                case TokenType.True:
                case TokenType.False:
                    return new JsonValue(JsonParser.ParseJson<bool>());
                
                case TokenType.Number:
                    return new JsonValue(JsonParser.ParseJson<double>());
                
                case TokenType.String:
                    return new JsonValue(JsonParser.ParseJson<string>());
                
                case TokenType.LeftBracket:
                    return new JsonValue(JsonParser.ParseJson<JsonValue[]>());
                
                case TokenType.LeftBrace:
                    return new JsonValue(JsonParser.ParseJson<JsonObject>());
                
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

        }
    }
}