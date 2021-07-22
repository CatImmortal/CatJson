using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class Generator
    {
        static Generator()
        {
			GenCodeDict.Add(typeof(Json1_Root),Parse_Json1_Root);
			GenCodeDict.Add(typeof(Json2_Root),Parse_Json2_Root);

        }
    }
}

