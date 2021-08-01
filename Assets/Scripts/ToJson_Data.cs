using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;
[GenJsonCodeRoot]
public class ToJson_Data
{
    public List<List<ToJson_Data>> dataListList;
    public Dictionary<string, Dictionary<string,ToJson_Data>> dataDictDict;
}
