using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class ParseCode
    {
        static ParseCode()
        {
			ParseCodeFuncDict.Add(typeof(TestJson1_Root),Parse_TestJson1_Root);

        }
    }
}

