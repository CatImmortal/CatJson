using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CatJson.Editor
{
    public static class ParseCodeGenConfig
    {
        public static string GenCodeDirPath = Application.dataPath + "/Scripts/CatJson/Runtime/GenParseCode";
        public static string ParseClassCodeTemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/ParseClassCodeTemplate.txt";
        public static string ParseStructCodeTemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/ParseStructCodeTemplate.txt";
        public static string StaticCtorTemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/StaticCtorTemplate.txt";

        /// <summary>
        /// 需要生成解析代码的json数据类的程序集名字
        /// </summary>
        public static string[] Assemblies =
        {
            "Assembly-CSharp"
        };
    }

}
