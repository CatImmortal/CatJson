using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class Generator
    {
        private static Json1_Root Gen_Json1_Root()
        {
            Json1_Root obj = new Json1_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json1_Root temp = (Json1_Root)userdata1;
                RangeString? rs;
                TokenType tokenType;
                switch (key.ToString())
                {
                    case "array":
                        List<System.Int32> list = new List<System.Int32>();
                        JsonParser.ParseJsonArrayProcedure(temp.array, null, (userdata11, userdata22, nextTokenType2) =>
            {
                rs = JsonParser.Lexer.GetNextToken(out tokenType);
                if (tokenType == TokenType.Number)
                {
                    ((List<System.Int32>)userdata11).Add(System.Int32.Parse(rs.Value.ToString()));
                }
                else if (tokenType != TokenType.Null)
                {
                    throw new System.Exception("array的value类型不正确，当前解析到的是: " + tokenType);
                }
            });
                        temp.array = list.ToArray();
                        break;
                    case "type":
                        rs = JsonParser.Lexer.GetNextToken(out tokenType);
                        if (tokenType == TokenType.True || tokenType == TokenType.False)
                        {
                            temp.type = tokenType == TokenType.True;
                        }
                        else if (tokenType != TokenType.Null)
                        {
                            throw new System.Exception("type的value类型不正确，当前解析到的是: " + tokenType);
                        }
                        break;
                    case "n":
                        rs = JsonParser.Lexer.GetNextToken(out tokenType);
                        if (tokenType == TokenType.String)
                        {
                            temp.n = rs.Value.ToString();
                        }
                        else if (tokenType != TokenType.Null)
                        {
                            throw new System.Exception("n的value类型不正确，当前解析到的是: " + tokenType);
                        }
                        break;
                    case "number":
                        rs = JsonParser.Lexer.GetNextToken(out tokenType);
                        if (tokenType == TokenType.Number)
                        {
                            temp.number = System.Int32.Parse(rs.Value.ToString());
                        }
                        else if (tokenType != TokenType.Null)
                        {
                            throw new System.Exception("number的value类型不正确，当前解析到的是: " + tokenType);
                        }
                        break;
                    case "obj":
                        break;
                    case "str":
                        rs = JsonParser.Lexer.GetNextToken(out tokenType);
                        if (tokenType == TokenType.String)
                        {
                            temp.str = rs.Value.ToString();
                        }
                        else if (tokenType != TokenType.Null)
                        {
                            throw new System.Exception("str的value类型不正确，当前解析到的是: " + tokenType);
                        }
                        break;
                }
            });


            return obj;
        }
    }

}

