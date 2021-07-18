using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;

public class Entry : MonoBehaviour
{

    private void Start()
    {
        TestParse();
    }

    private void TestParse()
    {
        JsonData data = JsonParser.FromJson("null");
        data = JsonParser.FromJson("true");
        data = JsonParser.FromJson("false");

        data = JsonParser.FromJson("-1");
        data = JsonParser.FromJson("-1.1");
        data = JsonParser.FromJson("-56465.6454");
        data = JsonParser.FromJson("1");
        data = JsonParser.FromJson("1.1");
        data = JsonParser.FromJson("56465.6454");
    }



    private void TestError()
    {
        JsonData data = JsonParser.FromJson("nullx");
    }
}
