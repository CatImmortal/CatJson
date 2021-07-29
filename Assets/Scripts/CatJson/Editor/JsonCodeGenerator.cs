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
    public static class JsonCodeGenerator
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

            GenCodeTypes.Clear();
            needGenTypes.Clear();

            //生成Root的解析代码
            List<Type> types = GetGenParseCodeTypes();
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
        /// 生成Json解析代码文件
        /// </summary>
        private static void GenJsonParseCode(Type type)
        {
            //读取模板文件
            StreamReader sr;

            if (type.IsValueType)
            {
                //值类型
                sr = new StreamReader(JsonCodeGenConfig.ParseStructCodeTemplateFilePath);
            }
            else
            {
                //引用类型
                sr = new StreamReader(JsonCodeGenConfig.ParseClassCodeTemplateFilePath);
            }

            string template = sr.ReadToEnd();
            sr.Close();

            //写入using
            template = template.Replace("#Using#", AppendUsingCode());

            //写入类名
            template = template.Replace("#ClassName#", type.FullName);

            //写入解析方法名
            template = template.Replace("#MethodName#", GetParseJsonCodeMethodName(type));

            //生成解析代码
            template = template.Replace("#IfElseParse#", AppendIfElseParseCode(type));

            StreamWriter sw = new StreamWriter($"{JsonCodeGenConfig.ParseJsonCodeDirPath}/Gen_{type.FullName.Replace(".", "_")}_ParseJsonCode.cs");
            sw.Write(template);
            sw.Close();
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
        /// 生成使用ifelse进行所有字段/属性解析的代码
        /// </summary>
        private static string AppendIfElseParseCode(Type type)
        {
            //处理属性
            bool isElseIf = false;
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //属性必须同时具有get set 并且不能是索引器item
                if (pi.SetMethod != null && pi.GetMethod != null && pi.Name != "Item")
                {
                    AppendIfElseCode(pi.PropertyType, pi.Name,isElseIf);

                    if (!isElseIf)
                    {
                        isElseIf = true;
                    }
                   
                }
               
            }

            //处理字段
            isElseIf = false;
            foreach (FieldInfo fi in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                AppendIfElseCode(fi.FieldType, fi.Name,isElseIf);

                if (!isElseIf)
                {
                    isElseIf = true;
                }
            }

            string result = sb.ToString();
            sb.Clear();

            return result;
        }

        /// <summary>
        /// 生成解析单个字段/属性的ifelse代码
        /// </summary>
        private static void AppendIfElseCode(Type type,string name,bool isElseIf)
        {
            if (isElseIf)
            {
                AppendLine($"else if (key.Equals(new RangeString(\"{name}\")))");
            }
            else
            {
                AppendLine($"if (key.Equals(new RangeString(\"{name}\")))");
            }

            AppendLine("{");

            string typeFullName = GetTypeFullName(type);

            //基础类型
            if (type == typeof(string))
            {
                AppendLine($"temp.{name} = JsonParser.Lexer.GetNextToken(out tokenType).ToString();");
            }
            else if (type == typeof(bool))
            {
                AppendLine("JsonParser.Lexer.GetNextToken(out tokenType);");
                AppendLine($"temp.{name} = tokenType == TokenType.True;");
            }
            else if (Util.IsNumber(type))
            {
                AppendLine($"temp.{name} = {type.FullName}.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());");
            }
            else if (Util.IsArrayOrList(type))
            {
                //数组和List<T>
                Type elementType;
                if (type.IsArray)
                {
                    elementType = type.GetElementType();
                }
                else
                {
                    elementType = type.GetGenericArguments()[0];
                }

                typeFullName = GetTypeFullName(elementType);  //获取正确的元素类型名
                AppendLine($"List<{typeFullName}> list = new List<{typeFullName}>();");

                //解析Json数组
                AppendParseArrayCode(elementType);

                if (type.IsArray)
                {
                    AppendLine($"temp.{name} = list.ToArray();");
                }
                else
                {
                    AppendLine($"temp.{name} = list;");
                }
            }
            else if (Util.IsDictionary(type))
            {
                //字典
                Type valueType = type.GetGenericArguments()[1];
                AppendLine($"{typeFullName} dict = new {typeFullName}();");
                AppendParseDictCode(valueType);
                AppendLine($"temp.{name} = dict;");
            }
            else if (JsonCodeGenConfig.ExtensionParseTypes.Contains(type))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"temp.{name} = ({typeFullName})JsonParser.ParseJsonValueByType(nextTokenType,typeof({typeFullName}));");
            }
            else
            {

                //其他类型 生成解析代码
                AppendLine($"temp.{name} = {GetParseJsonCodeMethodName(type)}();");

                if (!GenCodeTypes.Contains(type))
                {
                    needGenTypes.Enqueue(type);
                }
            }

            AppendLine("}");

        }

        /// <summary>
        /// 生成解析json数组的代码
        /// </summary>
        private static void AppendParseArrayCode(Type elementType, string listName = "list", string userdata1Name = "userdata11", string userdata2Name = "userdata21", string nextTokenTypeName = "nextTokenType1", string dictName = "dict",string keyName = "key1")
        {
            AppendLine($"JsonParser.ParseJsonArrayProcedure({listName}, null, ({userdata1Name}, {userdata2Name}, {nextTokenTypeName}) =>");
            AppendLine("{");

            //基础类型
            if (elementType == typeof(string))
            {
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add(JsonParser.Lexer.GetNextToken(out tokenType).ToString());");
            }
            else if (elementType == typeof(bool))
            {
                AppendLine("JsonParser.Lexer.GetNextToken(out tokenType);");
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add(tokenType == TokenType.True);");
            }
            else if (Util.IsNumber(elementType))
            {
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add({elementType.FullName}.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString()));");
            }
            else if (Util.IsArrayOrList(elementType))
            {
                //数组 List<T>
                Type newElementType;
                if (elementType.IsArray)
                {
                    newElementType = elementType.GetElementType();
                }
                else
                {
                    newElementType = elementType.GetGenericArguments()[0];
                }

                string typeFullName = GetTypeFullName(newElementType);

                AppendLine($"List<{typeFullName}> {listName}1 = new List<{typeFullName}>();");

                AppendParseArrayCode(newElementType, $"{listName}1", $"{userdata1Name}1", $"{userdata2Name}1", $"{nextTokenTypeName}1", $"{dictName}1", $"{keyName}1");



                if (elementType.IsArray)
                {
                    AppendLine($"{listName}.Add({listName}1.ToArray());");
                }
                else
                {
                    AppendLine($"{listName}.Add({listName}1);");
                }
            }
            else if (Util.IsDictionary(elementType))
            {
                //字典
                Type valueType = elementType.GetGenericArguments()[1];
                string typeFullName = GetTypeFullName(elementType);
                AppendLine($"{typeFullName} {dictName} = new {typeFullName}();");
                AppendParseDictCode(valueType, $"{dictName}", $"{userdata1Name}1", $"{userdata2Name}1", $"{keyName}1", $"{nextTokenTypeName}1");
                AppendLine($"{listName}.Add({dictName});");
            }
            else if (JsonCodeGenConfig.ExtensionParseTypes.Contains(elementType))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add(({elementType.FullName})JsonParser.ParseJsonValueByType(nextTokenType,typeof({elementType.FullName})));");

            }
            else
            {
                //自定义类型 使用生成的解析代码
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add({GetParseJsonCodeMethodName(elementType)}());");

                if (!GenCodeTypes.Contains(elementType))
                {
                    needGenTypes.Enqueue(elementType);
                }
            }

            AppendLine("});");
        }

        /// <summary>
        /// 生成解析字典的代码
        /// </summary>
        private static void AppendParseDictCode(Type valueType,string dictName = "dict",string userdata1Name = "userdata11",string userdata2Name = "userdata21",string keyName = "key1",string nextTokenTypeName = "nextTokenType1",string listName = "list")
        {
            AppendLine($"JsonParser.ParseJsonObjectProcedure({dictName}, null, ({userdata1Name}, {userdata2Name},{keyName}, {nextTokenTypeName}) =>");
            AppendLine("{");

            string valueTypeFullName = GetTypeFullName(valueType);

            //基础类型
            if (valueType == typeof(string))
            {
                AppendLine($"((Dictionary<string, {valueType.FullName}>){userdata1Name}).Add({keyName}.ToString(),JsonParser.Lexer.GetNextToken(out _).ToString());");
            }
            else if (valueType == typeof(bool))
            {
                AppendLine("JsonParser.Lexer.GetNextToken(out tokenType);");
                AppendLine($"((Dictionary<string, {valueType.FullName}>){userdata1Name}).Add({keyName}.ToString(),tokenType == TokenType.True);");
            }
            else if (Util.IsNumber(valueType))
            {
                AppendLine($"((Dictionary<string, {valueType.FullName}>){userdata1Name}).Add({keyName}.ToString(), {valueType.FullName}.Parse(JsonParser.Lexer.GetNextToken(out _).ToString()));");
            }
            else if (Util.IsArrayOrList(valueType))
            {
                //数组和List<T>
                Type elementType;
                if (valueType.IsArray)
                {
                    elementType = valueType.GetElementType();
                }
                else
                {
                    elementType = valueType.GetGenericArguments()[0];
                }

                string elementTypeFullName = GetTypeFullName(elementType);

                AppendLine($"List<{elementTypeFullName}> {listName}1 = new List<{elementTypeFullName}>();");

                AppendParseArrayCode(elementType, $"{listName}1", $"{userdata1Name}1", $"{userdata2Name}1", $"{nextTokenTypeName}1", $"{dictName}1", $"{keyName}1");

 
                if (valueType.IsArray)
                {
                    AppendLine($"((Dictionary<string, {valueTypeFullName}>){userdata1Name}).Add({keyName}.ToString(), {listName}1.ToArray());");
                }
                else
                {
                    AppendLine($"((Dictionary<string, {valueTypeFullName}>){userdata1Name}).Add({keyName}.ToString(), {listName}1);");
                }
            }
            else if (Util.IsDictionary(valueType))
            {
                //字典
                Type newValueType = valueType.GetGenericArguments()[1];
                AppendLine($"{valueTypeFullName} {dictName}1 = new {valueTypeFullName}();");
                AppendParseDictCode(newValueType,$"{dictName}1", $"{userdata1Name}1", $"{userdata2Name}1", $"{keyName}1", $"{nextTokenTypeName}1");
                AppendLine($"((Dictionary<string, {valueTypeFullName}>){userdata1Name}).Add({keyName}.ToString(),{dictName}1);");
            }
            else if (JsonCodeGenConfig.ExtensionParseTypes.Contains(valueType))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"((Dictionary<string, {valueTypeFullName}>){userdata1Name}).Add({keyName}.ToString(),({valueType.FullName})JsonParser.ParseJsonValueByType(nextTokenType,typeof({valueTypeFullName})));");
            }
            else
            {
                //自定义类型 使用生成的解析代码
                AppendLine($"((Dictionary<string, {valueTypeFullName}>){userdata1Name}).Add({keyName}.ToString(),{GetParseJsonCodeMethodName(valueType)}());");

                if (!GenCodeTypes.Contains(valueType))
                {
                    needGenTypes.Enqueue(valueType);
                }
            }

            AppendLine("});");
        }

        /// <summary>
        /// 获取需要生成解析代码的json数据类
        /// </summary>
        private static List<Type> GetGenParseCodeTypes()
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
        /// 获取类型对应的解析方法名
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
    }
}

