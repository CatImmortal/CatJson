using System;

namespace CatJson
{
    /// <summary>
    /// sbyte类型的Json格式化器
    /// </summary>
    public class SByteFormatter : BaseJsonFormatter<sbyte>
    {
        /// <inheritdoc />
        public override void ToJson(JsonParser parser, sbyte value, Type type, int depth)
        {
            parser.Append(value.ToString());
        }

        /// <inheritdoc />
        public override sbyte ParseJson(JsonParser parser, Type type)
        {
            RangeString rs = parser.Lexer.GetNextTokenByType(TokenType.Number);
            return rs.AsSByte();
        }
    }
}