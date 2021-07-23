using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class ParseCode
    {
        static ParseCode()
        {
			ParseCodeFuncDict.Add(typeof(Json7_Data),Parse_Json7_Data);

        }
    }
}

