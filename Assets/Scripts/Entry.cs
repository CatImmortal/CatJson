using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;
using UnityEngine.Profiling;
using LitJson;
using Newtonsoft.Json;

public class Entry : MonoBehaviour
{
    public TextAsset json1;
    public TextAsset json2;
    public TextAsset json3;
    public TextAsset json4;
    public TextAsset json5;
    public TextAsset json6;

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
            TestDeserializeJsonNodeTree();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TestDeserializeJsonObject();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            TestDeserializeJsonObject2();
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
    }

    /// <summary>
    /// 测试反序列化json数据对象，统一基于反射
    /// </summary>
    private void TestDeserializeJsonObject()
    {
        Profiler.BeginSample("Cat Json");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = JsonParser.ParseJson<Json2_Root>(json2Text);
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
    }

    /// <summary>
    /// 测试反序列化json数据对象，CatJson基于预生成代码，其他库基于反射
    /// </summary>
    private void TestDeserializeJsonObject2()
    {
        Profiler.BeginSample("Cat Json");
        for (int i = 0; i < 1000; i++)
        {
            Json2_Root result2 = JsonParser.ParseJson<Json2_Root>(json2Text,false);
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
    }

}
