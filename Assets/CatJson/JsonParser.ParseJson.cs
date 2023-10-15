using System;

namespace CatJson
{
    public partial class JsonParser
    {
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        public T ParseJson<T>(string json)
        {
            Lexer.SetJsonText(json);

            T result = (T) InternalParseJson(typeof(T));
            
            Lexer.SetJsonText(null);
            
            return result;
        }

        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        public object ParseJson(string json, Type type)
        {
            Lexer.SetJsonText(json);
            
            object result = InternalParseJson(type);
            
            Lexer.SetJsonText(null);
            
            return result;
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        internal T ParseJson<T>()
        {
            return (T) InternalParseJson(typeof(T));
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        internal object InternalParseJson(Type type)
        {
            if (Lexer.LookNextTokenType() == TokenType.Null)
            {
                return nullFormatter.ParseJson(this,null);
            }
            
            object result;
            
            if (ParserHelper.TryParseRealType(this,out var realType))
            {
                //多态处理
                result = polymorphicFormatter.ParseJson(this, realType);
            }
            else if (formatterDict.TryGetValue(type, out IJsonFormatter formatter))
            {
                //使用通常的formatter处理
                result = formatter.ParseJson(this, type);
            }
            else if (type.IsGenericType && formatterDict.TryGetValue(type.GetGenericTypeDefinition(), out formatter))
            {
                //使用泛型类型formatter处理
                result = formatter.ParseJson(this,type);
            }
            else if (type.IsEnum)
            {
                //使用枚举formatter处理
                result = enumFormatter.ParseJson(this, type);
            }
            else if (type.IsArray)
            {
                //使用数组formatter处理
                result = arrayFormatter.ParseJson(this,type);
            }
            else
            {
                //使用反射formatter处理
                result = reflectionFormatter.ParseJson(this,type);
            }

            if (result is IJsonParserCallbackReceiver receiver)
            {
                //触发序列化结束回调
                receiver.OnParseJsonEnd();
            }

            return result;
        }
    }
}