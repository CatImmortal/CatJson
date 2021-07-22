using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CatJson.Editor
{
    public static class ParseCodeGenConfig
    {
        public static string GenCodeDirPath = Application.dataPath + "/Scripts/CatJson/Runtime/GenParseCode";
        public static string ParseCodeTemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/ParseCodeTemplate.txt";
        public static string StaticCtorTemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/StaticCtorTemplate.txt";

        public static Type[] Types = {
            typeof(Json1_Root) ,
        };
    }

}
