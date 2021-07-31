using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenJsonCodes
    {
        static GenJsonCodes()
        {
			ParseJsonCodeFuncDict.Add(typeof(ToJson1_Data),ParseJson_ToJson1_Data);
			ToJsonCodeFuncDict.Add(typeof(ToJson1_Data), ToJson_ToJson1_Data);

        }
    }
}

