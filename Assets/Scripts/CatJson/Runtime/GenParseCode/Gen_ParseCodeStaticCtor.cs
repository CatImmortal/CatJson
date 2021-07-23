using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class ParseCode
    {
        static ParseCode()
        {
			ParseCodeFuncDict.Add(typeof(Json2_Root),Parse_Json2_Root);

        }
    }
}

