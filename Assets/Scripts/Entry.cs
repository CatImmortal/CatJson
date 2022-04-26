using System.Collections;
using System.Collections.Generic;
using CatJson;
using UnityEngine;

public class Entry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // JsonObject jo = new JsonObject();
        // jo["b"] = true;
        // jo["num"] = 123;
        // jo["str"] = "jsonTest";
        // JsonValue[] values = {new JsonValue(4),new JsonValue(5),new JsonValue(6)};
        // jo["array"] = values;
        // jo["nil"] = default;
        //
        // JsonObject jo2 = new JsonObject();
        // jo2["b"] = false;
        // jo2["num"] = 789;
        // jo2["str"] = "jsonTest";
        // JsonValue[] values2 = {new JsonValue(10.4f),new JsonValue(10.5d),new JsonValue(-100)};
        // jo2["array"] = values2;
        // jo2["obj"] = jo;
        // Debug.Log(JsonParser.ToJson(jo2));
        // Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<JsonObject>(JsonParser.ToJson(jo2))));
        //
        // int[] ints = {1, 2, 3};
        // Debug.Log(JsonParser.ToJson(ints));
        // Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<int[]>(JsonParser.ToJson(ints))));
        //
        // Dictionary<string, string> dict1 = new Dictionary<string, string>()
        // {
        //     {"str1","str11"},
        //     {"str2","str22"},
        //     {"str33","str33"},
        // };
        // Debug.Log(JsonParser.ToJson(dict1));
        // Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<Dictionary<string, string>>(JsonParser.ToJson(dict1))));
        //
        // Dictionary<int, string> dict2 = new Dictionary<int, string>()
        // {
        //     {1,"str11"},
        //     {2,"str22"},
        //     {3,"str33"},
        // };
        // Debug.Log(JsonParser.ToJson(dict2));
        // Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<Dictionary<int, string>>(JsonParser.ToJson(dict2))));

        TestDataRoot root = new TestDataRoot()
        {
            b = true,
            num = 3.14f,
            str = "textJson",
            item = new TestDataItem()
            {
                b = false,
                num = 6,
                str = "testDataItem",
            },
            intList = new List<int>() {1, 2, 3, 4},
            itemList = new List<TestDataItem>()
            {
                new TestDataItem() {b = true, num = 7, str = "item1"},
                new TestDataItem() {b = true, num = 8, str = "item2"},
                new TestDataItem() {b = true, num = 9, str = "item3"},
            },
            intDict = new Dictionary<string, int>()
            {
                {"key1", 1},
                {"key2", 2},
                {"key3", 3},
            },
            itemDict = new Dictionary<string, TestDataItem>()
            {
                { "key4", new TestDataItem() {b = true, num = 10, str = "value1"}},
                { "key5", new TestDataItem() {b = true, num = 10, str = "value2"}},
                { "key6", new TestDataItem() {b = true, num = 10, str = "value3"}},
            }
        };
        
        Debug.Log(JsonParser.ToJson(root));
        Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<TestDataRoot>(JsonParser.ToJson(root))));
    }


}
