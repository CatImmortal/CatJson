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
    /// 解析代码生成器
    /// </summary>
    public static class ParseCodeGenerator
    {

        private static Queue<Type> genCodeTypes = new Queue<Type>();

        private static StringBuilder sb = new StringBuilder();

        [MenuItem("CatJson/预生成Json解析代码")]
        private static void GenParseCode()
        {
            
            if (!Directory.Exists(ParseCodeGenConfig.GenCodeDirPath))
            {
                Directory.CreateDirectory(ParseCodeGenConfig.GenCodeDirPath);
            }
            else
            {
                //清空旧文件
                DirectoryInfo di = new DirectoryInfo(ParseCodeGenConfig.GenCodeDirPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
            }

            //先生成指定类型的解析代码文件
            for (int i = 0; i < ParseCodeGenConfig.Types.Length; i++)
            {
                Type type = ParseCodeGenConfig.Types[i];
                GenParseCode(type);
            }

            //然后生成依赖类型的解析代码文件
            while (genCodeTypes.Count != 0)
            {
                GenParseCode(genCodeTypes.Dequeue());
            }

            //生成静态构造器文件
            GenStaticCtorCode();

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 带制表符的AppendLine
        /// </summary>
        private static void AppendLine(string str, int tabNum = 5)
        {
            for (int i = 0; i < tabNum; i++)
            {
                sb.Append("\t");
            }
            sb.AppendLine(str);
        }

        /// <summary>
        /// 生成静态构造器文件
        /// </summary>
        private static void GenStaticCtorCode()
        { 
            //读取模板文件
            StreamReader sr = new StreamReader(ParseCodeGenConfig.StaticCtorTemplateFilePath);
            string template = sr.ReadToEnd();
            sr.Close();

            for (int i = 0; i < ParseCodeGenConfig.Types.Length; i++)
            {
                Type type = ParseCodeGenConfig.Types[i];
                AppendLine($"GenCodeDict.Add(typeof({type.FullName}), Parse_{type.FullName});",3);
            }

            template = template.Replace("#AddParseCode#", sb.ToString());
            sb.Clear();

            StreamWriter sw = new StreamWriter($"{ParseCodeGenConfig.GenCodeDirPath}/Gen_ParserStaticCtor.cs");
            sw.Write(template);
            sw.Close();
        }

        /// <summary>
        /// 生成解析代码文件
        /// </summary>
        private static void GenParseCode(Type type)
        {
            //读取模板文件
            StreamReader sr = new StreamReader(ParseCodeGenConfig.ParseCodeTemplateFilePath);
            string template = sr.ReadToEnd();
            sr.Close();

            //写入using
            template = template.Replace("#Using#", AppendUsingCode());

            //写入类名
            template = template.Replace("#ClassName#", type.FullName);

            //生成parse代码
            template = template.Replace("#SwitchCaseParse#", AppendSwitchCaseParseCode(type));

            StreamWriter sw = new StreamWriter($"{ParseCodeGenConfig.GenCodeDirPath}/Gen_{type.FullName}_ParseCode.cs");
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
        /// 生成使用switch case进行解析的代码
        /// </summary>
        private static string AppendSwitchCaseParseCode(Type type)
        {

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty))
            {
                AppendCaseCode(pi.PropertyType,pi.Name);
            }

            foreach (FieldInfo fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                AppendCaseCode(fi.FieldType,fi.Name);
            }

            string result = sb.ToString();
            sb.Clear();

            return result;
        }

     
        /// <summary>
        /// 生成case属性和字段名的代码
        /// </summary>
        private static void AppendCaseCode(Type type,string name)
        {
            AppendLine($"case \"{name}\":");

            //基础类型
            if (type == typeof(string))
            {
                AppendAssignmentCode($"temp.{name} = rs.Value.ToString();");
            }
            else if (type == typeof(bool))
            {
                AppendAssignmentCode($"temp.{name} = tokenType == TokenType.True;");
            }
            else if (type == typeof(int) || type == typeof(float) || type == typeof(double))
            {
                AppendAssignmentCode($"temp.{name} = {type.FullName}.Parse(rs.Value.ToString());");
            }
            //数组和List<T>
            else if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
            {
                Type elementType;
                if (type.IsArray)
                {
                    elementType = type.GetElementType();
                }
                else
                {
                    elementType = type.GetGenericArguments()[0];
                }

                AppendLine($"List<{elementType.FullName}> list = new List<{elementType.FullName}>();");

                AppendParseArrayCode(name, elementType);

                if (type.IsArray)
                {
                    AppendLine($"temp.{name} = list.ToArray();");
                }
                else
                {
                    AppendLine($"temp.{name} = list;");
                }
            }
            //字典
            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
              
            }
            //其他类型
            else
            {
                AppendLine($"temp.{name} = Parse_{type.FullName}();");
                genCodeTypes.Enqueue(type);
            }

            AppendLine("break;");
            AppendLine("");
        }

        /// <summary>
        /// 生成赋值代码
        /// </summary>
        private static void AppendAssignmentCode(string insertCode)
        {
            AppendLine("rs = JsonParser.Lexer.GetNextToken(out tokenType);");
            AppendLine(insertCode);
        }

        /// <summary>
        /// 生成解析json数组的代码
        /// </summary>
        private static void AppendParseArrayCode(string name,Type elementType)
        {
            AppendLine($"JsonParser.ParseJsonArrayProcedure(list, null, (userdata11, userdata22, nextTokenType2) =>");
            AppendLine("{");

            //基础类型
            if (elementType == typeof(string))
            {
                AppendAssignmentCode( $"((List<{elementType.FullName}>)userdata11).Add(rs.Value.ToString());");
            }
            else if (elementType == typeof(bool))
            {
                AppendAssignmentCode($"((List<{elementType.FullName}>)userdata11).Add(tokenType == TokenType.True);");
            }
            else if (elementType == typeof(int) || elementType == typeof(float) || elementType == typeof(double))
            {
                AppendAssignmentCode( $"((List<{elementType.FullName}>)userdata11).Add({elementType.FullName}.Parse(rs.Value.ToString()));");
            }
            //不支持数组 List 字典 互相直接嵌套 只能用一个class包装一下
            //自定义类型
            else
            {
                AppendLine($"((List<{elementType.FullName}>)userdata11).Add(Parse_{elementType.FullName}());");
            }

            AppendLine("});");
        }

       
    }
}

