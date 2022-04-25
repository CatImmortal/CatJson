using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Json格式化器接口
/// </summary>
public interface IJsonFormatter
{
    void ToJson(object value,int depth);
    object ParseJson();
}

