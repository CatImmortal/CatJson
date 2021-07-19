using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;
using UnityEngine.Profiling;

public class Entry : MonoBehaviour
{
    public TextAsset json1;
    public TextAsset json2;
    public TextAsset json3;

    private string json1Text;
    private string json2Text;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;

        json1Text = json1.text;
        json2Text = json2.text;

        Test();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Profiler.BeginSample("Cat Json");
            for (int i = 0; i < 1000; i++)
            {
                JsonObject obj = JsonParser.ParseJson(json2Text);
            }
            Profiler.EndSample();
        }
    }

    private void Test()
    {
        JsonObject obj = JsonParser.ParseJson(json3.text);
        Debug.Log(obj);

        //Debug.Log(obj["array"]);
        //Debug.Log(obj["type"]);
        //Debug.Log(obj["null"]);
        //Debug.Log(obj["number"]);
        //Debug.Log(obj["object"]);
        //Debug.Log(obj["string"]);

    }


}
