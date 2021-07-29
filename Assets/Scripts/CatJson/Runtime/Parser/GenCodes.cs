using System.Collections;
using System.Collections.Generic;
using System;


namespace CatJson
{
    public static partial class GenCodes
    {
        /// <summary>
        /// 类型与对应的预生成解析Json代码
        /// </summary>
        public static Dictionary<Type, Func<object>> ParseJsonCodeFuncDict = new Dictionary<Type, Func<object>>();
    }
}

