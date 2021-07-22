using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CatJson.Editor
{
    public static class ParseCodeConfig
    {
        public static string GenCodeDirPath = Application.dataPath + "/Scripts/CatJson/Runtime/GenParseCode";
        public static string TemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/CodeTemplate.txt";

        public static Type[] Types = {
            typeof(Json1_Root) ,
        };
    }

}
