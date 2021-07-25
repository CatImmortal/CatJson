using System.Collections;
using System.Collections.Generic;
using System;


namespace CatJson
{
    public static partial class ParseCode
    {
        /// <summary>
        /// 类型与对应的预生成解析代码
        /// </summary>
        public static Dictionary<Type, Func<object>> ParseCodeFuncDict = new Dictionary<Type, Func<object>>();
    }
}

