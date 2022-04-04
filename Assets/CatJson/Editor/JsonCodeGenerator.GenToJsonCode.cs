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

            if (!GenRootTypes.Contains(type) && type.IsValueType)
            {
                //不是root并且是值类型 obj的参数类型直接使用原本的类型名 防止装箱
                template = template.Replace("#Type#", type.FullName);
            }
            else
            {
                template = template.Replace("#Type#", "object");
            }

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
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < propertyInfos.Length; i++)
            {
                PropertyInfo pi = propertyInfos[i];
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

            if (propertyInfos.Length > 0)
            {
                int commaIndex = sb.ToString().LastIndexOf("TextUtil.AppendLine(\",\");", StringComparison.Ordinal);
                if (commaIndex!=-1)
                {
                    sb.Remove(commaIndex, 25);
                }
            }
           
            //处理字段
            FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo fi = fieldInfos[i];
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
            if (fieldInfos.Length > 0)
            {
                int commaIndex = sb.ToString().LastIndexOf("TextUtil.AppendLine(\",\");", StringComparison.Ordinal);
                if (commaIndex!=-1)
                {
                    sb.Remove(commaIndex, 25);
                }
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
            if (!type.IsValueType || type.IsPrimitive)
            {
                hasOpInequality = true;
            }
            else
            {
                //自定义值类型要检查所有静态方法
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
                if (type == typeof(float))
                {
                    AppendLine($"if (Math.Abs(data.{name} - default(float)) > 1E-6f)", 3);
                }
                else if (type == typeof(double))
                {
                    AppendLine($"if (Math.Abs(data.{name} - default(double)) > 1E-15)", 3);
                }
                else
                {
                    AppendLine($"if (data.{name} != default)", 3);
                }
                AppendLine("{", 3);
                AppendLine($"JsonParser.AppendJsonKey(\"{name}\", depth + 1);", 4);
                AppendToJsonValueCode(type, $"data.{name}",depth:4);
                AppendLine("TextUtil.AppendLine(\",\");", 4);
                AppendLine("}", 3);
            }
            else
            {
                AppendLine($"JsonParser.AppendJsonKey(\"{name}\", depth + 1);", 3);
                AppendToJsonValueCode(type, $"data.{name}",depth:3);
                AppendLine("TextUtil.AppendLine(\",\");", 3);
            }

            AppendLine(string.Empty);
        }

        /// <summary>
        /// 生成转换Json值的代码
        /// </summary>
        private static void AppendToJsonValueCode(Type valueType, string valueName, string itemName = "item", string depthCode = "depth+1",int depth = 0)
        {
            if (TypeUtil.IsBaseType(valueType))
            {
                //内置基础类型
                AppendLine($"JsonParser.AppendJsonValue({valueName});", depth);
            }
            else if (valueType.IsEnum)
            {
                //枚举
                AppendLine($"JsonParser.AppendJsonValue((int){valueName});", depth);
            }
            else if (TypeUtil.IsArrayOrListType(valueType))
            {
                //数组或List
                AppendToJsonArrayCode(valueType, TypeUtil.GetArrayOrListElementType(valueType), valueName, itemName, depthCode,depth);
            }
            else if (TypeUtil.IsDictionaryType(valueType))
            {
                //字典
                AppendToJsonDictCode(valueType.GetGenericArguments()[0],valueType.GetGenericArguments()[1], valueName, itemName, depthCode,depth);
            }
            else if (JsonCodeGenConfig.UseExtensionFuncTypes.Contains(valueType))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"JsonParser.AppendJsonValue(typeof({valueType.FullName}),{valueName},depth + 1);", depth);
            }
            else
            {
                //其他类型 使用生成的转换代码
                AppendLine($"{GetToJsonCodeMethodName(valueType)}({valueName},{depthCode});", depth);
            }
        }

        /// <summary>
        /// 生成转换Json数组的代码 
        /// depthCode表示该数组作为value时，对应key的depth表达式代码
        /// </summary>
        private static void AppendToJsonArrayCode(Type arrayType, Type elementType, string arrayName, string itemName, string depthCode,int depth)
        {
            AppendLine("TextUtil.AppendLine(\"[\");", depth);
            AppendLine("int index = 0;", depth);
            AppendLine($"foreach (var {itemName} in {arrayName})", depth);
            AppendLine("{", depth);
            AppendLine($"TextUtil.AppendTab({depthCode}+1);", depth+1);
            if (!elementType.IsValueType)
            {
                AppendLine($"if ({itemName} == null)",depth+1);
                AppendLine("{",depth+1);
                AppendLine("TextUtil.Append(\"null\");", depth+2);
                AppendLine("}",depth+1);
                AppendLine("else", depth+1);
                AppendLine("{", depth+1);
                AppendToJsonValueCode(elementType, itemName, itemName + "1", depthCode + "+1",depth+2);
                AppendLine("}", depth+1);
            }
            else
            {
                AppendToJsonValueCode(elementType, itemName, itemName + "1", depthCode + "+1",depth+1);
            }

            if (arrayType.IsGenericType && arrayType.GetGenericTypeDefinition() == typeof(List<>))
            {
                AppendLine($"if (index < {arrayName}.Count-1)", depth+1);
            }
            else
            {
                AppendLine($"if (index < {arrayName}.Length-1)", depth+1);
            }
            AppendLine("{", depth+1);
            AppendLine("TextUtil.AppendLine(\",\");", depth+2);
            AppendLine("}", depth+1);
            AppendLine("index++;", depth+1);
            AppendLine("}", depth);
            AppendLine("TextUtil.AppendLine(string.Empty);", depth);
            AppendLine($"TextUtil.Append(\"]\",{depthCode});", depth);
        }

        /// <summary>
        /// 生成转换Json字典的代码
        /// </summary>
        private static void AppendToJsonDictCode(Type keyType,Type valueType, string dictName, string itemName, string depthCode,int depth)
        {
            AppendLine("TextUtil.AppendLine(\"{\");", depth);
            AppendLine("int index = 0;", depth);
            AppendLine($"foreach (var {itemName} in {dictName})", depth);
            AppendLine("{", depth);
            if (keyType != typeof(int))
            {
                AppendLine($"JsonParser.AppendJsonKey({itemName}.Key, {depthCode}+1);", depth+1);
            }
            else
            {
                //处理key为int的情况
                AppendLine($"JsonParser.AppendJsonKey({itemName}.Key.ToString(), {depthCode}+1);", depth+1);
            }
           

            if (!valueType.IsValueType)
            {
                //对引用类型添加null检查
                AppendLine($"if ({itemName}.Value == null)", depth+1);
                AppendLine("{", depth+1);
                AppendLine("TextUtil.Append(\"null\");", depth+2);
                AppendLine("}", depth+1);
                AppendLine("else", depth+1);
                AppendLine("{", depth+1);
                AppendToJsonValueCode(valueType, itemName + ".Value", itemName + "1", depthCode + "+1",depth+2);
                AppendLine("}", depth+1);
            }
            else
            {
                AppendToJsonValueCode(valueType, itemName + ".Value", itemName + "1", depthCode + "+1",depth+1);
            }

            AppendLine($"if (index < {dictName}.Count-1)", depth+1);
            AppendLine("{", depth+1);
            AppendLine("TextUtil.AppendLine(\",\");", depth+2);
            AppendLine("}", depth+1);
            AppendLine("index++;", depth+1);
            AppendLine("}", depth);
            AppendLine("TextUtil.AppendLine(string.Empty);", depth);
            AppendLine($"TextUtil.Append(\"}}\",{depthCode});", depth);
        }
    }

}