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

        private bool hasNextTokenCache;
        private TokenType nextTokenType;
        private string nextToken;

        public static StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 当前剩余的json文本的首字符
        /// </summary>
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
            hasNextTokenCache = false;
        }

        /// <summary>
        /// 查看下一个token的类型
        /// </summary>
        public TokenType LookNextTokenType()
        {
            if (hasNextTokenCache)
            {
                //有缓存直接返回缓存
                return nextTokenType;
            }

            //没有就get一下
            nextToken = GetNextToken(out nextTokenType);
            hasNextTokenCache = true;
            return nextTokenType;
        }

        /// <summary>
        /// 获取下一个指定类型的token，如果是字符串或数字，那么将值存放在RangeString中
        /// </summary>
        public string GetNextTokenByType(TokenType type)
        {
            string token = GetNextToken(out TokenType resultType);
            if (type != resultType)
            {
                throw new Exception($"NextTokenOfType调用失败，需求{type}但获取到的是{resultType}");
            }
            return token;
        }

        /// <summary>
        /// 获取下一个token
        /// </summary>
        public string GetNextToken(out TokenType type)
        {
            type = default;

            
            if (hasNextTokenCache)
            {
                //有缓存下一个token的信息 直接返回
                type = nextTokenType;

                hasNextTokenCache = false;

                return nextToken;
            }

            //跳过空白字符
            SkipWhiteSpace();

            if (IsEnd)
            {
                //文本结束
                type = TokenType.Eof;
                return null;
            }

            //扫描字面量 分隔符
            switch (CurChar)
            {
                case 'n':
                    type = TokenType.Null;
                    ScanLiteral("null");
                    return null;
                case 't':
                    type = TokenType.True;
                    ScanLiteral("true");
                    return null;
                case 'f':
                    type = TokenType.False;
                    ScanLiteral("false");
                    return null;
                case '[':
                    type = TokenType.LeftBracket;
                    Next();
                    return null;
                case ']':
                    type = TokenType.RightBracket;
                    Next();
                    return null;
                case '{':
                    type = TokenType.LeftBrace;
                    Next();
                    return null;
                case '}':
                    type = TokenType.RightBrace;
                    Next();
                    return null;
                case ':':
                    type = TokenType.Colon;
                    Next();
                    return null;
                case ',':
                    type = TokenType.Comma;
                    Next();
                    return null;
            }

            //扫描数字
            if (char.IsDigit(CurChar) || CurChar == '-')
            {
                string str = ScanNumber();
                type = TokenType.Number;
                return str;
            }

            //扫描字符串
            if (CurChar == '"')
            {
                string str = ScanString();
                type = TokenType.String;
                return str;
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

            // 起始字符是 " 要跳过
            Next();

            while (!IsEnd & CurChar != '"')
            {
                //处理转义字符
                if (CurChar == '\\')
                {
                    if (curIndex == json.Length - 1)
                    {
                        throw new Exception("处理转义字符失败，\\后没有剩余字符了");
                    }

                    Next();
                    switch (CurChar)
                    {
                        case '"':
                            Util.CachedSB.Append('\"');
                            break;
                        case '\\':
                            Util.CachedSB.Append('\\');
                            break;
                        case '/':
                            Util.CachedSB.Append('/');
                            break;
                        case 'b':
                            Util.CachedSB.Append('\b');
                            break;
                        case 'f':
                            Util.CachedSB.Append('\f');
                            break;
                        case 'n':
                            Util.CachedSB.Append('\n');
                            break;
                        case 'r':
                            Util.CachedSB.Append('\r');
                            break;
                        case 't':
                            Util.CachedSB.Append('\t');
                            break;
                        default:
                            throw new Exception("处理转义字符失败，\\后的字符不在可转义范围内");
                    }

                    Next();

                    continue;
                }

                //处理普通字符
                Util.CachedSB.Append(CurChar);
                Next();
            }

            if (IsEnd)
            {
                throw new Exception("字符串扫描失败，不是以双引号结尾的");
            }
            else
            {
                // 末尾也是 " 也要跳过
                Next();
            }

            string str = Util.CachedSB.ToString();
            Util.CachedSB.Clear();

            return str;
        }

    }

}
