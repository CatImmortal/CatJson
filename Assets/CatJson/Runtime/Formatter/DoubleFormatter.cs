namespace CatJson
{
    public class DoubleFormatter : BaseJsonFormatter<double>
    {
        public override void ToJson(double value, int depth)
        {
            TextUtil.Append(value.ToString());
        }

        public override double ParseJson()
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.Number);
            return double.Parse(rs.ToString());
        }
    }
}