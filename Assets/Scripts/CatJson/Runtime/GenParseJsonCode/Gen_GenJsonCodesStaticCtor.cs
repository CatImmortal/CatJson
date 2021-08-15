using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenJsonCodes
    {
        static GenJsonCodes()
        {
			ParseJsonCodeFuncDict.Add(typeof(IntKeyJson_Data),ParseJson_IntKeyJson_Data);
			ToJsonCodeFuncDict.Add(typeof(IntKeyJson_Data), ToJson_IntKeyJson_Data);

        }
    }
}

