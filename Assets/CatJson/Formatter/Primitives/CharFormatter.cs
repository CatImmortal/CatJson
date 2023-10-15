using System;

namespace CatJson
{
    /// <summary>
    /// char类型的Json格式化器
    /// </summary>
    public class CharFormatter : BaseJsonFormatter<char>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, char value, Type type, int depth)
        {
            parser.Append('\"');
            parser.Append(value);
            parser.Append('\"');
        }

        /// <inheritdoc />
        public override char ParseJson(JsonParser parser, Type type)
        {
            RangeString rs = parser.Lexer.GetNextTokenByType(TokenType.String);
            return rs[0];
        }
    }
}