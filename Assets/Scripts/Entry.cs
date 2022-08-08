using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using CatJson;
using UnityEngine;
using UnityEngine.Profiling;
using Debug = UnityEngine.Debug;

public class ReflectionTest
{
    public int intA;
    public string strB;
    public bool boolC;
    public float floatD;
}

public class Entry : MonoBehaviour
{
    
    
    // Start is called before the first frame update
    void Start()
    {

        Application.targetFrameRate = 30;
        
        // string testJson1Text = Resources.Load<TextAsset>("TestJson1").text;
        // Debug.Log(testJson1Text);
        //
        // TestJson1_Root testJson1Object = JsonParser.ParseJson<TestJson1_Root>(testJson1Text);
        // string json = JsonParser.ToJson(testJson1Object);
        // Debug.Log(json);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetValueTest();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetValueTest();
        }
    }

    private void GetValueTest()
    {
        Type type = typeof(ReflectionTest);
        ReflectionTest obj = new ReflectionTest();
        FieldInfo[] fis = type.GetFields(BindingFlags.Public | BindingFlags.Instance );
            
        Stopwatch sw = Stopwatch.StartNew();
        Profiler.BeginSample("Reflection GetValue");
        for (int i = 0; i < 100000; i++)
        {
            foreach (FieldInfo fi in fis)
            {
                object value = fi.GetValue(obj);
            }   
        }
        Profiler.EndSample();
        sw.Stop();
            
        Debug.Log(sw.ElapsedMilliseconds);
            
        UnsafeReflection.AddReflectionInfo(type);
        Dictionary<RangeString, UnsafeFieldInfo> ufis = UnsafeReflection.GetFieldInfos(type);
        sw = Stopwatch.StartNew();
        Profiler.BeginSample("Unsafe Reflection GetValue");
        for (int i = 0; i < 100000; i++)
        {
            foreach (KeyValuePair<RangeString,UnsafeFieldInfo> pair in ufis)
            {
                //object value = pair.Value.GetValue(obj);
            }
        }
        Profiler.EndSample();
        sw.Stop();
            
        Debug.Log(sw.ElapsedMilliseconds);
    }
    
    private void SetValueTest()
    {
        Type type = typeof(ReflectionTest);
        ReflectionTest obj = new ReflectionTest();
        FieldInfo[] fis = type.GetFields(BindingFlags.Public | BindingFlags.Instance );
            
        Stopwatch sw = Stopwatch.StartNew();
        Profiler.BeginSample("Reflection SetValue");
        for (int i = 0; i < 100000; i++)
        {
            foreach (FieldInfo fi in fis)
            {
                if (fi.FieldType == typeof(int))
                {
                    fi.SetValue(obj,111);

                }else if (fi.FieldType == typeof(float))
                {
                    fi.SetValue(obj,111f);
                }else if (fi.FieldType == typeof(string))
                {
                    fi.SetValue(obj,"str");
                }else if (fi.FieldType == typeof(bool))
                {
                    fi.SetValue(obj,true);
                }
            }   
        }
        Profiler.EndSample();
        sw.Stop();
            
        Debug.Log(sw.ElapsedMilliseconds);
            
        UnsafeReflection.AddReflectionInfo(type);
        Dictionary<RangeString, UnsafeFieldInfo> ufis = UnsafeReflection.GetFieldInfos(type);
        sw = Stopwatch.StartNew();
        Profiler.BeginSample("Unsafe Reflection SetValue");
        for (int i = 0; i < 100000; i++)
        {
            foreach (KeyValuePair<RangeString,UnsafeFieldInfo> pair in ufis)
            {
                UnsafeFieldInfo fi = pair.Value;
                // if (fi.FieldType == typeof(int))
                // {
                //     fi.SetValue(obj,111);
                //
                // }else if (fi.FieldType == typeof(float))
                // {
                //     fi.SetValue(obj,111f);
                // }else if (fi.FieldType == typeof(string))
                // {
                //     fi.SetValue(obj,"str");
                // }else if (fi.FieldType == typeof(bool))
                // {
                //     fi.SetValue(obj,true);
                // }
            }
        }
        Profiler.EndSample();
        sw.Stop();
            
        Debug.Log(sw.ElapsedMilliseconds);
    }
}
