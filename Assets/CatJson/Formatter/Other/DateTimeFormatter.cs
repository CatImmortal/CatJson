using System;

namespace CatJson
{
    /// <summary>
    /// DateTime类型的Json格式化器
    /// </summary>
    public class DateTimeFormatter : BaseJsonFormatter<DateTime>
    {
        /// <inheritdoc />
        public override void ToJson(DateTime value, Type type, Type realType, int depth)
        {
            TextUtil.Append(value.ToString());
        }

        /// <inheritdoc />
        public override DateTime ParseJson(Type type, Type realType)
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.Number);
            return DateTime.Parse(rs.AsSpan());
        }
    }
}