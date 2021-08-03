using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenJsonCodes
    {
        static GenJsonCodes()
        {
			ParseJsonCodeFuncDict.Add(typeof(UnityJson_Data),ParseJson_UnityJson_Data);
			ToJsonCodeFuncDict.Add(typeof(UnityJson_Data), ToJson_UnityJson_Data);

        }
    }
}

