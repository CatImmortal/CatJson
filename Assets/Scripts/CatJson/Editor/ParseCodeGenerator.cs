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
    public static class ParseCodeGenerator
    {
        private static string switchCaseCodeAlignTab = "\t\t\t\t";

        private static Queue<Type> genCodeTypes = new Queue<Type>();

        private static StringBuilder sb = new StringBuilder();

        [MenuItem("CatJson/预生成Json解析代码")]
        private static void GenParseCode()
        {
            
            if (!Directory.Exists(ParseCodeConfig.GenCodeDirPath))
            {
                Directory.CreateDirectory(ParseCodeConfig.GenCodeDirPath);
            }
            else
            {
                //清空旧文件
                DirectoryInfo di = new DirectoryInfo(ParseCodeConfig.GenCodeDirPath);
                foreach (FileInfo fi in di.GetFiles())
                {
                    fi.Delete();
                }
            }

            for (int i = 0; i < ParseCodeConfig.Types.Length; i++)
            {
                Type type = ParseCodeConfig.Types[i];
                GenParseCode(type);
            }

            while (genCodeTypes.Count != 0)
            {
                GenParseCode(genCodeTypes.Dequeue());
            }

            AssetDatabase.Refresh();
        }

        private static void GenParseCode(Type type)
        {
            //读取模板文件
            StreamReader sr = new StreamReader(ParseCodeConfig.TemplateFilePath);
            string template = sr.ReadToEnd();
            sr.Close();

            //写入using
            template = template.Replace("#Using#", GenUsingCode());

            //写入类名
            template = template.Replace("#ClassName#", type.FullName);

            //生成parse代码
            template = template.Replace("#ParseObj#", GenParseObjectCode(type));

            Debug.Log(template);

            StreamWriter sw = new StreamWriter($"{ParseCodeConfig.GenCodeDirPath}/Gen_{type.FullName}_ParseCode.cs");
            sw.Write(template);
            sw.Close();
        }

        /// <summary>
        /// 生成using代码
        /// </summary>
        private static string GenUsingCode()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            string result = sb.ToString();
            sb.Clear();
            return result;
        }

        /// <summary>
        /// 生成解析json数据对象的代码
        /// </summary>
        private static string GenParseObjectCode(Type type)
        {
            sb.AppendLine($"JsonParser.ParseJsonObjectProcedure(obj, null, (userdata1, userdata2, key, nextTokenType) =>");
            sb.AppendLine("{");

            sb.AppendLine($"{type.FullName} temp = ({type.FullName})userdata1;");
            sb.AppendLine("RangeString? rs;");
            sb.AppendLine("TokenType tokenType;");

            sb.AppendLine("switch (key.ToString())");
            sb.AppendLine("{");

            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.GetProperty))
            {
                GenCaseCode(pi.PropertyType,pi.Name);
            }

            foreach (FieldInfo fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                GenCaseCode(fi.FieldType,fi.Name);
            }

            sb.AppendLine("}");

            sb.AppendLine("});");

            string result = sb.ToString();
            sb.Clear();

            return result;
        }

     
        /// <summary>
        /// 生成case属性和字段名的代码
        /// </summary>
        private static void GenCaseCode(Type type,string name)
        {
            sb.AppendLine($"case \"{name}\":");

            //基础类型
            if (type == typeof(string))
            {
                GenTokenTypeCheckCode(name,"String", $"temp.{name} = rs.Value.ToString();");
            }
            else if (type == typeof(bool))
            {
                GenTokenTypeCheckCode(name,"True", $"temp.{name} = tokenType == TokenType.True;", "|| tokenType == TokenType.False");
            }
            else if (type == typeof(int) || type == typeof(float) || type == typeof(double))
            {
                GenTokenTypeCheckCode(name, "Number", $"temp.{name} = {type.FullName}.Parse(rs.Value.ToString());");
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

                sb.AppendLine($"List<{elementType.FullName}> list = new List<{elementType.FullName}>();");

                GenParseArrayCode(name, elementType);

                if (type.IsArray)
                {
                    sb.AppendLine($"temp.{name} = list.ToArray();");
                }
                else
                {
                    sb.AppendLine($"temp.{name} = list;");
                }
            }
            //字典
            else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
              
            }
            //其他类型
            else
            {

            }

            sb.AppendLine("break;");
        }

        /// <summary>
        /// 生成token类型检查代码
        /// </summary>
        private static void GenTokenTypeCheckCode(string name, string tokenType,string insertCode,string exCheckCode = "")
        {
            sb.AppendLine("rs = JsonParser.Lexer.GetNextToken(out tokenType);");
            sb.AppendLine($"if (tokenType == TokenType.{tokenType}{exCheckCode})");
            sb.AppendLine("{");
            sb.AppendLine(insertCode);
            sb.AppendLine("}");
            sb.AppendLine("else if(tokenType != TokenType.Null)");
            sb.AppendLine("{");
            sb.AppendLine($"throw new System.Exception(\"{name}的value类型不正确，当前解析到的是: \" + tokenType);");
            sb.AppendLine("}");
        }

        /// <summary>
        /// 生成解析json数组的代码
        /// </summary>
        private static void GenParseArrayCode(string name,Type elementType)
        {
            sb.AppendLine($"JsonParser.ParseJsonArrayProcedure(temp.{name}, null, (userdata11, userdata22, nextTokenType2) =>");
            sb.AppendLine("{");

            //基础类型
            if (elementType == typeof(string))
            {
                GenTokenTypeCheckCode(name, "String", $"((List<{elementType.FullName}>)userdata11).Add(rs.Value.ToString());");
            }
            else if (elementType == typeof(bool))
            {
                GenTokenTypeCheckCode(name, "True", $"((List<{elementType.FullName}>)userdata11).Add(tokenType == TokenType.True);", "|| tokenType == TokenType.False");
            }
            else if (elementType == typeof(int) || elementType == typeof(float) || elementType == typeof(double))
            {
                GenTokenTypeCheckCode(name, "Number", $"((List<{elementType.FullName}>)userdata11).Add({elementType.FullName}.Parse(rs.Value.ToString()));");
            }
            //不支持数组 List 字典 互相直接嵌套 只能用一个class包装一下
            //自定义类型
            else
            {

            }

            sb.AppendLine("});");
        }


    }
}

