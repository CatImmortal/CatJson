using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenJsonCodes
    {
        static GenJsonCodes()
        {
			ParseJsonCodeFuncDict.Add(typeof(ToJson_Data),ParseJson_ToJson_Data);
			ToJsonCodeFuncDict.Add(typeof(ToJson_Data), ToJson_ToJson_Data);

        }
    }
}

