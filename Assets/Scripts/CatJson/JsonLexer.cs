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
        private string json;
        private int curIndex;

        private string nextToken;
        private TokenType nextTokenType;

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
        /// 设置Json文本
        /// </summary>
        public void SetJsonText(string json)
        {
            this.json = json;
            curIndex = 0;
            nextToken = null;
        }

        /// <summary>
        /// 查看下一个token的类型
        /// </summary>
        public TokenType LookNextTokenType()
        {
            if (nextToken != null)
            {
                return nextTokenType;
            }

            nextToken = GetNextToken(out nextTokenType);
            return nextTokenType;
        }

        /// <summary>
        /// 获取下一个指定类型的token
        /// </summary>
        public string GetNextTokenOfType(TokenType type)
        {
           string token = GetNextToken(out TokenType reusltType);
            if (type != reusltType)
            {
                throw new Exception($"NextTokenOfType调用失败，需求{type}但提取到的是{token}");
            }
            return token;
        }

        /// <summary>
        /// 获取下一个token
        /// </summary>
        public string GetNextToken(out TokenType type)
        {
            string token = null;

            if (nextToken != null)
            {
                //有缓存下一个token的信息 直接返回
                type = nextTokenType;
                token = nextToken;

                //重置缓存信息
                nextToken = null;

                return token;
            }

            SkipWhiteSpace();

            if (IsEnd)
            {
                //文本结束
                type = TokenType.Eof;
                token = "Eof";
                return token;
            }

            token = null;
            type = default;

            //扫描字面量 分隔符
            switch (CurChar)
            {
                case 'n':
                    token = "null";
                    type = TokenType.Null;
                    ScanLiteral(token);
                    return token;
                case 't':
                    token = "true";
                    type = TokenType.True;
                    ScanLiteral(token);
                    return token;
                case 'f':
                    token = "false";
                    type = TokenType.False;
                    ScanLiteral(token);
                    return token;
                case '[':
                    token = "]";
                    type = TokenType.LeftBracket;
                    Next();
                    return token;
                case ']':
                    token = "]";
                    type = TokenType.RightBracket;
                    Next();
                    return token;
                case '{':
                    token = "{";
                    type = TokenType.LeftBrace;
                    Next();
                    return token;
                case '}':
                    token = "}";
                    type = TokenType.RightBrace;
                    Next();
                    return token;
                case ':':
                    token = ":";
                    type = TokenType.Colon;
                    Next();
                    return token;
                case ',':
                    token = ",";
                    type = TokenType.Comma;
                    Next();
                    return token; 
            }

            //扫描数字
            if (char.IsDigit(CurChar) || CurChar == '-')
            {
                token = ScanNumber();
                type = TokenType.Number;
                return token;
            }

            //扫描字符串
            if (CurChar == '"')
            {
                token = ScanString();
                type = TokenType.String;
                return token;
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
        /// 跳过空白字符
        /// </summary>
        private void SkipWhiteSpace()
        {

            while (!IsEnd && IsWhiteSpace(CurChar))
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
        /// 扫描字面量 null true false
        /// </summary>
        private void ScanLiteral(string keyword)
        {
            if (IsPrefix(keyword))
            {

                Next(keyword.Length);
                return;
            }

            throw new Exception($"Json语法错误,{keyword}解析失败");
        }

        /// <summary>
        /// 扫描数字
        /// </summary>
        private string ScanNumber()
        {

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
                        throw new Exception("数字扫描失败，出现了2次小数点");
                    }
                }

                sb.Append(CurChar);
                Next();

            }

            if (sb.Length == 1 && sb[0] == '-')
            {
                throw new Exception("数字扫描失败,只有1个-号");
            }

            string result = sb.ToString();
            sb.Clear();

            return result;
        }

        /// <summary>
        /// 扫描字符串
        /// </summary>
        private string ScanString()
        {

            // 起始字符是 "
            Next();

            while (!IsEnd & CurChar != '"')
            {
                sb.Append(CurChar);
                Next();
            }

            if (IsEnd && CurChar != '"')
            {
                throw new Exception("字符串扫描失败，不是以双引号结尾的");
            }
            else
            {
                // 末尾也是 "
                Next();
            }

            string result = sb.ToString();
            sb.Clear();

            return result;
        }
    }

}
