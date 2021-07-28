using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToJson1_Data
{
    public bool b;
    public float n;
    public string s;
    public ToJson1_Data child;

    public Dictionary<string, int> intDict;
    public Dictionary<string, ToJson1_Data> dataDict;

    public int[] ints;
    public List<bool> boolList;

    public List<ToJson1_Data> dataList;
}
