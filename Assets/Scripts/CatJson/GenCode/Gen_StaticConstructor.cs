using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class Gen
    {
        static Gen()
        {
            GenCodeDict.Add(typeof(Json2_Root), Gen_Json2_Root);
        }
    }
}

