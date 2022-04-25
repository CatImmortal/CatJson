namespace CatJson
{
    public class StringFormatter : BaseJsonFormatter<string>
    {
        public override void ToJson(string value, int depth)
        {
            TextUtil.Append($"\"{value}\"", depth);
        }

        public override string ParseJson()
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.String);
            return rs.ToString();
        }
    }
}