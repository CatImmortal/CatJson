using System.Collections.Generic;
using CatJson;


public class Json2_ListItem
{
    /// <summary>
    /// 
    /// </summary>
    public string datetime { get; set; }
    /// <summary>
    /// 已收件
    /// </summary>
    public string remark { get; set; }
    /// <summary>
    /// 台州市
    /// </summary>
    public string zone { get; set; }
}


public class Json2_Result
{
    /// <summary>
    /// 顺丰
    /// </summary>
    public string company { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string com { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string no { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public List<Json2_ListItem> list { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int status { get; set; }
}

//[GenParseCodeRoot]
public class Json2_Root
{
    /// <summary>
    /// 
    /// </summary>
    public string resultcode { get; set; }
    /// <summary>
    /// 成功的返回
    /// </summary>
    public string reason { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public Json2_Result result { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public int error_code { get; set; }
}