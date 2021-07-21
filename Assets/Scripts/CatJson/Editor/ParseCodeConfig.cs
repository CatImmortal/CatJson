using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace CatJson.Editor
{
    public static class ParseCodeConfig
    {
        public static string GenCodeDirPath = Application.dataPath + "/Scripts/CatJson/Runtime/GenCode";
        public static string TemplateFilePath = Application.dataPath + "/Scripts/CatJson/Editor/GenCodeTemplate.txt";

        public static Type[] Types = {
            typeof(Json1_Root) ,
        };
    }

}
