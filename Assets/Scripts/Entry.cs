using System.Collections;
using System.Collections.Generic;
using CatJson;
using UnityEngine;

public class Entry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        JsonObject jo = new JsonObject();
        jo["b"] = true;
        jo["num"] = 123;
        jo["str"] = "jsonTest";
        JsonValue[] values = {new JsonValue(4),new JsonValue(5),new JsonValue(6)};
        jo["array"] = values;
        jo["nil"] = null;
        
        JsonObject jo2 = new JsonObject();
        jo2["b"] = false;
        jo2["num"] = 789;
        jo2["str"] = "jsonTest";
        JsonValue[] values2 = {new JsonValue(10.4f),new JsonValue(10.5d),new JsonValue(-100)};
        jo2["array"] = values2;
        jo2["obj"] = jo;
        Debug.Log(JsonParser.ToJson(jo2));
        Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<JsonObject>(JsonParser.ToJson(jo2))));
        
        int[] ints = {1, 2, 3};
        Debug.Log(JsonParser.ToJson(ints));
        Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<int[]>(JsonParser.ToJson(ints))));

        Dictionary<string, string> dict1 = new Dictionary<string, string>()
        {
            {"str1","str11"},
            {"str2","str22"},
            {"str33","str33"},
        };
        Debug.Log(JsonParser.ToJson(dict1));
        Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<Dictionary<string, string>>(JsonParser.ToJson(dict1))));
        
        Dictionary<int, string> dict2 = new Dictionary<int, string>()
        {
            {1,"str11"},
            {2,"str22"},
            {3,"str33"},
        };
        Debug.Log(JsonParser.ToJson(dict2));
        Debug.Log(JsonParser.ToJson(JsonParser.ParseJson<Dictionary<int, string>>(JsonParser.ToJson(dict2))));
    }


}
