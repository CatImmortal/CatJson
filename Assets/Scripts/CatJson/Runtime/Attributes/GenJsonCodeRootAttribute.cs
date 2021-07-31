using System.Collections;
using System.Collections.Generic;
using System;

namespace CatJson
{
    /// <summary>
    /// 标记所有需要生成解析/转换代码的json数据类根节点
    /// 依赖的其他类不用标记也会自动生成
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenJsonCodeRootAttribute : Attribute
    {

    }
}

