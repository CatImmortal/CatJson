using System;

namespace CatJson
{
    /// <summary>
    /// int类型的Json格式化器
    /// </summary>
    public class Int32Formatter : BaseJsonFormatter<int>
    {
        /// <inheritdoc />
        public override void ToJson(int value,Type type,int depth)
        {
            TextUtil.Append(value.ToString());
        }

        /// <inheritdoc />
        public override int ParseJson(Type type)
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.Number);
            return int.Parse(rs.ToString());
        }
    }
}
