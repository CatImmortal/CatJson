﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CatJson.Editor
{
    public static partial class JsonCodeGenerator
    {
        /// <summary>
        /// 生成Json解析代码文件
        /// </summary>
        private static void GenParseJsonCodeFile(Type type)
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
        /// 生成使用ifelse进行所有字段/属性解析的代码
        /// </summary>
        private static string AppendIfElseParseCode(Type type)
        {
            JsonParser.IgnoreSet.TryGetValue(type, out HashSet<string> set);

            //处理属性
            bool isElseIf = false;
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                //属性必须同时具有get set 并且不能是索引器item
                if (pi.SetMethod != null && pi.GetMethod != null && pi.Name != "Item")
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

                    AppendIfElseCode(pi.PropertyType, pi.Name, isElseIf);

                    if (!isElseIf)
                    {
                        isElseIf = true;
                    }

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

                AppendIfElseCode(fi.FieldType, fi.Name, isElseIf);

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
        private static void AppendIfElseCode(Type type, string name, bool isElseIf)
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
            else if (Util.IsNumber(type) || type == typeof(char))
            {
                AppendLine($"temp.{name} = {type.FullName}.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString());");
            }
            else if (type.IsEnum)
            {
                //枚举
                AppendLine($"temp.{name} = ({type.FullName})int.Parse(JsonParser.Lexer.GetNextToken(out _).ToString());");
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
                AppendParseJsonArrayCode(elementType);

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
                Type keyType = type.GetGenericArguments()[0];
                Type valueType = type.GetGenericArguments()[1];
                AppendLine($"{typeFullName} dict = new {typeFullName}();");
                AppendParseJsonDictCode(keyType,valueType);
                AppendLine($"temp.{name} = dict;");
            }
            else if (JsonCodeGenConfig.UseExtensionFuncTypes.Contains(type))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"temp.{name} = ({typeFullName})JsonParser.ParseJsonValueByType(nextTokenType,typeof({typeFullName}));");
            }

            else
            {

                //其他类型 使用生成的解析代码
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
        private static void AppendParseJsonArrayCode(Type elementType, string listName = "list", string userdata1Name = "userdata11", string userdata2Name = "userdata21", string nextTokenTypeName = "nextTokenType1", string dictName = "dict", string keyName = "key1")
        {
            AppendLine($"JsonParser.ParseJsonArrayProcedure({listName}, null, ({userdata1Name}, {userdata2Name}, {nextTokenTypeName}) =>");
            AppendLine("{");

            if (!elementType.IsValueType)
            {
                //元素是引用类型 可能为null
                AppendLine("if (nextTokenType1 == TokenType.Null)");
                AppendLine("{");
                AppendLine(" JsonParser.Lexer.GetNextToken(out _);");
                AppendLine($"((List<{GetTypeFullName(elementType)}>){userdata1Name}).Add(null);");
                AppendLine("return;");
                AppendLine("}");
            }

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
            else if (Util.IsNumber(elementType) || elementType == typeof(char))
            {
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add({elementType.FullName}.Parse(JsonParser.Lexer.GetNextToken(out tokenType).ToString()));");
            }
            else if (elementType.IsEnum)
            {
                //枚举
                AppendLine($"((List<{elementType.FullName}>){userdata1Name}).Add(({elementType.FullName})int.Parse(JsonParser.Lexer.GetNextToken(out _).ToString()));");
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

                AppendParseJsonArrayCode(newElementType, $"{listName}1", $"{userdata1Name}1", $"{userdata2Name}1", $"{nextTokenTypeName}1", $"{dictName}1", $"{keyName}1");



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
                Type keyType = elementType.GetGenericArguments()[0];
                Type valueType = elementType.GetGenericArguments()[1];
                string typeFullName = GetTypeFullName(elementType);
                AppendLine($"{typeFullName} {dictName} = new {typeFullName}();");
                AppendParseJsonDictCode(keyType,valueType, $"{dictName}", $"{userdata1Name}1", $"{userdata2Name}1", $"{keyName}1", $"{nextTokenTypeName}1");
                AppendLine($"{listName}.Add({dictName});");
            }
            else if (JsonCodeGenConfig.UseExtensionFuncTypes.Contains(elementType))
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
        private static void AppendParseJsonDictCode(Type keyType,Type valueType, string dictName = "dict", string userdata1Name = "userdata11", string userdata2Name = "userdata21",string isIntKeyName = "isIntKey1", string keyName = "key1", string nextTokenTypeName = "nextTokenType1", string listName = "list")
        {
            AppendLine($"JsonParser.ParseJsonObjectProcedure({dictName}, null,false, ({userdata1Name}, {userdata2Name},{isIntKeyName},{keyName}, {nextTokenTypeName}) =>");
            AppendLine("{");

            string valueTypeFullName = GetTypeFullName(valueType);
            string addKeyCode = $"{keyName}.ToString()";
            if (keyType == typeof(int))
            {
                addKeyCode = $"int.Parse({keyName}.ToString())";
            }

            if (!valueType.IsValueType)
            {
                //元素是引用类型 可能为null
                AppendLine("if (nextTokenType1 == TokenType.Null)");
                AppendLine("{");
                AppendLine(" JsonParser.Lexer.GetNextToken(out _);");
                AppendLine($"((Dictionary<{keyType.FullName}, {valueTypeFullName}>){userdata1Name}).Add({addKeyCode},null);");
                AppendLine("return;");
                AppendLine("}");
            }

            //基础类型
            if (valueType == typeof(string))
            {
                AppendLine($"((Dictionary<{keyType.FullName}, {valueType.FullName}>){userdata1Name}).Add({addKeyCode},JsonParser.Lexer.GetNextToken(out _).ToString());");
            }
            else if (valueType == typeof(bool))
            {
                AppendLine("JsonParser.Lexer.GetNextToken(out tokenType);");
                AppendLine($"((Dictionary<{keyType.FullName}, {valueType.FullName}>){userdata1Name}).Add({addKeyCode},tokenType == TokenType.True);");
            }
            else if (Util.IsNumber(valueType) || valueType == typeof(char))
            {
                AppendLine($"((Dictionary<{keyType.FullName}, {valueType.FullName}>){userdata1Name}).Add({addKeyCode}, {valueType.FullName}.Parse(JsonParser.Lexer.GetNextToken(out _).ToString()));");
            }
            else if (valueType.IsEnum)
            {
                //枚举
                AppendLine($"((Dictionary<{keyType.FullName}, {valueType.FullName}>){userdata1Name}).Add({addKeyCode},({valueType.FullName})int.Parse(JsonParser.Lexer.GetNextToken(out _).ToString()));");
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

                AppendParseJsonArrayCode(elementType, $"{listName}1", $"{userdata1Name}1", $"{userdata2Name}1", $"{nextTokenTypeName}1", $"{dictName}1", $"{keyName}1");


                if (valueType.IsArray)
                {
                    AppendLine($"((Dictionary<{keyType.FullName}, {valueTypeFullName}>){userdata1Name}).Add({addKeyCode}, {listName}1.ToArray());");
                }
                else
                {
                    AppendLine($"((Dictionary<{keyType.FullName}, {valueTypeFullName}>){userdata1Name}).Add({addKeyCode}, {listName}1);");
                }
            }
            else if (Util.IsDictionary(valueType))
            {
                //字典
                Type newKeyType = valueType.GetGenericArguments()[0];
                Type newValueType = valueType.GetGenericArguments()[1];
                AppendLine($"{valueTypeFullName} {dictName}1 = new {valueTypeFullName}();");
                AppendParseJsonDictCode(newKeyType,newValueType, $"{dictName}1", $"{userdata1Name}1", $"{userdata2Name}1",$"{isIntKeyName}1", $"{keyName}1", $"{nextTokenTypeName}1");
                AppendLine($"((Dictionary<{newKeyType}, {valueTypeFullName}>){userdata1Name}).Add({addKeyCode},{dictName}1);");
            }
            else if (JsonCodeGenConfig.UseExtensionFuncTypes.Contains(valueType))
            {
                //其他类型 使用JsonParser.Extension里的扩展
                AppendLine($"((Dictionary<{keyType.FullName}, {valueTypeFullName}>){userdata1Name}).Add({addKeyCode},({valueType.FullName})JsonParser.ParseJsonValueByType(nextTokenType,typeof({valueTypeFullName})));");
            }
            else
            {
                //自定义类型 使用生成的解析代码
                AppendLine($"((Dictionary<{keyType.FullName}, {valueTypeFullName}>){userdata1Name}).Add({addKeyCode},{GetParseJsonCodeMethodName(valueType)}());");

                if (!GenCodeTypes.Contains(valueType))
                {
                    needGenTypes.Enqueue(valueType);
                }
            }

            AppendLine("});");
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
                Type keyType = type.GetGenericArguments()[0];
                Type valueType = type.GetGenericArguments()[1];
                if (keyType != typeof(int))
                {
                    return $"Dictionary<{keyType},{GetTypeFullName(valueType)}>";
                }
                return $"Dictionary<int,{GetTypeFullName(valueType)}>";
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


