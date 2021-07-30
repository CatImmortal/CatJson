using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;
using System.Text;
namespace CatJson.Editor
{
    /// <summary>
    /// Json解析/转换代码生成器
    /// </summary>
    public static partial class JsonCodeGenerator
    {

        private static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 存放已生成过代码的Type
        /// </summary>
        private static HashSet<Type> GenCodeTypes = new HashSet<Type>();

        /// <summary>
        /// 存放待生成代码的被Root依赖的Type
        /// </summary>
        private static Queue<Type> needGenTypes = new Queue<Type>();



        [MenuItem("CatJson/预生成Json解析/转换代码")]
        private static void GenJsonCode()
        {

            ClearGenCodeDir();

            GenCodeTypes.Clear();
            needGenTypes.Clear();

            //生成Root的解析代码
            List<Type> types = GetGenCodeTypes();
            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];
                GenJsonParseCode(type);
                GenCodeTypes.Add(type);
            }

            //生成被Root依赖的Type的解析代码
            while (needGenTypes.Count > 0)
            {
                Type type = needGenTypes.Dequeue();
                GenJsonParseCode(type);
            }

            //生成静态构造器文件
            GenStaticCtorCode(types);

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 清理已生成代码的目录
        /// </summary>
        private static void ClearGenCodeDir()
        {
            if (!Directory.Exists(JsonCodeGenConfig.ParseJsonCodeDirPath))
            {
                Directory.CreateDirectory(JsonCodeGenConfig.ParseJsonCodeDirPath);
            }
            else
            {
                //清空旧文件
                DirectoryInfo di = new DirectoryInfo(JsonCodeGenConfig.ParseJsonCodeDirPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
            }

            if (!Directory.Exists(JsonCodeGenConfig.ToJsonCodeDirPath))
            {
                Directory.CreateDirectory(JsonCodeGenConfig.ToJsonCodeDirPath);
            }
            else
            {

                DirectoryInfo di = new DirectoryInfo(JsonCodeGenConfig.ToJsonCodeDirPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
            }
        }

        /// <summary>
        /// 生成静态构造器文件
        /// </summary>
        private static void GenStaticCtorCode(List<Type> types)
        {
            //读取模板文件
            StreamReader sr = new StreamReader(JsonCodeGenConfig.StaticCtorTemplateFilePath);
            string template = sr.ReadToEnd();
            sr.Close();

            foreach (Type type in types)
            {
                AppendLine($"ParseCodeFuncDict.Add(typeof({type.FullName}),{GetParseJsonCodeMethodName(type)});", 3);
            }

            template = template.Replace("#AddParseCodeFunc#", sb.ToString());
            sb.Clear();

            StreamWriter sw = new StreamWriter($"{JsonCodeGenConfig.ParseJsonCodeDirPath}/Gen_ParseCodeStaticCtor.cs");
            sw.Write(template);
            sw.Close();
        }


        /// <summary>
        /// 生成using代码
        /// </summary>
        private static string AppendUsingCode()
        {
            AppendLine("using System;",0);
            AppendLine("using System.Collections.Generic;",0);
            string result = sb.ToString();
            sb.Clear();
            return result;
        }


        /// <summary>
        /// 获取需要生成代码的json数据类
        /// </summary>
        private static List<Type> GetGenCodeTypes()
        {
            List<Type> types = new List<Type>();
            foreach (string item in JsonCodeGenConfig.Assemblies)
            {
                Assembly assembly = Assembly.Load(item);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<GenCodeRootAttribute>() != null)
                    {
                        types.Add(type);
                    }
                }
            }

            return types;
        }
      

        /// <summary>
        /// 获取类型对应的解析Json方法名
        /// </summary>
        private static string GetParseJsonCodeMethodName(Type type)
        {
            return $"ParseJson_{type.FullName.Replace(".", "_")}";
        }

        /// <summary>
        /// 获取类型全名，特殊处理带泛型的字典与List<T>
        /// </summary>
        private static string GetTypeFullName(Type type)
        {
            if (!type.IsGenericType)
            {
                return type.FullName;
            }

            if (Util.IsDictionary(type))
            {
                Type valueType = type.GetGenericArguments()[1];
                return $"Dictionary<string,{GetTypeFullName(valueType)}>";
            }

            Type elementType;
            if (type.IsArray)
            {
                elementType = type.GetElementType();
            }
            else
            {
                elementType = type.GetGenericArguments()[0];
            }

            return $"List<{GetTypeFullName(elementType)}>";
        }

        /// <summary>
        /// 带制表符的AppendLine
        /// </summary>
        private static void AppendLine(string str, int tabNum = 4)
        {
            for (int i = 0; i < tabNum; i++)
            {
                sb.Append("\t");
            }
            sb.AppendLine(str);
        }
    
    
       
    }
}

