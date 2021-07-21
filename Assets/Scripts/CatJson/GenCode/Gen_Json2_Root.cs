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
                switch (key.ToString())
                {
                    case "resultcode":
                        temp.resultcode = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "reason":
                        temp.reason = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "result":
                        temp.result = Gen_Json2_Result();
                        break;
                    case "error_code":
                        string token = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        temp.error_code = int.Parse(token);
                        break;
                    default:
                        JsonParser.ParseJsonValue(nextTokenType);
                        break;
                }
            });

            return obj;
        }
    }

}
