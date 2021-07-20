using System.Collections.Generic;

public class Json3_ListItem
{
    /// <summary>
    /// 
    /// </summary>
    public List<int> list1 { get; set; }
}

public class Json3_Root
{
    public int[] array { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<Json3_ListItem> list { get; set; }
}