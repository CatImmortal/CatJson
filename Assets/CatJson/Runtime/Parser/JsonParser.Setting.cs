using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
namespace CatJson
{
    public static partial class JsonParser
    {
        /// <summary>
        /// 序列化时是否开启格式化
        /// </summary>
        public static bool IsFormat { get; set; } = true;

        /// <summary>
        /// 序列化时是否忽略默认值
        /// </summary>
        public static bool IgnoreDefaultValue { get; set; } = true;
        
        /// <summary>
        /// 真实类型key
        /// </summary>
        private const string RealTypeKey = "<>RealType";
    }

}
