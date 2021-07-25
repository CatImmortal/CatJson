using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// Json解析回调接收接口
    /// </summary>
    public interface IJsonParserCallbackReceiver
    {
        /// <summary>
        /// json解析结束回调
        /// </summary>
        void OnParseJsonEnd();
    }
}

