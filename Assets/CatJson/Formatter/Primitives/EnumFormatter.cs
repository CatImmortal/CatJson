using System;

namespace CatJson
{
    /// <summary>
    /// 枚举类型的Json格式化器
    /// </summary>
    public class EnumFormatter : IJsonFormatter
    {
        /// <inheritdoc />
        public void ToJson(object value, Type type, Type realType, int depth)
        {
            TextUtil.Append('\"');
            TextUtil.Append(value.ToString());
            TextUtil.Append('\"');
        }

        /// <inheritdoc />
        public object ParseJson(Type type, Type realType)
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.String);
            object enumOBj = Enum.Parse(realType,rs.ToString());
            return enumOBj;
        }
    }
}