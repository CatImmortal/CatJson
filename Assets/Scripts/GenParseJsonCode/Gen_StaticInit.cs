using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenJsonCodesHelper
    {
        public static void Init()
        {
			GenJsonCodes.ParseJsonCodeFuncDict.Add(typeof(TestJson1_Root),ParseJson_TestJson1_Root);
			GenJsonCodes.ToJsonCodeFuncDict.Add(typeof(TestJson1_Root), ToJson_TestJson1_Root);

        }
    }
}

