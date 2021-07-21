using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.IO;

namespace CatJson.Editor
{
    public static class ParseCodeGenerator
    {
        private static string switchCaseCodeAlignTab = "\t\t\t\t";

        private static Queue<Type> genCodeTypes = new Queue<Type>();

        [MenuItem("CatJson/预生成Json解析代码")]
        private static void GenParseCode()
        {
            //清空旧文件
            DirectoryInfo di = new DirectoryInfo(ParseCodeConfig.GenCodeDirPath);


            foreach (FileInfo item in di.GetFiles())
            {
                item.Delete();
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

            //写入类名
            template = template.Replace("#ClassName#", type.FullName);
            template = template.Replace("#SwitchCase#", GetSwitchCaseCode(type));

            Debug.Log(template);

        }

        private static string GetSwitchCaseCode(Type type)
        {
            foreach (FieldInfo fi in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
            {


                Util.CachedSB.AppendLine($"case \"{fi.Name}\":");

                GetAssignmentCode(fi.FieldType, fi.Name);


                Util.CachedSB.AppendLine("break;");
            }

            string str = Util.CachedSB.ToString();
            Util.CachedSB.Clear();

            return str;
        }

        private static void GetAssignmentCode(Type type,string name)
        {


            if (type == typeof(string))
            {
                GetCaseCode("String", $"temp.{name} = rs.Value.ToString();");
                return;
            }

            if (type == typeof(bool))
            {
                GetCaseCode("True", $"temp.{name} = tokenType == TokenType.True", " || tokenType == TokenType.False");
                return;
            }

            if (type == typeof(int))
            {
                GetCaseCode("Number", $"temp.{name} = int.Parse(rs.Value.ToString());");
                return;
            }

            if (type == typeof(float))
            {
                GetCaseCode("Number", $"temp.{name} = float.Parse(rs.Value.ToString());");
                return;
            }

            if (type == typeof(double))
            {
                GetCaseCode("Number", $"temp.{name} = double.Parse(rs.Value.ToString());");
                return;
            }



            //List<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type elementType = type.GetGenericArguments()[0];
                Util.CachedSB.AppendLine($"temp.{name} = new List<{elementType.FullName}>();");
                Util.CachedSB.AppendLine($"JsonParser.ParseJsonArrayProcedure(temp.{name}, null, (userdata11, userdata22, nextTokenType2) =>");
                Util.CachedSB.AppendLine("{");
                //Util.CachedSB.AppendLine($"{elementType.FullName} item = Gen_{elementType.FullName}();");
                Util.CachedSB.AppendLine($"((List<{elementType}>)userdata11).Add(item);");
                Util.CachedSB.AppendLine("}");
                return;
            }

            //自定义类型
            Util.CachedSB.AppendLine($"temp.{name} = Gen_{type.FullName}();");
            genCodeTypes.Enqueue(type);
        }

        private static void GetCaseCode(string tokenType,string assgimentCode,string ex = "")
        {

            Util.CachedSB.AppendLine("rs = JsonParser.Lexer.GetNextToken(out tokenType);");

 
            Util.CachedSB.AppendLine($"if(tokenType == TokenType.{tokenType}{ex})");


            Util.CachedSB.AppendLine("{");


            Util.CachedSB.AppendLine(assgimentCode);


            Util.CachedSB.AppendLine("}");

            Util.CachedSB.AppendLine("else");

            Util.CachedSB.AppendLine("{");

            Util.CachedSB.AppendLine("throw new Exception(\"resultcode的value类型不正确，当前解析到的是:\" + tokenType);");

            Util.CachedSB.AppendLine("}");
        }
    }
}

