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

public class Test : MonoBehaviour
{

        
    public int TestCount = 1000;

    private string testJson1Text;

    private TestJson1_Root testJson1Object;

    void Start()
    {
        Application.targetFrameRate = 30;

        testJson1Text = Resources.Load<TextAsset>("TestJson1").text;

        testJson1Object = JsonParser.ParseJson<TestJson1_Root>(testJson1Text);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //反序列化 节点树
            TestParseJsonNodeTree();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            //序列化 节点树
            TestToJsonNodeTree();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //反序列化 Json数据类对象
            TestParseJsonObject();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //序列化 Json数据类对象
            TestToJsonObject();
        }

    }




    /// <summary>
    /// 测试反序列化为Json节点树
    /// </summary>
    private void TestParseJsonNodeTree()
    {
        Profiler.BeginSample("Cat Json");
        for (int i = 0; i < TestCount; i++)
        {
            JsonObject result2 = JsonParser.ParseJson<JsonObject>(testJson1Text);
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
    /// 测试序列化Json节点树为Json文本
    /// </summary>
    private void TestToJsonNodeTree()
    {
        JsonObject result2_1 = JsonParser.ParseJson<JsonObject>(testJson1Text);

        Profiler.BeginSample("Cat Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonParser.ToJson(result2_1);
        }
        Profiler.EndSample();

        JsonData result2_2 = JsonMapper.ToObject(testJson1Text);
        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonMapper.ToJson(result2_2);
        }
        Profiler.EndSample();

        object result2_3 = JsonConvert.DeserializeObject(testJson1Text);
        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonConvert.SerializeObject(result2_3);
        }
        Profiler.EndSample();


        Dictionary<string, object> result2_4 = (Dictionary<string, object>)NetJSON.NetJSON.DeserializeObject(testJson1Text);
        Profiler.BeginSample("Net Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = NetJSON.NetJSON.Serialize(result2_4);
        }
        Profiler.EndSample();

        Dictionary<string, object> result2_5 = MiniJSON.Json.Deserialize(testJson1Text) as Dictionary<string, object>;

        Profiler.BeginSample("Mini Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = MiniJSON.Json.Serialize(result2_5);
        }
        Profiler.EndSample();

        //JSONNode result2_6 = JSON.Parse(testJson1Text);
        //Profiler.BeginSample("Simple Json");
        //for (int i = 0; i < TestCount; i++)
        //{

        //}
        //Profiler.EndSample();

        //MojoJson.JsonValue result2_7 = MojoJson.Json.Parse(testJson1Text);
        //Profiler.BeginSample("Mojo Json");
        //for (int i = 0; i < TestCount; i++)
        //{
         
        //}
        //Profiler.EndSample();
    }

    /// <summary>
    /// 测试反序列化json数据对象
    /// </summary>
    private void TestParseJsonObject()
    {
        Profiler.BeginSample("Cat Json Reflection");
        for (int i = 0; i < TestCount; i++)
        {
            TestJson1_Root result = JsonParser.ParseJson<TestJson1_Root>(testJson1Text);
        }
        Profiler.EndSample();

        // Profiler.BeginSample("Cat Json GenCode");
        // for (int i = 0; i < TestCount; i++)
        // {
        //     TestJson1_Root result = JsonParser.ParseJson<TestJson1_Root>(testJson1Text, false);
        // }
        // Profiler.EndSample();

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



    /// <summary>
    /// 测试指定类型对象序列化为Json文本
    /// </summary>
    public void TestToJsonObject()
    {
        Profiler.BeginSample("Cat Json Reflection");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonParser.ToJson(testJson1Object);
        }
        Profiler.EndSample();

        // Profiler.BeginSample("Cat Json GenCode");
        // for (int i = 0; i < TestCount; i++)
        // {
        //     string json = JsonParser.ToJson(testJson1Object, false);
        // }
        // Profiler.EndSample();

        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonMapper.ToJson(testJson1Object);
        }
        Profiler.EndSample();


        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonConvert.SerializeObject(testJson1Object);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Net Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = NetJSON.NetJSON.Serialize(testJson1Object);
        }
        Profiler.EndSample();
    }

    /// <summary>
    /// Json格式化测试
    /// </summary>
    private void TestJsonFormat()
    {
        

        string json1 = JsonParser.ToJson(testJson1Object);
        StreamWriter sw = File.CreateText(Application.dataPath + "/ToJsonResultByReflection.txt");
        sw.Write(json1);
        sw.Close();

        JsonObject jo = JsonParser.ParseJson<JsonObject>(json1);
        string json2 = JsonParser.ToJson(jo);
        StreamWriter sw2 = File.CreateText(Application.dataPath + "/ToJsonResultByJsonTree.txt");
        sw2.Write(json2);
        sw2.Close();

        // string json3 = JsonParser.ToJson(testJson1Object, false);
        // StreamWriter sw3 = File.CreateText(Application.dataPath + "/ToJsonResultByCode.txt");
        // sw3.Write(json3);
        // sw3.Close();


        //Debug.Log(json1 == json2 && json2 == json3);
    }
}
