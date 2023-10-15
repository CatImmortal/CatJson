using System;

namespace CatJson
{
    /// <summary>
    /// uint类型的Json格式化器
    /// </summary>
    public class UInt32Formatter : BaseJsonFormatter<uint>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, uint value, Type type, int depth)
        {
            parser.Append(value.ToString());
        }

        /// <inheritdoc />
        public override uint ParseJson(JsonParser parser, Type type)
        {
            RangeString rs = parser.Lexer.GetNextTokenByType(TokenType.Number);
            return rs.AsUInt();
        }
    }
}