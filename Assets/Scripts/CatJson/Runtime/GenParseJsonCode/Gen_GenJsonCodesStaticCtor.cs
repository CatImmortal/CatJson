using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenJsonCodes
    {
        static GenJsonCodes()
        {
			ParseJsonCodeFuncDict.Add(typeof(TestJson1_Root),ParseJson_TestJson1_Root);
			ToJsonCodeFuncDict.Add(typeof(TestJson1_Root), ToJson_TestJson1_Root);

        }
    }
}

