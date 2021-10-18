using System.Collections.Generic;
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
        /// 需要生成代码并注册到JsonParser的Type
        /// </summary>
        private static HashSet<Type> GenRootTypes = new HashSet<Type>();

        /// <summary>
        /// 存放已生成过代码的Type
        /// </summary>
        private static HashSet<Type> GenCodeTypes = new HashSet<Type>();

        /// <summary>
        /// 存放待生成代码的被Root依赖的Type
        /// </summary>
        private static Queue<Type> needGenTypes = new Queue<Type>();

        [MenuItem("CatJson/清理已生成的Json解析-转换代码")]
        private static void ClearJsonCode()
        {
            ClearGenCodeDir();
            Directory.Delete(JsonCodeGenConfig.ParseJsonCodeDirPath);
            Directory.Delete(JsonCodeGenConfig.ToJsonCodeDirPath);


            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("提示", "已清理", "确认");
        }

        [MenuItem("CatJson/预生成Json解析-转换代码")]
        private static void GenJsonCode()
        {

            ClearGenCodeDir();

            GenCodeTypes.Clear();
            needGenTypes.Clear();

            GetGenRootTypes();

            //生成root的解析/转换代码
            foreach (Type root in GenRootTypes)
            {
                GenParseJsonCodeFile(root);
                GenToJsonCodeFile(root);
                GenCodeTypes.Add(root);
            }
            //生成被依赖的Type的解析/转换代码
            while (needGenTypes.Count > 0)
            {
                Type type = needGenTypes.Dequeue();
                if (!GenCodeTypes.Contains(type))
                {
                    GenParseJsonCodeFile(type);
                    GenToJsonCodeFile(type);
                    GenCodeTypes.Add(type);
                }
            }

            //生成静态构造器文件
            GenStaticInitCodeFile();

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("提示", "已生成完毕", "确认");
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

        /// <summary>
        /// 清理已生成代码的目录
        /// </summary>
        private static void ClearGenCodeDir()
        {
            //如果文件夹不存在 就创建
            //否则清理该文件夹

            if (!Directory.Exists(JsonCodeGenConfig.ParseJsonCodeDirPath))
            {
                Directory.CreateDirectory(JsonCodeGenConfig.ParseJsonCodeDirPath);
            }
            else
            {
                
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
        /// 生成静态初始化文件
        /// </summary>
        private static void GenStaticInitCodeFile()
        {
            //读取模板文件
            StreamReader sr = new StreamReader(JsonCodeGenConfig.StaticInitTemplateFilePath);
            string template = sr.ReadToEnd();
            sr.Close();

            foreach (Type type in GenRootTypes)
            {
                if (type.IsValueType && !type.IsPrimitive)
                {
                    AppendLine($"GenJsonCodes.ParseJsonCodeFuncDict.Add(typeof({type.FullName}),()=>{GetParseJsonCodeMethodName(type)}());", 3);
                }
                else
                {
                    AppendLine($"GenJsonCodes.ParseJsonCodeFuncDict.Add(typeof({type.FullName}),{GetParseJsonCodeMethodName(type)});", 3);
                }
                AppendLine($"GenJsonCodes.ToJsonCodeFuncDict.Add(typeof({type.FullName}), {GetToJsonCodeMethodName(type)});", 3);
            }

            template = template.Replace("#AddJsonCodeFunc#", sb.ToString());
            sb.Clear();

            StreamWriter sw = new StreamWriter($"{JsonCodeGenConfig.ParseJsonCodeDirPath}/Gen_StaticInit.cs");
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
        private static void GetGenRootTypes()
        {
            foreach (string item in JsonCodeGenConfig.Assemblies)
            {
                Assembly assembly = Assembly.Load(item);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute<GenJsonCodeRootAttribute>() != null)
                    {
                        GenRootTypes.Add(type);
                    }
                }
            }
        }
      

        /// <summary>
        /// 获取类型对应的解析Json方法名
        /// </summary>
        private static string GetParseJsonCodeMethodName(Type type)
        {
            return $"ParseJson_{type.FullName.Replace(".", "_")}";
        }

        /// <summary>
        /// 获取类型对应的转换Json方法名
        /// </summary>
        private static string GetToJsonCodeMethodName(Type type)
        {
            return $"ToJson_{type.FullName.Replace(".", "_")}";
        }

      
    }
}

