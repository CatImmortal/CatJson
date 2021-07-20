using System.Collections;
using System.Collections.Generic;
using System;


namespace CatJson
{
    public static partial class Gen
    {
        public static Dictionary<Type, Func<JsonLexer,object>> GenCodeDict = new Dictionary<Type, Func<JsonLexer, object>>();


    }
}

