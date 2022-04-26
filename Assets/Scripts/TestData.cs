using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataRoot
{
    public bool b;
    public float num;
    public string str;
    public List<int> intList;
    public Dictionary<string, int> intDict;
    public TestDataItem item;
    public List<TestDataItem> itemList;
    public Dictionary<string, TestDataItem> itemDict;
}

public class TestDataItem
{
    public bool b;
    public float num;
    public string str;
}