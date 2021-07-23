using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;
using UnityEngine.Profiling;
using LitJson;
using Newtonsoft.Json;
using System;
using MiniJSON;
using SimpleJSON;
using System.IO;
using System.Text;
using NetJSON;

public class Entry : MonoBehaviour
{
    public TextAsset json1;
    public TextAsset json2;
    public TextAsset json3;
    public TextAsset json4;
    public TextAsset json5;
    public TextAsset json6;
    public TextAsset json7;

    private string json2Text;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;

        json2Text = json2.text;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //节点树
            TestDeserializeJsonNodeTree();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {

            TestDeserializeJsonObject();
        }



    }




    /// <summary>
    /// 测试反序列化为Json节点树
    /// </summary>
    private void TestDeserializeJsonNodeTree()
    {
        Profiler.BeginSample("Cat Json");
        for (int i = 0; i < 1000; i++)
        {
            JsonObject result2 = JsonParser.ParseJson(json2Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < 1000; i++)
        {
            JsonData result2 = JsonMapper.ToObject(json2Text);
        }
        Profiler.EndSample();


        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < 1000; i++)
        {
           object result2 = JsonConvert.DeserializeObject(json2Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Mini Json");
        for (int i = 0; i < 1000; i++)
        {
           var result2 = Json.Deserialize(json2Text) as Dictionary<string,object>;
        }
        Profiler.EndSample();

        Profiler.BeginSample("Simple Json");
        for (int i = 0; i < 1000; i++)
        {
            JSONNode result2 = JSON.Parse(json2Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Net Json");
        for (int i = 0; i < 1000; i++)
        {
            Dictionary<string,object> result2 = (Dictionary<string, object>)NetJSON.NetJSON.DeserializeObject(json2Text);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// 测试反序列化json数据对象
    /// </summary>
    private void TestDeserializeJsonObject()
    {
        Profiler.BeginSample("Cat Json Reflection");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = JsonParser.ParseJson<Json2_Root>(json2Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Cat Json ParseCode");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = JsonParser.ParseJson<Json2_Root>(json2Text, false);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = JsonMapper.ToObject<Json2_Root>(json2Text);
        }
        Profiler.EndSample();


        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = JsonConvert.DeserializeObject<Json2_Root>(json2Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Net Json");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = NetJSON.NetJSON.Deserialize<Json2_Root>(json2Text);
        }
        Profiler.EndSample();


    }

}
