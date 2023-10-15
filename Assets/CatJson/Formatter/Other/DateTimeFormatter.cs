using System;

namespace CatJson
{
    /// <summary>
    /// DateTime类型的Json格式化器
    /// </summary>
    public class DateTimeFormatter : BaseJsonFormatter<DateTime>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, DateTime value, Type type, int depth)
        {
            parser.Append('\"');
            parser.Append(value.ToString());
            parser.Append('\"');
        }

        /// <inheritdoc />
        public override DateTime ParseJson(JsonParser parser, Type type)
        {
            RangeString rs = parser.Lexer.GetNextTokenByType(TokenType.String);
            return rs.AsDateTime();
        }
    }
}