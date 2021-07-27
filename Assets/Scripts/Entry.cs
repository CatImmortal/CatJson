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
using MojoJson;

public class Entry : MonoBehaviour
{
    
    public int TestCount = 1000;

    private string testJson1Text;
    
    void Start()
    {
        Application.targetFrameRate = 30;

        testJson1Text = Resources.Load<TextAsset>("TestJson1").text;

        string toJsonTest = Resources.Load<TextAsset>("ToJsonTest").text;
        JsonObject jo = JsonParser.ParseJson(testJson1Text);
        string json = JsonParser.ToJson(jo);
        Debug.Log(json);

        StreamWriter sw = File.CreateText(Application.dataPath + "/ToJsonResult.txt");
        sw.Write(json);
        sw.Close();
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
            //Json数据类对象
            TestDeserializeJsonObject();
        }



    }




    /// <summary>
    /// 测试反序列化为Json节点树
    /// </summary>
    private void TestDeserializeJsonNodeTree()
    {
        Profiler.BeginSample("Cat Json");
        for (int i = 0; i < TestCount; i++)
        {
            JsonObject result2 = JsonParser.ParseJson(testJson1Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < TestCount; i++)
        {
            JsonData result2 = JsonMapper.ToObject(testJson1Text);
        }
        Profiler.EndSample();


        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < TestCount; i++)
        {
            object result2 = JsonConvert.DeserializeObject(testJson1Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Net Json");
        for (int i = 0; i < TestCount; i++)
        {
            Dictionary<string, object> result2 = (Dictionary<string, object>)NetJSON.NetJSON.DeserializeObject(testJson1Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Mini Json");
        for (int i = 0; i < TestCount; i++)
        {
            Dictionary<string, object> result2 = MiniJSON.Json.Deserialize(testJson1Text) as Dictionary<string, object>;
        }
        Profiler.EndSample();

        Profiler.BeginSample("Simple Json");
        for (int i = 0; i < TestCount; i++)
        {
            JSONNode result2 = JSON.Parse(testJson1Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Mojo Json");
        for (int i = 0; i < TestCount; i++)
        {
            MojoJson.JsonValue result2 = MojoJson.Json.Parse(testJson1Text);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// 测试反序列化json数据对象
    /// </summary>
    private void TestDeserializeJsonObject()
    {
        Profiler.BeginSample("Cat Json Reflection");
        for (int i = 0; i < TestCount; i++)
        {
            TestJson1_Root result = JsonParser.ParseJson<TestJson1_Root>(testJson1Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Cat Json ParseCode");
        for (int i = 0; i < TestCount; i++)
        {
            TestJson1_Root result = JsonParser.ParseJson<TestJson1_Root>(testJson1Text, false);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < TestCount; i++)
        {
            TestJson1_Root result = JsonMapper.ToObject<TestJson1_Root>(testJson1Text);
        }
        Profiler.EndSample();


        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < TestCount; i++)
        {
            TestJson1_Root result = JsonConvert.DeserializeObject<TestJson1_Root>(testJson1Text);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Net Json");
        for (int i = 0; i < TestCount; i++)
        {
            TestJson1_Root result = NetJSON.NetJSON.Deserialize<TestJson1_Root>(testJson1Text);
        }
        Profiler.EndSample();


    }

}
