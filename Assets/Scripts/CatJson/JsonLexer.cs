using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace CatJson
{
    /// <summary>
    /// Json词法分析器
    /// </summary>
    public class JsonLexer
    {
        public JsonLexer(string json)
        {
            this.json = json;
        }

        /// <summary>
        /// 关键字与token类型的映射
        /// </summary>
        private static Dictionary<string, TokenType> keyWordTokenTypeMap = new Dictionary<string, TokenType>
        {
            {"null",TokenType.Null},
            {"true",TokenType.True },
            {"false",TokenType.False }
        };

        private string json;
        private int curIndex;

        public static StringBuilder sb = new StringBuilder();

        public char CurChar
        {
            get
            {
                return json[curIndex];
            }
        }

        /// <summary>
        /// Json文本是否已结束
        /// </summary>
        private bool IsEnd
        {
            get
            {
                return curIndex >= json.Length;
            }
        }

        /// <summary>
        /// 获取下一个token
        /// </summary>
        public void GetNextToken(out TokenType type, out string token)
        {
            SkipWhiteSpace();

            if (IsEnd)
            {
                type = TokenType.Eof;
                token = "Eof";
                return;
            }

            token = null;
            type = default;

            //扫描关键字
            switch (CurChar)
            {
                case 'n':
                    token = "null";
                    type = TokenType.Null;
                    break;
                case 't':
                    token = "true";
                    type = TokenType.True;
                    break;
                case 'f':
                    token = "false";
                    type = TokenType.False;
                    break;
            }
            if (token != null)
            {
                ScanKeyword(token);
                return;
            }

            //扫描数字
            if (char.IsDigit(CurChar) || CurChar == '-')
            {
                token = ScanNumber();
                type = TokenType.Number;
                return;
            }

            throw new Exception("json解析失败");
        }

        /// <summary>
        /// 移动CurIndex
        /// </summary>
        private void Next(int n = 1)
        {
            curIndex += n;
        }

     

        /// <summary>
        /// 剩余json字符串是否以prefix开头
        /// </summary>
        private bool IsPrefix(string prefix)
        {
            int tempCurIndex = curIndex;
            for (int i = 0; i < prefix.Length; i++, tempCurIndex++)
            {
                if (json[tempCurIndex] != prefix[i])
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 解析空白字符
        /// </summary>
        private void SkipWhiteSpace()
        {

            while (!IsEnd&& IsWhiteSpace(CurChar))
            {
                Next();
            }
        }

        /// <summary>
        /// 是否是空白字符
        /// </summary>
        private bool IsWhiteSpace(char c)
        {
            bool result = c == ' ' || c == '\t' || c == '\n' || c == '\r';
            return result;
        }

        /// <summary>
        /// 扫描关键字 null true false
        /// </summary>
        private void ScanKeyword(string keyword)
        {
            if (IsPrefix(keyword))
            {

                Next(keyword.Length);
                return;
            }

            throw new Exception($"Json语法错误,{keyword}解析失败");
        }

        /// <summary>
        /// 扫描浮点数
        /// </summary>
        private string ScanNumber()
        {
            sb.Clear();

            bool hasDot = false;

            //第一个字符是0-9或者-
            sb.Append(CurChar);
            Next();

            while (!IsEnd && (char.IsDigit(CurChar) || CurChar == '.'))
            {
                if (CurChar == '.')
                {
                    if (!hasDot)
                    {
                        hasDot = true;
                    }
                    else
                    {
                        throw new Exception("浮点数扫描失败，出现了2次小数点");
                    }
                }

                sb.Append(CurChar);
                Next();

            }

            //不是被空白字符打断扫描
            if (!IsEnd&& !IsWhiteSpace(CurChar))
            {
                throw new Exception("浮点数扫描失败,数字后有其他字符");
            }

            if (sb.Length == 1 && sb[0] == '-')
            {
                throw new Exception("浮点数扫描失败,只有1个-号");
            }

            return sb.ToString();
        }


    }

}
