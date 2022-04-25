namespace CatJson
{
    public class BooleanFormatter : BaseJsonFormatter<bool>
    {
        public override void ToJson(bool value, int depth)
        {
            string json = "true";
            if (!value)
            {
                json = "false";
            }
            
            TextUtil.Append(json);
        }

        public override bool ParseJson()
        {
            JsonParser.Lexer.GetNextToken(out TokenType nextTokenType);
            return nextTokenType == TokenType.True;
        }
    }
}