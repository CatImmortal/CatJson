using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_Result Gen_Json2_Result()
        {
            Json2_Result obj = new Json2_Result();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_Result temp = (Json2_Result)userdata1;
                switch (key.ToString())
                {
                    case "company":
                        temp.company = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "com":
                        temp.com = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "no":
                        temp.no = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "list":
                        List <Json2_ListItem> list = new List<Json2_ListItem>();

                        JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata22, nextTokenType2) =>
                        {
                            Json2_ListItem item = Gen_Json2_ListItem();
                            ((List<Json2_ListItem>)userdata11).Add(item);
                        });

                        temp.list = list;
                        break;
                    case "status":
                        string token = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        temp.status = int.Parse(token);
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
