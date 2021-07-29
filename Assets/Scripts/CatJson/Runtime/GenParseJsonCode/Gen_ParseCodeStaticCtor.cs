using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class GenCodes
    {
        static GenCodes()
        {
			ParseJsonCodeFuncDict.Add(typeof(TestJson1_Root),Parse_TestJson1_Root);

        }
    }
}

