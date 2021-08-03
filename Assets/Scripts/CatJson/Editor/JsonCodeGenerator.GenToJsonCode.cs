using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CatJson.Editor
{
    public static partial class JsonCodeGenerator
    {
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
            JsonParser.IgnoreSet.TryGetValue(type, out HashSet<string> set);

            //处理属性
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (pi.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                {
                    //忽略
                    continue;
                }

                if (set != null && set.Contains(pi.Name))
                {
                    //忽略
                    continue;
                }

                //属性必须同时具有get set 并且不能是索引器item
                if (pi.SetMethod != null && pi.GetMethod != null && pi.Name != "Item")
                {

                    AppendToJsonCodeBySingle(pi.PropertyType, pi.Name);

                }

            }

            //处理字段
            foreach (FieldInfo fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.GetCustomAttribute<JsonIgnoreAttribute>() != null)
                {
                    //忽略
                    continue;
                }

                if (set != null && set.Contains(fi.Name))
                {
                    //忽略
                    continue;
                }

                AppendToJsonCodeBySingle(fi.FieldType, fi.Name);

            }

            string result = sb.ToString();
            sb.Clear();

            return result;
        }

        /// <summary>
        /// 为单个字段/属性生成Json转换代码
        /// </summary>
        private static void AppendToJsonCodeBySingle(Type type, string name)
        {
            //是否有 != 运算符可以调用
            bool hasOpInequality = false;
            if (!type.IsValueType)
            {
                hasOpInequality = true;
            }
            else
            {
                //值类型要检查所有静态方法
                MethodInfo[] mis = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
                foreach (MethodInfo mi in mis)
                {
                    if (mi.Name == "op_Inequality")
                    {
                        hasOpInequality = true;
                        break;
                    }
                }
            }

          

            if (hasOpInequality)
            {
                AppendLine($"if (data.{name} != default)", 3);
                AppendLine("{", 3);
                AppendLine("flag = true;", 3);
                AppendLine($"JsonParser.AppendJsonKey(\"{name}\", depth + 1);", 3);
                AppendToJsonValueCode(type, $"data.{name}");
                AppendLine("Util.AppendLine(\",\");", 3);
                AppendLine("}", 3);
            }
            else
            {
                AppendLine("flag = true;", 3);
                AppendLine($"JsonParser.AppendJsonKey(\"{name}\", depth + 1);", 3);
                AppendToJsonValueCode(type, $"data.{name}");
                AppendLine("Util.AppendLine(\",\");", 3);
            }

            AppendLine(string.Empty);
        }

        /// <summary>
        /// 生成转换Json值的代码
        /// </summary>
        private static void AppendToJsonValueCode(Type valueType, string valueName, string itemName = "item", string depthCode = "depth+1")
        {
            if (Util.IsBaseType(valueType))
            {
                //内置基础类型
                AppendLine($"JsonParser.AppendJsonValue({valueName});", 3);
            }
            else if (valueType.IsEnum)
            {
                //枚举
                AppendLine($"JsonParser.AppendJsonValue((int){valueName});", 3);
            }
            else if (Util.IsArrayOrList(valueType))
            {
                //数组或List
                AppendToJsonArrayCode(valueType, Util.GetArrayElementType(valueType), valueName, itemName, depthCode);
            }
            else if (Util.IsDictionary(valueType))
            {
                //字典
                AppendToJsonDictCode(valueType.GetGenericArguments()[1], valueName, itemName, depthCode);
            }
            else if (JsonCodeGenConfig.UseExtensionFuncTypes.Contains(valueType))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"JsonParser.AppendJsonValue(typeof({valueType.FullName}),{valueName})", 3);
            }
            else
            {
                //其他类型 使用生成的转换代码
                AppendLine($"{GetToJsonCodeMethodName(valueType)}({valueName},{depthCode});", 3);
            }
        }

        /// <summary>
        /// 生成转换Json数组的代码 
        /// depthCode表示该数组作为value时，对应key的depth表达式代码
        /// </summary>
        private static void AppendToJsonArrayCode(Type arrayType, Type elementType, string arrayName, string itemName, string depthCode)
        {
            AppendLine("Util.AppendLine(\"[\");", 3);

            AppendLine($"foreach (var {itemName} in {arrayName})", 3);
            AppendLine("{", 3);
            AppendLine($"Util.AppendTab({depthCode}+1);", 3);
            if (!elementType.IsValueType)
            {
                AppendLine($"if ({itemName} == null)",3);
                AppendLine("{",3);
                AppendLine("Util.Append(\"null\");", 3);
                AppendLine("}",3);
                AppendLine("else", 3);
                AppendLine("{", 3);
                AppendToJsonValueCode(elementType, itemName, itemName + "1", depthCode + "+1");
                AppendLine("}", 3);
            }
            else
            {
                AppendToJsonValueCode(elementType, itemName, itemName + "1", depthCode + "+1");
            }
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

            AppendLine($"Util.Append(\"]\",{depthCode});", 3);
        }

        /// <summary>
        /// 生成转换Json字典的代码
        /// </summary>
        private static void AppendToJsonDictCode(Type valueType, string dictName, string itemName, string depthCode)
        {
            AppendLine("Util.AppendLine(\"{\");", 3);

            AppendLine($"foreach (var {itemName} in {dictName})", 3);
            AppendLine("{", 3);
            AppendLine($"JsonParser.AppendJsonKey({itemName}.Key, {depthCode}+1);", 3);

            if (!valueType.IsValueType)
            {
                AppendLine($"if ({itemName}.Value == null)", 3);
                AppendLine("{", 3);
                AppendLine("Util.Append(\"null\");", 3);
                AppendLine("}", 3);
                AppendLine("else", 3);
                AppendLine("{", 3);
                AppendToJsonValueCode(valueType, itemName + ".Value", itemName + "1", depthCode + "+1");
                AppendLine("}", 3);
            }
            else
            {
                AppendToJsonValueCode(valueType, itemName + ".Value", itemName + "1", depthCode + "+1");
            }

            AppendLine("Util.AppendLine(\",\");", 3);
            AppendLine("}", 3);

            AppendLine($"if ({dictName}.Count > 0)", 3);
            AppendLine("{", 3);
            AppendLine(" Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);", 3);
            AppendLine("}", 3);

            AppendLine($"Util.Append(\"}}\",{depthCode});", 3);
        }
    }

}