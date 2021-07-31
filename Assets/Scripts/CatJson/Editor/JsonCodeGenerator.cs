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



        [MenuItem("CatJson/预生成Json解析-转换代码")]
        private static void GenJsonCode()
        {

            ClearGenCodeDir();

            GenCodeTypes.Clear();
            needGenTypes.Clear();

            //生成Root的解析/转换代码
            List<Type> types = GetGenCodeTypes();
            for (int i = 0; i < types.Count; i++)
            {
                Type type = types[i];

                GenParseJsonCodeFile(type);
                GenToJsonCodeFile(type);
                GenCodeTypes.Add(type);
            }

            //生成被Root依赖的Type的解析/转换代码
            while (needGenTypes.Count > 0)
            {
                Type type = needGenTypes.Dequeue();
                if (!GenCodeTypes.Contains(type))
                {
                    GenParseJsonCodeFile(type);
                    GenCodeTypes.Add(type);
                }
            }

            //生成静态构造器文件
            GenStaticCtorCode(types);

            AssetDatabase.Refresh();
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
                AppendLine($"ParseJsonCodeFuncDict.Add(typeof({type.FullName}),{GetParseJsonCodeMethodName(type)});", 3);
                AppendLine($"ToJsonCodeFuncDict.Add(typeof({type.FullName}), {GetToJsonCodeMethodName(type)});", 3);
            }

            template = template.Replace("#AddJsonCodeFunc#", sb.ToString());
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
                    if (type.GetCustomAttribute<GenJsonCodeRootAttribute>() != null)
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
        /// 获取类型对应的转换Json方法名
        /// </summary>
        private static string GetToJsonCodeMethodName(Type type)
        {
            return $"ToJson_{type.FullName.Replace(".", "_")}";
        }

        /// <summary>
        /// 生成Json转换代码文件
        /// </summary>
        private static void GenToJsonCodeFile(Type type)
        {
            //读取模板文件
            StreamReader sr = new StreamReader(JsonCodeGenConfig.ToJsonCodeTemplateFilePaht);

            string template = sr.ReadToEnd();
            sr.Close();

            //写入using
            template = template.Replace("#Using#", AppendUsingCode());

            //写入类名
            template = template.Replace("#ClassName#", type.FullName);

            //写入转换方法名
            template = template.Replace("#MethodName#", GetToJsonCodeMethodName(type));

            //生成转换代码
            template = template.Replace("#ToJsonCode#", AppendToJsonCode(type));

            StreamWriter sw = new StreamWriter($"{JsonCodeGenConfig.ToJsonCodeDirPath}/Gen_{type.FullName.Replace(".", "_")}_ToJsonCode.cs");
            sw.Write(template);
            sw.Close();
        }
    
        /// <summary>
        /// 生成转换字段/属性为json文本的代码
        /// </summary>
        private static string AppendToJsonCode(Type type)
        {
            //处理属性
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //属性必须同时具有get set 并且不能是索引器item
                if (pi.SetMethod != null && pi.GetMethod != null && pi.Name != "Item")
                {
  
                    AppendToJsonCodeBySingle(pi.PropertyType, pi.Name);

                }

            }

            //处理字段
            foreach (FieldInfo fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {

                AppendToJsonCodeBySingle(fi.FieldType, fi.Name);

            }

            string result = sb.ToString();
            sb.Clear();

            return result;
        }

        /// <summary>
        /// 为单个字段/属性生成Json转换代码
        /// </summary>
        private static void AppendToJsonCodeBySingle(Type type,string name)
        {
            AppendLine($"if (data.{name} != default)",3);
            AppendLine("{", 3);
            AppendLine("flag = true;", 3);
            AppendLine($"JsonParser.AppendJsonKey(\"{name}\", depth);", 3);
            AppendToJsonValueCode(type, $"data.{name}");
            AppendLine("Util.AppendLine(\",\");", 3);
            AppendLine("}", 3);
        }
  
        /// <summary>
        /// 生成转换Json值的代码
        /// </summary>
        private static void AppendToJsonValueCode(Type valueType,string valueName,string itemName = "item",string depthCode = "depth+1")
        {
            if (Util.IsBaseType(valueType))
            {
                //基础类型 string bool 数字
                AppendLine($"JsonParser.AppendJsonValue({valueName});", 3);
            }
            else if (valueType.IsEnum)
            {
                //枚举 todo:
            }
            else if (Util.IsArrayOrList(valueType))
            {
                //数组或List
                AppendToJsonArrayCode(valueType, Util.GetArrayElementType(valueType), valueName, itemName,depthCode);
            }
            else if (Util.IsDictionary(valueType))
            {
                //字典
                AppendToJsonDictCode(valueType.GetGenericArguments()[1], valueName, itemName,depthCode);
            }
            else if (JsonCodeGenConfig.UseExtensionFuncTypes.Contains(valueType))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"JsonParser.AppendJsonValue(typeof({valueType.FullName}),{valueName})", 3);
            }
            else
            {
                //其他类型 使用生成的转换代码
                AppendLine($"{GetToJsonCodeMethodName(valueType)}({valueName},depth + 1);", 3);
            }
        }

        /// <summary>
        /// 生成转换Json数组的代码
        /// </summary>
        private static void AppendToJsonArrayCode(Type arrayType,Type elementType,string arrayName,string itemName,string depthCode)
        {
            AppendLine("Util.AppendLine(\"[\");", 3);

            AppendLine($"foreach (var {itemName} in {arrayName})", 3);
            AppendLine("{", 3);
            AppendLine($"Util.AppendTab({depthCode});", 3);
            AppendToJsonValueCode(elementType,itemName, itemName + "1",depthCode + "+1");
            AppendLine("Util.AppendLine(\",\");", 3);
            AppendLine("}", 3);

            if (arrayType.IsArray)
            {
                AppendLine($"if ({arrayName}.Length > 0)", 3);
            }
            else
            {
                AppendLine($"if ({arrayName}.Count > 0)", 3);
            }
            AppendLine("{", 3);
            AppendLine(" Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);", 3);
            AppendLine("}", 3);

            AppendLine("Util.Append(\"]\",depth);", 3);
        }
    
        /// <summary>
        /// 生成转换Json字典的代码
        /// </summary>
        private static void AppendToJsonDictCode(Type valueType,string dictName,string itemName,string depthCode)
        {
            AppendLine("Util.AppendLine(\"{\");", 3);

            AppendLine($"foreach (var {itemName} in {dictName})", 3);
            AppendLine("{", 3);
            AppendLine($"JsonParser.AppendJsonKey({itemName}.Key, {depthCode});", 3);
            AppendToJsonValueCode(valueType,itemName + ".Value", itemName + "1", depthCode + "+1");
            AppendLine("Util.AppendLine(\",\");", 3);
            AppendLine("}", 3);

            AppendLine($"if ({dictName}.Count > 0)", 3);
            AppendLine("{", 3);
            AppendLine(" Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);", 3);
            AppendLine("}", 3);

            AppendLine("Util.Append(\"}\",depth);", 3);
        }
    }
}

