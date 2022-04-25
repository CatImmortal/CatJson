using System;

namespace CatJson
{
    public class JsonValueFormatter : BaseJsonFormatter<JsonValue>
    {
        public override void ToJson(JsonValue value, int depth)
        {
            switch (value.Type)
            {
                case ValueType.Null:
                    TextUtil.Append("null");
                    break;
                case ValueType.Boolean:
                    JsonParser.InternalToJson(value.Boolean);
                    break;
                case ValueType.Number:
                    JsonParser.InternalToJson(value.Number);
                    break;
                case ValueType.String:
                    JsonParser.InternalToJson(value.Str);
                    break;
                case ValueType.Array:
                    JsonParser.InternalToJson(value.Array, depth + 1);
                    break;
                case ValueType.Object:
                    JsonParser.InternalToJson(value.Obj, depth + 1);
                    break;
            }
        }

        public override JsonValue ParseJson()
        {
            JsonValue value = new JsonValue();
            
            //这里只能look不能get，get交给各类型的formatter去进行
            TokenType nextTokenType = JsonParser.Lexer.LookNextTokenType();
            
            switch (nextTokenType)
            {
                case TokenType.Null:
                    JsonParser.Lexer.GetNextToken(out _);
                    value.Type = ValueType.Null;
                    break;
                case TokenType.True:
                case TokenType.False:
                    value.Type = ValueType.Boolean;
                    value.Boolean = JsonParser.InternalParseJson<bool>();
                    break;
                case TokenType.Number:
                    value.Type = ValueType.Number;
                    value.Number = JsonParser.InternalParseJson<double>();
                    break;
                case TokenType.String:
                    value.Type = ValueType.String;
                    value.Str = JsonParser.InternalParseJson<string>();
                    break;
                case TokenType.LeftBracket:
                    value.Type = ValueType.Array;
                    value.Array = JsonParser.InternalParseJson<JsonValue[]>();
                    break;
                case TokenType.LeftBrace:
                    value.Type = ValueType.Object;
                    value.Obj  = JsonParser.InternalParseJson<JsonObject>();
                    break;
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

            return value;
        }
    }
}