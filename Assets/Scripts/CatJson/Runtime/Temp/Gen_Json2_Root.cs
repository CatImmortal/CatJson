using System;
using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_Root Gen_Json2_Root()
        {
            Json2_Root obj = new Json2_Root();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_Root temp = (Json2_Root)userdata1;
                RangeString? rs;
                TokenType tokenType;
                switch (key.ToString())
                {
                    case "resultcode":
                        rs = JsonParser.Lexer.GetNextToken(out tokenType);
                        if (tokenType == TokenType.String)
                        {
                            temp.resultcode = rs.Value.ToString();
                        }
                        else if(tokenType != TokenType.Null)
                        {
                            throw new Exception("resultcode的value类型不正确，当前解析到的是:" + tokenType);
                        }
                        break;
                    case "reason":
                        temp.reason = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "result":
                        temp.result = Gen_Json2_Result();
                        break;
                    case "error_code":
                        rs = JsonParser.Lexer.GetNextToken(out _);
                        if (rs.HasValue)
                        {
                            temp.error_code = int.Parse(rs.Value.ToString());
                        }
                        break;
                    //case "boolean":
                    //    rs = JsonParser.Lexer.GetNextToken(out tokenType);
                    //    if (tokenType == TokenType.True || tokenType == TokenType.False)
                    //    {
                    //        temp.boolean = tokenType == TokenType.True;
                    //    }
                    //    else if (tokenType != TokenType.Null)
                    //    {
                    //        throw new Exception("resultcode的value类型不正确，当前解析到的是:" + tokenType);
                    //    }
                    //    break;
                        
                    default:
                        JsonParser.ParseJsonValue(nextTokenType);
                        break;
                }
            });

            return obj;
        }
    }

}
