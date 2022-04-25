
using UnityEngine;

namespace CatJson
{
    public class Int32Formatter : BaseJsonFormatter<int>
    {
        public override void ToJson(int value, int depth)
        {
            TextUtil.Append(value.ToString());
        }

        public override int ParseJson()
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.Number);
            return int.Parse(rs.ToString());
        }
    }
}
