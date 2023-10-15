using System;

namespace CatJson
{
    /// <summary>
    /// decimal类型的Json格式化器
    /// </summary>
    public class DecimalFormatter : BaseJsonFormatter<decimal>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, decimal value, Type type, int depth)
        {
            parser.Append(value.ToString());
        }

        /// <inheritdoc />
        public override decimal ParseJson(JsonParser parser, Type type)
        {
            RangeString rs = parser.Lexer.GetNextTokenByType(TokenType.Number);
            return rs.AsDecimal();
        }
    }
}