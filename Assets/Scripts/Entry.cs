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

    private TestJson1_Root testJson1Object;

    void Start()
    {
        Application.targetFrameRate = 30;

        testJson1Text = Resources.Load<TextAsset>("TestJson1").text;

        testJson1Object = JsonParser.ParseJson<TestJson1_Root>(testJson1Text);

        UnityJson_Data data = new UnityJson_Data();
        data.v2 = new Vector2(1, 2);
        data.v3 = new Vector3(1, 2, 3);
        data.v4 = new Vector4(1, 2, 3, 4);
        data.rotate = new Quaternion(1, 2, 3, 4);
        data.color = new Color(1, 2, 3, 4);
        data.bounds = new Bounds(new Vector3(1, 2, 3), new Vector3(1, 2, 3));
        data.rect = new Rect(1, 2, 3, 4);
        data.keyframe = new Keyframe(1, 2, 3, 4);
        data.ac = new AnimationCurve(new Keyframe(1, 2, 3, 4), new Keyframe(5, 6, 7, 8));

        string json = JsonParser.ToJson(data);
        Debug.Log(json);
        json = JsonParser.ToJson(data, false);
        Debug.Log(json);

        UnityJson_Data data2 = JsonParser.ParseJson<UnityJson_Data>(json, false);
        Debug.Log(data2);



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

        Profiler.BeginSample("Cat Json GenCode");
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

    /// <summary>
    /// 测试序列化Json节点树为Json文本
    /// </summary>
    private void TestToJsonNodeTree()
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
            string json = JsonParser.ToJson(testJson1Object);
        }
        Profiler.EndSample();

        Profiler.BeginSample("Cat Json GenCode");
        for (int i = 0; i < TestCount; i++)
        {
            string json = JsonParser.ToJson(testJson1Object, false);
        }
        Profiler.EndSample();

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

        JsonObject jo = JsonParser.ParseJson(json1);
        string json2 = JsonParser.ToJson(jo);
        StreamWriter sw2 = File.CreateText(Application.dataPath + "/ToJsonResultByJsonTree.txt");
        sw2.Write(json2);
        sw2.Close();

        string json3 = JsonParser.ToJson(testJson1Object, false);
        StreamWriter sw3 = File.CreateText(Application.dataPath + "/ToJsonResultByCode.txt");
        sw3.Write(json3);
        sw3.Close();


        Debug.Log(json1 == json2 && json2 == json3);
    }
}
