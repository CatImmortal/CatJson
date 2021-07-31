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

    private TestJson1_Root result;

    void Start()
    {
        Application.targetFrameRate = 30;

        testJson1Text = Resources.Load<TextAsset>("TestJson1").text;

        result = JsonParser.ParseJson<TestJson1_Root>(testJson1Text);

        ToJson1_Data data = new ToJson1_Data();

        data.b = true;
        data.n = 3.14f;
        data.s = "tojson";

        data.intDict = new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } };
        data.boolList = new List<bool>() { true, false, true };

        ToJson1_Data d = new ToJson1_Data();
        d.b = true;
        d.n = 3.14f;
        d.s = "tojson";

        d.intDict = new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } };
        d.boolList = new List<bool>() { true, false, true };

        data.d = d;

        data.dictDcit = new Dictionary<string, Dictionary<string, ToJson1_Data>>()
        {
            {"key3",new Dictionary<string, ToJson1_Data>(){ { "key31", d },{ "key32", d } } },
            {"key4",new Dictionary<string, ToJson1_Data>(){ { "key41", d },{ "key42", d } } },
        };

        data.listList = new List<List<int>>()
        {
            new List<int>(){1,2,3},
            new List<int>(){4,5,6},
        };

        string json = JsonParser.ToJson(data);
        StreamWriter sw = File.CreateText(Application.dataPath + "/ToJsonResultByReflection.txt");
        sw.Write(json);
        sw.Close();

        JsonObject jo = JsonParser.ParseJson(json);
        string json2 = JsonParser.ToJson(jo);
        StreamWriter sw2 = File.CreateText(Application.dataPath + "/ToJsonResultByJsonTree.txt");
        sw2.Write(json2);
        sw2.Close();

        //string json2 = JsonParser.ToJson(data,false);
        //StreamWriter sw2 = File.CreateText(Application.dataPath + "/ToJsonResultByCode.txt");
        //sw2.Write(json2);
        //sw2.Close();



    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //反序列化 节点树
            TestParseJsonNodeTree();
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
    private void TestParseJsonObject()
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

    public void TestToJsonNodeTree()
    {

    }

    /// <summary>
    /// 测试指定类型对象序列化为Json文本
    /// </summary>
    public void TestToJsonObject()
    {
        Profiler.BeginSample("Cat Json Reflection");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonParser.ToJson(result);
        }
        Profiler.EndSample();

        //Profiler.BeginSample("Cat Json ParseCode");
        //for (int i = 0; i < TestCount; i++)
        //{

        //}
        //Profiler.EndSample();

        Profiler.BeginSample("Lit Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonMapper.ToJson(result);
        }
        Profiler.EndSample();


        Profiler.BeginSample("Newtonsoft Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonConvert.SerializeObject(result);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Net Json");
        for (int i = 0; i < TestCount; i++)
        {
            string json = NetJSON.NetJSON.Serialize(result);
        }
        Profiler.EndSample();
    }
}
