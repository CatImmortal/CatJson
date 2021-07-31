using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CatJson;
[GenJsonCodeRoot]
public class ToJson1_Data
{
    public bool b;
    public float n;
    public string s;

    public Dictionary<string, int> intDict;

    public List<bool> boolList;

    public ToJson1_Data d;

    public Dictionary<string, Dictionary<string, ToJson1_Data>> dictDcit;
    public List<List<int>> listList;

}

