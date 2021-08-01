using System;
using System.Collections.Generic;


namespace CatJson
{
    public static partial class GenJsonCodes
    {
        private static ToJson_Data ParseJson_ToJson_Data()
        {
            ToJson_Data obj = new ToJson_Data();

            JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>
            {
                ToJson_Data temp = (ToJson_Data)userdata1;
                TokenType tokenType;
                
				if (key.Equals(new RangeString("dataListList")))
				{
				List<List<ToJson_Data>> list = new List<List<ToJson_Data>>();
				JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata21, nextTokenType1) =>
				{
				List<ToJson_Data> list1 = new List<ToJson_Data>();
				JsonParser.ParseJsonArrayProcedure(list1, null, (userdata111, userdata211, nextTokenType11) =>
				{
				((List<ToJson_Data>)userdata111).Add(ParseJson_ToJson_Data());
				});
				list.Add(list1);
				});
				temp.dataListList = list;
				}
				else if (key.Equals(new RangeString("dataDictDict")))
				{
				Dictionary<string,Dictionary<string,ToJson_Data>> dict = new Dictionary<string,Dictionary<string,ToJson_Data>>();
				JsonParser.ParseJsonObjectProcedure(dict, null, (userdata11, userdata21,key1, nextTokenType1) =>
				{
				Dictionary<string,ToJson_Data> dict1 = new Dictionary<string,ToJson_Data>();
				JsonParser.ParseJsonObjectProcedure(dict1, null, (userdata111, userdata211,key11, nextTokenType11) =>
				{
				((Dictionary<string, ToJson_Data>)userdata111).Add(key11.ToString(),ParseJson_ToJson_Data());
				});
				((Dictionary<string, Dictionary<string,ToJson_Data>>)userdata11).Add(key1.ToString(),dict1);
				});
				temp.dataDictDict = dict;
				}

                else{
                    JsonParser.ParseJsonValue(nextTokenType);
                }
                
            });


            return obj;
        }
    }

}
