using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;
public class Entry : MonoBehaviour
{
    public TextAsset json;

    // Start is called before the first frame update
    void Start()
    {
        JsonLexer lexer = new JsonLexer();
        lexer.SetJsonText(json.text);

        JsonObject obj = JsonParser.ParseJson(json.text);
        Debug.Log(obj);

        //Debug.Log(obj["array"]);
        //Debug.Log(obj["type"]);
        //Debug.Log(obj["null"]);
        //Debug.Log(obj["number"]);
        //Debug.Log(obj["object"]);
    }

    
}
