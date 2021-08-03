using System.Collections;
using System.Collections.Generic;
using System;

namespace CatJson
{
    /// <summary>
    /// 标记需要生成解析/转换代码的此json数据类根节点
    /// 依赖的其他类不用标记也会自动生成代码
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GenJsonCodeRootAttribute : Attribute
    {

    }
}

