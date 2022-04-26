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

public class TestPolymorph
{
    public object intA;
    public object floatB;
    public object boolC;
    public object strD;
    
    public object intList;
    public object intDict;
    public IList strList;
    public IDictionary strDict;
    public TestBase testChlid;
    
    public object childList;
}

public class TestBase
{
    public bool baseBool;
    public int baseInt;
    public string baseStr;
}

public class TestChild : TestBase
{
    public bool ChildBool;
    public int ChildInt;
    public string ChlidStr;
}

public class TestBase2
{
    public TestBase2 Base;
}

public class TestChild2 : TestBase2
{
    //public int intA;
}
