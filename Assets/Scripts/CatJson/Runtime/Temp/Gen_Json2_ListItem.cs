using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class Generator
    {
        private static Json2_ListItem Gen_Json2_ListItem()
        {
            
            Json2_ListItem obj = new Json2_ListItem();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                Json2_ListItem temp = (Json2_ListItem)userdata1;
                switch (key.ToString())
                {
                    case "datetime":
                        temp.datetime = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "remark":
                        temp.remark = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
                        break;
                    case "zone":
                        temp.zone = JsonParser.Lexer.GetNextToken(out _).Value.ToString();
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
